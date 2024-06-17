using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedditAuthorizationFilter.Interfaces;
using RedditAuthorizationFilter.Services;
using RedditAuthorizationFilter;

namespace UnitTests.Assertions.Using.Authentication
{
    public class TestableAuthenticationFilter : AuthenticationFilter<TokenService>
    {
        public TestableAuthenticationFilter(TokenService tokenService, IDistributedCacheHelper memoryCache, ILogger<AuthenticationFilter<TokenService>> logger)
            : base(tokenService, memoryCache, logger)
        {
            Counter = 0;
        }

        public int Counter { get; set; }

        override protected bool IsAuthenticationError(ObjectResult? resultObj)
        {
            var result = base.IsAuthenticationError(resultObj);

            if (result) Counter++;

            return result;
        }
    }
}
