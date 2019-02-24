using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pm.Common.Exceptions;
using Pm.Core;
using Pm.Data.Entity;
using Pm.Models;
using ProjectPlaguemangler.Filters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Serilog;

namespace ProjectPlaguemangler.Controllers
{
    [Route("cms")]
    [ServiceFilter(typeof(ExceptionHandler))]
    public class CmsLoginController : BaseController
    {
        private readonly UserService userService;
        
        public CmsLoginController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("")]
        public async Task<IActionResult> PostLogin(LoginModel model)
        {
            User user;

            try
            {
                user = await userService.GetVerifiedUser(model.Username, model.Password);
            }
            catch (PmNotFoundException)
            {
                await Task.Delay(2000); // Security measure
                AddBox("Invalid credentials", BoxType.Error);
                Log.Warning($"Failed login attempt {model.Username}:{model.Password}");
                return RedirectToAction("Login");
            }

            // use IdentityExtensions
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserIdentifier", user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "cookie");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("CookieScheme", claimsPrincipal);

            return RedirectToAction("List", "CmsPost");
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
