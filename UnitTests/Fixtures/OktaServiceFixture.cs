using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedditAuthorizationFilter.Models;
using RedditAuthorizationFilter.Services;
using RedditAuthorizationFilter;
using System.Net;
using Moq;

namespace UnitTests.Fixtures
{
    public class OktaServiceFixture : AbstractFixture
    {
        private MockHttpMessageHandler MockMessageHandler(HttpStatusCode status, string? payload) =>
            new MockHttpMessageHandler((request, token) =>
            {
                HttpResponseMessage result;

                if (request!.RequestUri!.AbsoluteUri == Settings.OAUTHURL)
                {
                    var response = new HttpResponseMessage(status);
                    response.Content = new StringContent(payload!);
                    result = response;
                }
                else
                {
                    var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                    response.Content = new StringContent($"Bad set-up in OKTA service fixture; the target URL does not match '{Settings.OAUTHURL}'.");
                    result = response;
                }

                return Task.FromResult(result);
            });

        protected override void Arrange(params object[] parameters)
        {
            Settings.OAUTHURL = "https://mockoauthserver.com/token";
            Settings.OAUTHUSER = "testuser";
            Settings.OAUTHPASSWORD = "testpassword";
            Settings.GRANTTYPE = "client_credentials";
            Settings.SCOPE = "read";
            Settings.RETRYSLEEP = "30";
            Settings.RETRIES = "3";
            Settings.TOKENLIFETIME = "300";

            var mockLogger = new Mock<ILogger<TokenService>>();
            var token = new Token
            {
                AccessToken = "mocked_token",
                ExpiresIn = Settings.TOKENLIFETIME,
                Scope = Settings.SCOPE,
                TokenType = "bearer"
            };

            var mockClientOK = new HttpClient(MockMessageHandler(HttpStatusCode.OK, JsonConvert.SerializeObject(token)));
            var mockClientUnauthorized = new HttpClient(MockMessageHandler(HttpStatusCode.Unauthorized, "Unauthorized: Invalid authentication credentials."));
            var mockClientForbidden = new HttpClient(MockMessageHandler(HttpStatusCode.Forbidden, "Forbidden: Insufficient permissions to access this resource."));
            var mockClientProxyRequired = new HttpClient(MockMessageHandler((HttpStatusCode)407, "Proxy Authentication Required: Unable to authenticate with proxy server."));

            ContextServiceOK = new TokenService(mockLogger.Object, mockClientOK);
            ContextServiceUnauthorized = new TokenService(mockLogger.Object, mockClientUnauthorized);
            ContextServiceForbidden = new TokenService(mockLogger.Object, mockClientForbidden);
            ContextServiceProxyRequired = new TokenService(mockLogger.Object, mockClientProxyRequired);
        }

        public OktaServiceFixture() => Arrange();

        public TokenService ContextServiceOK { get; set; }
        public TokenService ContextServiceUnauthorized { get; set; }
        public TokenService ContextServiceForbidden { get; set; }
        public TokenService ContextServiceProxyRequired { get; set; }
    }
}
