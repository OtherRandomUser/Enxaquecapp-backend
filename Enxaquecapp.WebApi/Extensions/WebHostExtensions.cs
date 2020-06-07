using System;
using System.Collections.Generic;
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
                        user = new User("João", "address@example.com", "123", DateTime.Parse("2010-12-20"), Gender.Male);
                        context.Users.Add(user);
                        context.SaveChanges();

                        var reliefs = new List<Relief>
                        {
                            new Relief(user, "Relief 1", null),
                            new Relief(user, "Relief 2", null)
                        };

                        var causes = new List<Cause>
                        {
                            new Cause(user, "Cause 1", null),
                            new Cause(user, "Cause 2", null)
                        };

                        var locals = new List<Local>
                        {
                            new Local(user, "Local 1", null),
                            new Local(user, "Local 2", null)
                        };

                        context.Reliefs.AddRange(reliefs);
                        context.Causes.AddRange(causes);
                        context.Locals.AddRange(locals);
                        context.SaveChanges();
                    }
                }
            }

            return host;
        }
    }
}