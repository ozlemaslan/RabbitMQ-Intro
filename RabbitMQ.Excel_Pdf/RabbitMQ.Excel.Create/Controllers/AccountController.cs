using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Excel.Create.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Excel.Create.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email,string password)
        {
            var userEmail =await _userManager.FindByEmailAsync(email);

            if (userEmail!=null)
            {
                var pswUser = _signInManager.PasswordSignInAsync(userEmail, password, true, false);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
