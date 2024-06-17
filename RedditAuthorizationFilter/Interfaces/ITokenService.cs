using RedditAuthorizationFilter.Models;

namespace RedditAuthorizationFilter.Interfaces
{
    public interface ITokenService
    {
        Task<Token?> GetToken();
    }
}
