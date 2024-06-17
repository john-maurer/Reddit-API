using Newtonsoft.Json;
using RedditClient.Verbs.Interfaces;
using System.Net.Http.Headers;

namespace RedditClient
{
    public class RedItClient : ISendMessage
    {
        protected HttpClient _httpClient;

        public RedItClient(HttpClient client) => _httpClient = client;

        virtual public async Task<HttpResponseMessage> SendRequestAsync<TVerb>(string uri, object token = null!, object payload = null!, CancellationToken cancellationToken = new CancellationToken()) where TVerb : AbstractVerb
        {
            HttpResponseMessage response;

            try
            {

                using (var message = new HttpRequestMessage())
                {
                    message.RequestUri = new Uri(uri);

                    if (payload != null)
                    {
                        if (payload.GetType() == typeof(FormUrlEncodedContent))
                        {
                            message.Content = (FormUrlEncodedContent)payload;
                        }
                        else
                            message.Content = new StringContent(JsonConvert.SerializeObject(payload));
                    }

                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token as string);
                    message.Headers.UserAgent.ParseAdd("RedEd-Listener/1.0");

                    response =
                        await ((TVerb)typeof(TVerb).GetConstructor(new Type[] { typeof(HttpClient).MakeByRefType() })!
                            .Invoke(new object[] { _httpClient }))
                                .Invoke(message, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
    }
}
