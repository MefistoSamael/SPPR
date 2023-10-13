using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using WEB_153501_BYCHKO.IdentityServer.Data;
using WEB_153501_BYCHKO.IdentityServer.Models;

namespace WEB_153501_BYCHKO.IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(WebApplication app)
        {
            using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Проверьте, существует ли роль "admin", и создайте ее, если она не существует
                var adminRoleExists = roleMgr.RoleExistsAsync("admin").Result;
                if (!adminRoleExists)
                {
                    var adminRole = new IdentityRole("admin");
                    roleMgr.CreateAsync(adminRole);
                }


                var admin = userMgr.FindByNameAsync("Admin").Result;
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(admin, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(admin, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Big Admin"),
                            new Claim(JwtClaimTypes.GivenName, "Admin"),
                            new Claim(JwtClaimTypes.FamilyName, "Big"),
                            new Claim(JwtClaimTypes.WebSite, "https://admin.com"),
                        }).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    else
                    {
                        // Назначьте пользователю роль "admin"
                        userMgr.AddToRoleAsync(admin, "admin");

                    }
                    Log.Debug("admin created");
                }
                else
                {
                    Log.Debug("admin already exists");
                }


                // Проверьте, существует ли роль "user", и создайте ее, если она не существует
                var userRoleExists = roleMgr.RoleExistsAsync("user").Result;
                if (!userRoleExists)
                {
                    var userRole = new IdentityRole("user");
                    roleMgr.CreateAsync(userRole);
                }

                var user = userMgr.FindByNameAsync("User").Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "User",
                        Email = "user@email.com",
                        EmailConfirmed = true
                    };
                    var result = userMgr.CreateAsync(user, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(user, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "User Simple"),
                            new Claim(JwtClaimTypes.GivenName, "User"),
                            new Claim(JwtClaimTypes.FamilyName, "Simple"),
                            new Claim(JwtClaimTypes.WebSite, "http://user.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    else
                    {
                        userMgr.AddToRoleAsync(user, "user");
                    }

                    Log.Debug("user created");
                }
                else
                {
                    Log.Debug("user already exists");
                }
            }
        }
    }
}