using BlazorAuthenticationWeb.Data;
using Microsoft.AspNetCore.Identity;

namespace BlazorAuthenticationWeb.Handlers;

public class RoleHandler
{
    public async Task CreateUserRolesAsync(string user, string role, IServiceProvider serviceProvider)
    {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();

            var userRoleCheck = await roleManager.RoleExistsAsync(role);
            if (!userRoleCheck)
            {
                var roleObj = new IdentityRole(role);
                await roleManager.CreateAsync(roleObj);
            }

            ApplicationUser identityUser = await userManager.FindByEmailAsync(user);

            await userManager.AddToRoleAsync(identityUser, role);

            // Refresh SignIn for Claims
            await signInManager.RefreshSignInAsync(identityUser);
    }
}
