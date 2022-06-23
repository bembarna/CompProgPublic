using Microsoft.AspNetCore.Identity;
using CompProgEdu.Core.Features.Users;
using CompProgEdu.Core.Security;
using System.Linq;
using System.Threading.Tasks;

namespace CompProgEdu.Core.Data
{
    public static class DataSeeder
    {
        public static void SeedData(this DataContext dataContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            SeedUsersAndRoles(dataContext, userManager, roleManager).Wait();

            dataContext.SaveChanges();
        }

        private static async Task SeedUsersAndRoles(DataContext dataContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            foreach (var role in Roles.List)
            {
                var existingRole = await roleManager.FindByNameAsync(role);
                if (existingRole == null)
                {
                    await roleManager.CreateAsync(new Role { Name = role });
                }
            }

            var admin = new User
            {
                Email = "admin@compprogedu.com",
                UserName = "admin@compprogedu.com",
                FirstName = "Global",
                LastName = "Admin",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(admin.Email);
            if (user == null)
            {
                user = admin;
                await userManager.CreateAsync(user, "BigBingee1!");
                await userManager.AddToRoleAsync(user, Roles.GlobalAdmin);
            }
            else
            {
                user.UserClaims = dataContext.Set<UserClaim>().Where(x => x.UserId == user.Id).ToList();
            }
        }
    }
}
