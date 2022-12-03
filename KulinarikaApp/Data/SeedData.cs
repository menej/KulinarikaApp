using KulinarikaApp.Authorization;
using KulinarikaApp.Authorization.RecipeAuthorization;
using KulinarikaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KulinarikaApp.Data;

public class SeedData
{
    public static async Task Initialize(
        IServiceProvider serviceProvider,
        string password = "Aa1234!")
    {
        // Because we can not use dependency injection we create our own context using the service provider.
        await using (var context = new ApplicationDbContext(
                         serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // Add normal users
            await EnsureUser(serviceProvider, "demo1@test.com", password, "Andrej", "Novak");
            await EnsureUser(serviceProvider, "demo2@test.com", password, "Berta", "Bizjak");
        
            // Add moderators
            var moderatorUid = await EnsureUser(serviceProvider, "moderator@test.com", password, "Moderator", "Moderator");
            var identityResult = await EnsureRole(serviceProvider, moderatorUid, Constants.ModeratorRole);
        }
    }

    private static async Task<string> EnsureUser(
        IServiceProvider serviceProvider,
        string userName,
        string initialPassword,
        string firstName,
        string lastName)
    {
        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByNameAsync(userName);

        // If the user does not exists we create a new one
        if (user == null)
        {
            user = new ApplicationUser
            {
                FristName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = userName,
                EmailConfirmed = true
            };

            // Please make sure the password matches the password policy of the application
            var result = await userManager.CreateAsync(user, initialPassword);
            if (result == null)
                throw new Exception("Something wron.");
        }


        if (user == null)
            throw new Exception("User did not get created. Might be password policy problem?");

        return user.Id;
    }

    // Ensure role and give it to a specific user, if the role does not exists in the database, create it
    private static async Task<IdentityResult> EnsureRole(
        IServiceProvider serviceProvider,
        string uid,
        string role)
    {
        var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

        if (roleManager == null)
            throw new Exception("No RoleManager service found. Is it registered as a service?");

        IdentityResult identityResult;

        // if this role does not exists create it
        if (await roleManager.RoleExistsAsync(role) == false)
        {
            identityResult = await roleManager.CreateAsync(new IdentityRole(role));
        }

        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByIdAsync(uid);

        if (user == null)
            throw new Exception("User does not exists.");

        identityResult = await userManager.AddToRoleAsync(user, role);
        return identityResult;
    }
}