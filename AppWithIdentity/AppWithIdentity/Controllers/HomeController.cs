using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppWithIdentity.Models;
using AppWithIdentity.Models.Home;
using AppWithIdentity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace AppWithIdentity.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<AppUser> userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Name);
                if (user == null)
                {
                    user = new AppUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.Name
                    };

                    var result = await userManager.CreateAsync(user, model.Password);
                }

                return View("Success");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                    if (await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        var identity = new ClaimsIdentity("cookies");
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                        await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError("", "Invalide username and password");
            }

            return View();
        }

    }
}
