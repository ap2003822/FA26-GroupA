using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace Alliance_Webapp.Auth;
public class TokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _http;
    public TokenHandler(IHttpContextAccessor http) => _http = http;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _http.HttpContext;
        var token = context == null ? null : await context.GetTokenAsync("access_token");

        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}