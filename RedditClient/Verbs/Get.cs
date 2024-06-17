﻿using RedditClient.Verbs.Interfaces;

namespace RedditClient.Verbs
{
    public class Get : AbstractVerb
    {
        public Get(ref HttpClient client) : base(ref client)
        {

        }

        override public async Task<HttpResponseMessage> Invoke(HttpRequestMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                message.Method = HttpMethod.Get;

                return await base.Invoke(message, cancellationToken);
            }
            catch
            {
                throw;
            }
        }
    }
}
