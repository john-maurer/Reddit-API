﻿namespace RedditClient.Verbs.Interfaces
{
    public interface IVerb
    {
        Task<HttpResponseMessage> Invoke(HttpRequestMessage message, CancellationToken cancellationToken = new CancellationToken());
    }
}
