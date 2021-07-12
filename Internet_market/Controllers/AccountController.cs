using Internet_market.Models;
using Internet_market.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _appContext;

        public AccountController(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                User user = _appContext.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == passwordHash);

                if (user != null)
                {
                    await Authenticate(user);

                    
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login or password");
                
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sha256 = new SHA256Managed();
                var passwordHash = Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                User user = await _appContext.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == passwordHash);

                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email,
                        Password = passwordHash,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    };

                    _appContext.Users.Add(user);
                    await _appContext.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError("", "Такой e-mail уже зарегистрирован !");
            }
            return View(model);
        }

      

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "" + user.Id + "|" + user.FirstName +" "+ user.LastName),
            };

            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity
                (claims, 
                "ApplicationCookie", 
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
            ClaimsPrincipal principal = new ClaimsPrincipal(id);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            
        }
    }
}