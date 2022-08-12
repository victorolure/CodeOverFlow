using CodeOverFlow.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeOverFlow.Models
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            ApplicationDbContext _context = new ApplicationDbContext
                (serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            RoleManager<IdentityRole> _roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            UserManager<ApplicationUser> _userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            List<string> roles = new List<string>()
            {
                "Owner", "Manager", "User"
            };



            foreach (string role in roles)
            {
                if (!_context.Roles.Any(r => r.Name == role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    await _context.SaveChangesAsync();
                }

            }


            if (!_context.Users.Any(u => u.Email == "dane@gmail.com"))
            {
                ApplicationUser seededUser = new ApplicationUser
                {
                    Email = "dane@gmail.com",
                    NormalizedEmail = "DANE@GMAIL.COM",
                    UserName = "dane@gmail.com",
                    NormalizedUserName = "DANE@GMAIL.COM",
                    EmailConfirmed = true

                };


                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(seededUser, "Cyberthron1!");
                seededUser.PasswordHash = hashed;

                await _userManager.CreateAsync(seededUser);
                await _userManager.AddToRoleAsync(seededUser, "Owner");

                await _context.SaveChangesAsync();

            }

            if (!_context.Users.Any(u => u.Email == "jaesung@gmail.com"))
            {

                ApplicationUser secondSeededUser = new ApplicationUser
                {
                    Email = "jaesung@gmail.com",
                    NormalizedEmail = "JAESUNG@GMAIL.COM",
                    UserName = "jaesung@gmail.com",
                    NormalizedUserName = "JAESUNG@GMAIL.COM",
                    EmailConfirmed = true

                };


                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(secondSeededUser, "Cyberthron1!");
                secondSeededUser.PasswordHash = hashed;

                await _userManager.CreateAsync(secondSeededUser);
                await _userManager.AddToRoleAsync(secondSeededUser, "Manager");

                await _context.SaveChangesAsync();

            }
            if (!_context.Users.Any(u => u.Email == "max@gmail.com"))
            {
                ApplicationUser thirdSeededUser = new ApplicationUser
                {
                    Email = "max@gmail.com",
                    NormalizedEmail = "MAX@GMAIL.COM",
                    UserName = "max@gmail.com",
                    NormalizedUserName = "MAX@GMAIL.COM",
                    EmailConfirmed = true

                };

                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(thirdSeededUser, "Cyberthron1!");
                thirdSeededUser.PasswordHash = hashed;

                await _userManager.CreateAsync(thirdSeededUser);
                await _userManager.AddToRoleAsync(thirdSeededUser, "User");
                await _context.SaveChangesAsync();
            }

        }
    }
}
