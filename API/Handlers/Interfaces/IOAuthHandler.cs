using API.DTOs;

namespace API.Handlers.Interfaces;

public interface IOAuthHandler
{
    /// <summary>
    /// Authorizes a new or returning user via osu!
    /// </summary>
    /// <param name="osuAuthToken">The token provided by
    /// the osu! oAuth redirect.
    /// 
    /// <a href="https://osu.ppy.sh/docs/index.html#authorization-code-grant">
    /// osu! Authorization Code Grant documentation</a>
    /// </param>
    Task<OAuthResponseDTO?> AuthorizeAsync(string osuAuthToken);

    /// <summary>
    /// Authorize a user's OAuth client. Returns a response that allows
    /// clients to call the API.
    /// </summary>
    /// <param name="userId">The id of the user for which the client belongs</param>
    /// <param name="clientId">The id of the OAuth client</param>
    /// <param name="clientSecret">The client secret</param>
    /// <returns></returns>
    Task<OAuthResponseDTO?> AuthorizeAsync(int userId, int clientId, string clientSecret);
    /// <summary>
    /// Refreshes the accessToken using the provided refreshToken. Both tokens must be
    /// previously encoded JWTs. 
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="refreshToken"></param>
    /// <returns></returns>
    Task<OAuthResponseDTO?> RefreshAsync(string accessToken, string refreshToken);

    /// <summary>
    /// Creates a new OAuth client for a user that can be used for API access.
    /// </summary>
    /// <param name="userId">The id of the user who owns this client</param>
    /// <param name="scopes">The scopes this client has access to</param>
    /// <returns></returns>
    Task<OAuthClientDTO?> CreateClientAsync(int userId, params string[] scopes);
}