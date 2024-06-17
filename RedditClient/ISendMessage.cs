using RedditClient.Verbs.Interfaces;

namespace RedditClient
{
    public interface ISendMessage
    {
        Task<HttpResponseMessage> SendRequestAsync<TVerb>
            (string uri, object token = null!, object payload = null!, CancellationToken cancellationToken = new CancellationToken())
                where TVerb : AbstractVerb;
    }
}
