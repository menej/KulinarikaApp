using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KulinarikaApp.Authorization;
using KulinarikaApp.Authorization.RecipeAuthorization;
using KulinarikaApp.Data;
using KulinarikaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Getting Connection String from Azure Key Vault secrets
/*SecretClientOptions options = new SecretClientOptions()
{
    Retry =
    {
        Delay = TimeSpan.FromSeconds(2),
        MaxDelay = TimeSpan.FromSeconds(16),
        MaxRetries = 5,
        Mode = RetryMode.Exponential
    }
};

var client = new SecretClient(new Uri("https://klinarika-secret-vault.vault.azure.net/"), new DefaultAzureCredential(),
    options);

KeyVaultSecret secret = client.GetSecret("ConnectionStrings--AzureContext");
var secretValue = secret.Value;
*/

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
/*
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(secretValue));
*/
// This should only be used locale -> connection string should be in secrets
// No idea if this or what is standard practice here, but I guess it works
var connectionString = builder.Configuration.GetConnectionString("AzureContext");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = true) // for required accounts
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
});

/* Enabling global authorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
*/


builder.Services.AddScoped<IAuthorizationHandler, RecipeCreatorAuthorization>();
builder.Services.AddSingleton<IAuthorizationHandler, RecipeModeratorAuthorizationHandler>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run our SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// The use of swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/api/v1/swagger.json", "My API V1");
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();