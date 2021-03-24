using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuristickaAgencija.Options;
using TuristickaAgencija.Strings;

namespace TuristickaAgencija.Extensions
{
    public static class IdentityDataInitializer
    {
        public static IApplicationBuilder SeedIdentityData (this IApplicationBuilder app,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            AdminOptions options = configuration.GetSection("DefaultAdmin").Get<AdminOptions>();

            SeedRoles(roleManager);
            SeedUsers(userManager,options);

            return app;
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(RoleNames.Admin).Result)
            {
                IdentityRole identityRole = new IdentityRole { Name = RoleNames.Admin };
                roleManager.CreateAsync(identityRole).Wait();
            }
            if (!roleManager.RoleExistsAsync(RoleNames.Korisnik).Result)
            {
                IdentityRole identityRole = new IdentityRole { Name = RoleNames.Korisnik };
                roleManager.CreateAsync(identityRole).Wait();
            }

        }
        private static void SeedUsers (UserManager<IdentityUser> userManager,AdminOptions adminOptions)
        {
            if(userManager.FindByNameAsync(adminOptions.Email).Result == null)
            {
                IdentityUser identityUser = new IdentityUser { Email = adminOptions.Email, UserName = adminOptions.Email };

                IdentityResult result = userManager.CreateAsync(identityUser, adminOptions.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(identityUser, RoleNames.Admin).Wait();
                }
            }
        }
    }
    
}
