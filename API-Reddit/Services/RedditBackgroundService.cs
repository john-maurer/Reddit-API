using API_Reddit.Model.Response;
using Newtonsoft.Json;
using RedditAuthorizationFilter.Interfaces;
using RedditClient;
using System.Collections.Concurrent;
using API_Reddit.Model;
using RedditClient.Verbs;
using Newtonsoft.Json.Linq;

namespace API_Reddit.Services
{
    public class RedditBackgroundService : BackgroundService
    {
        private readonly ILogger<RedditBackgroundService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISendMessage _redditClient;
        private readonly ITokenService _tokenService;
        private readonly ConcurrentDictionary<string, int> _userPostCounts = new();
        private readonly ConcurrentDictionary<string, int> _postUpvotes = new();

        private RateLimiting RateLimits(HttpResponseMessage message)
        {
            var result = new RateLimiting();

            result.Used = message.Headers.Contains("x-ratelimit-used")
                ? float.Parse(message.Headers.GetValues("x-ratelimit-used").FirstOrDefault()!)
                : -1;
            result.Remaining = message.Headers.Contains("x-ratelimit-remaining")
                ? float.Parse(message.Headers.GetValues("x-ratelimit-remaining").FirstOrDefault()!)
                : -1;
            result.Reset = message.Headers.Contains("x-ratelimit-reset")
                ? float.Parse(message.Headers.GetValues("x-ratelimit-reset").FirstOrDefault()!)
                : -1;

            return result;
        }

        public RedditBackgroundService(ISendMessage client, ITokenService tokenService, IConfiguration configuration, ILogger<RedditBackgroundService> logger)
        {
            _redditClient = client;
            _tokenService = tokenService;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Beginning SubReddit Stats Pooling...\n");

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                var subReddits = _configuration.GetSection("SubReddits").Value!.Trim().Split(",");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var token = await _tokenService.GetToken();

                    if (token != null)
                    {
                        foreach (var subReddit in subReddits)
                        {
                            var response = await _redditClient.SendRequestAsync<Get>($"https://oauth.reddit.com/r/{subReddit}/new.json?limit={_configuration.GetSection("PoolingLimit").Value}", token.AccessToken);
                            var result = new BaseResponse
                            {
                                RateLimiting = RateLimits(response)
                            };

                            foreach (var post in JObject.Parse(await response.Content.ReadAsStringAsync()!)["data"]!["children"]!)
                            {
                                var postId = post["data"]!["id"]!.ToString();
                                var author = post["data"]!["author"]!.ToString();
                                var upvotes = post["data"]!["ups"]!.ToObject<int>();

                                _userPostCounts.AddOrUpdate(author, 1, (key, count) => count + 1);
                                _postUpvotes[postId] = upvotes!;
                            }

                            result.MostUpvotedPosts = _postUpvotes
                                .OrderByDescending(pair => pair.Value)
                                .Take(int.Parse(_configuration.GetSection("NumberOfResults").Value!));

                            result.MostActiveUsers = _userPostCounts
                                .OrderByDescending(pair => pair.Value)
                                .Take(int.Parse(_configuration.GetSection("NumberOfResults").Value!));

                            Console.WriteLine();
                            Console.WriteLine(JsonConvert.SerializeObject(result));
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("401 Unauthorized - Null Token");

                        break;
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
