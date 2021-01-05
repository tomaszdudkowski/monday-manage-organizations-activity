using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Services
{
    public class SuperadminService
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<IdentityUser> userManager;

        public SuperadminService(IServiceProvider serviceProvider)
        {
            roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        }

        public async Task SuperadminRole()
        {
            bool isSuperadmin = await roleManager.RoleExistsAsync("Superadmin");

            if(!isSuperadmin)
            {
                // Utworzenie roli Superadmina
                var role = new IdentityRole();
                role.Name = "Superadmin";
                await roleManager.CreateAsync(role);

                // Utworzenie użytkownika Superadmina i dodanie do roli Superadmina
                var user = new IdentityUser();
                user.UserName = "superadmin@monday.pl";
                user.Email = "superadmin@monday.pl";
                user.EmailConfirmed = true;

                string SuperadminPassword = "Superadmin123!@#";

                IdentityResult identityResult = await userManager.CreateAsync(user, SuperadminPassword);

                if(identityResult.Succeeded)
                {
                    var result = await userManager.AddToRoleAsync(user, "Superadmin");
                }
                
            }
        }
    }
}
