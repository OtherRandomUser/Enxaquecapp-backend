using System;
using System.Linq;
using Enxaquecapp.Data;
using Enxaquecapp.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Enxaquecapp.WebApi.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost Seed(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();

                    var user = context.Users.FirstOrDefault();

                    if (user == null)
                    {
                        user = new User("João", "address@example.com", "123", DateTime.Parse("2010-12-20"), Sex.Male);
                        context.Users.Add(user);
                        context.SaveChanges();
                    }
                }
            }

            return host;
        }
    }
}