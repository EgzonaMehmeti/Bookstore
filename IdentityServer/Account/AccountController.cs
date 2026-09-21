using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Account;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        string username,
        string password,
        string? returnUrl)
    {
        if (username != TestUser.Username ||
            password != TestUser.Password)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Error = "Invalid username or password.";

            return View();
        }

        var user = new IdentityServerUser(TestUser.SubjectId)
        {
            DisplayName = TestUser.Username
        };

        await HttpContext.SignInAsync(user);

        if (!string.IsNullOrWhiteSpace(returnUrl) &&
            Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return Redirect("/");
    }
}