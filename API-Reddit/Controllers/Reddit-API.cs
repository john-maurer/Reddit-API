using RedditAuthorizationFilter;
using RedditAuthorizationFilter.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RedditClient;
using RedditClient.Verbs;
using API_Reddit.Model;
using API_Reddit.Model.Response;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using RedditAuthorizationFilter.Models;

namespace API_Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthenticationFilter<ITokenService>))]
    public class Reddit_API : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Reddit_API> _logger;
        private readonly IDistributedCacheHelper _memoryCache;
        private readonly ISendMessage _redditClient;
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

        public Reddit_API(ILogger<Reddit_API> logger, IConfiguration configuration, IDistributedCacheHelper memoryCache, ISendMessage client)
        {
            _logger = logger;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _redditClient = client;
        }

        [HttpGet("PoolSubReddit")]
        public async Task<IActionResult> PoolSubReddit(string subReddit, int limit = 50)
        {
            try
            {
                var token = JsonConvert.DeserializeObject<Token>((await _memoryCache.Get("OKTA-TOKEN"))!);

                if (token != null)
                {
                    var response = await _redditClient.SendRequestAsync<Get>($"https://oauth.reddit.com/r/{subReddit}/new.json?limit={limit.ToString()}", token.AccessToken);
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
                        _postUpvotes[postId] = upvotes;
                    }

                    result.MostUpvotedPosts = _postUpvotes
                        .OrderByDescending(pair => pair.Value)
                        .Take(int.Parse(_configuration.GetSection("NumberOfResults").Value!));

                    result.MostActiveUsers = _userPostCounts
                        .OrderByDescending(pair => pair.Value)
                        .Take(int.Parse(_configuration.GetSection("NumberOfResults").Value!));

                    return Ok(result);
                }
                else
                    return Unauthorized("Null Token");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
