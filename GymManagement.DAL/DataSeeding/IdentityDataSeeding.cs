using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager
            , UserManager<ApplicationUser> userManager
            , ILogger logger
            , CancellationToken ct = default)
        {
            try
            {
                //Check if users or roles exists or Not 
                bool HasUsers = await userManager.Users.AnyAsync(ct);
                bool HasRoles = await roleManager.Roles.AnyAsync(ct);

                if (HasUsers && HasRoles) return;

                var roles = new List<IdentityRole>()
            {
                new IdentityRole("SuperAdmin"),
                new IdentityRole("Admin")

            };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Add Role {role.Name}");
                        }
                    }
                }

                //User
                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Mona",
                        LastName = "Allam",
                        Email = "MonaAllam@gmail.com",
                        UserName = "MonaAllam",
                        PhoneNumber = "0128733645"
                    };
                    await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Myar",
                        LastName = "Elsherqawy",
                        Email = "Myar123@gmail.com",
                        UserName = "MyarElsherqawy",
                        PhoneNumber = "0112109732"
                    };
                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.CreateAsync(Admin, "Admin");

                    logger.LogInformation("Identity Seeded Successfully.");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return;
            }       
        }
    }
}    
