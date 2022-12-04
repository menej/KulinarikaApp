using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace KulinarikaApp.Authorization.RecipeAuthorization;

public class RecipeOperations
{
    public static OperationAuthorizationRequirement Create = new() { Name = Constants.CreateOperationName };
    public static OperationAuthorizationRequirement Read = new() { Name = Constants.ReadOperationName };
    public static OperationAuthorizationRequirement Update = new() { Name = Constants.UpdateOperationName };
    public static OperationAuthorizationRequirement Delete = new() { Name = Constants.DeleteOperationName };
}

public class Constants
{
    public static readonly string CreateOperationName = "Create";
    public static readonly string ReadOperationName = "Read";
    public static readonly string UpdateOperationName = "Update";
    public static readonly string DeleteOperationName = "Delete";

    public static readonly string ModeratorRole = "Moderator";
}