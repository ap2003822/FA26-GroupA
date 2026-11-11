using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using Alliance_Webapp.Models;
using Alliance_Webapp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alliance_Webapp.Controllers;

[AllowAnonymous]
public class UserController : Controller
{
    private readonly IHttpClientFactory _factory;
    public UserController(IHttpClientFactory factory) => _factory = factory;

    // Matches the API's LoginResponseModel
    private record ApiLoginResponse(bool Status, string? Message, string? AuthToken);

    [HttpGet]
    public IActionResult Login(string? returnUrl = null) =>
        View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var client = _factory.CreateClient("Api");
        HttpResponseMessage response;
        try
        {
            response = await client.PostAsJsonAsync("api/auth/login",
                new { Email = model.Email, Password = model.Password });
        }
        catch (HttpRequestException)
        {
            model.ErrorMessage = "Cannot reach the server. Is the API running?";
            return View(model);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            model.ErrorMessage = "Invalid email or password.";
            return View(model);
        }
        response.EnsureSuccessStatusCode();

        var login = await response.Content.ReadFromJsonAsync<ApiLoginResponse>();
        if (login == null || !login.Status || string.IsNullOrEmpty(login.AuthToken))
        {
            model.ErrorMessage = "Invalid email or password.";
            return View(model);
        }

        // Read the user's details from the token
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(login.AuthToken);
        var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
        var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
        {
            model.ErrorMessage = "The login token was missing user details.";
            return View(model);
        }

        var claims = new List<Claim>
{
    new(ClaimTypes.Name, email),
    new(ClaimTypes.Role, role)
};
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var properties = new AuthenticationProperties
        {
            ExpiresUtc = new DateTimeOffset(jwt.ValidTo)
        };
        properties.StoreTokens(new[]
        {
    new AuthenticationToken { Name = "access_token", Value = login.AuthToken }
});

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            properties);

        return Url.IsLocalUrl(model.ReturnUrl)
            ? LocalRedirect(model.ReturnUrl!)
            : RedirectToAction("Index", "Item");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();   // deletes the cookie
        return RedirectToAction("Login");
    }
}