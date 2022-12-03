using KulinarikaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace KulinarikaApp.Authorization.RecipeAuthorization;

public class RecipeCreatorAuthorization : AuthorizationHandler<OperationAuthorizationRequirement, Recipe>
{
    private UserManager<ApplicationUser> _userManager;

    public RecipeCreatorAuthorization(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        Recipe recipe)
    {
        if (context.User == null || recipe == null) // no user logged in at all OR no recipe given
        {
            return Task.CompletedTask;
        }

        if (requirement.Name != Constants.CreateOperationName &&
            requirement.Name != Constants.ReadOperationName &&
            requirement.Name != Constants.UpdateOperationName &&
            requirement.Name != Constants.DeleteOperationName)
        {
            return Task.CompletedTask;
        }

        if (recipe.UserId == _userManager.GetUserId(context.User)) // We have the user
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask; // the user is trying to do something else (this handler is not meant for this)
    }
}