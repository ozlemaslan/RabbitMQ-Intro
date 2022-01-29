using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Excel.Create.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Excel.Create.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public ProductController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductExcel()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            string fileName = Guid.NewGuid().ToString();

            UserFile userFile = new()
            {
                UserId = user.Id,
                FileStatus = FileStatus.Creating,
                FileName = fileName

            };

            await _context.UserFiles.AddAsync(userFile);
            await _context.SaveChangesAsync();

            return RedirectToAction("Files");
        }

        [HttpPost]
        public async Task<IActionResult> Files()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            return View(_context.UserFiles.Where(a=>a.UserId==user.Id).ToListAsync());
        }
    }
}
