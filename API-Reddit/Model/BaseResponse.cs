using API_Reddit.Model.Response;
using System.Collections.Concurrent;

namespace API_Reddit.Model
{
    public class BaseResponse
    {
        public BaseResponse() 
        {
            RateLimiting = new RateLimiting();
            MostActiveUsers = new ConcurrentDictionary<KeyValuePair<string, int>, string>();
            MostUpvotedPosts = new ConcurrentDictionary<KeyValuePair<string, int>, string>();
        }

        public BaseResponse(BaseResponse responseObject) 
        {
            RateLimiting = responseObject.RateLimiting;
            MostActiveUsers = responseObject.MostActiveUsers;
            MostUpvotedPosts = responseObject.MostUpvotedPosts;
        }

        public object MostActiveUsers { get; set; }

        public object MostUpvotedPosts { get; set; }

        public RateLimiting RateLimiting { get; set; }
    }
}
