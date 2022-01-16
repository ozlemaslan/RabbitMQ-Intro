using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Excel.Create.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Excel.Create
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope= host.Services.CreateScope())
            {
                var appDbContext= scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                //tüm migrationlarý package manager console dan update-database demeden oluþturur.Uyg ayaða kalkýnca 
                appDbContext.Database.Migrate(); 
                //user tablosunda hiç kayýt yoksa kayýt atacak
                if (!appDbContext.Users.Any())
                {
                    userManager.CreateAsync(new IdentityUser
                    {
                        Id = "1",
                        Email = "admin@admin.com",
                        UserName = "admin",
                    },"12345");

                }
            }

            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
