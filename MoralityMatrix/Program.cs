using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoralityMatrix.Data;
using MoralityMatrix.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, 
        providerOptions => providerOptions.EnableRetryOnFailure()
    )
);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Token provider options
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    // Set the lifetime for Password Recovery and Email Confirmation Tokens to 5 minutes
    options.TokenLifespan = TimeSpan.FromMinutes(15);
});

// Identity Options
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddErrorDescriber<CustomIdentityErrorDescriber>()
.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();

// Add Email Services
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Add Question manager
builder.Services.AddScoped<QuestionManager>();

// Configure Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".MoralityMatrix.SessionVariables";
});

// Add Google OAuth Services
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
 {
     googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
     googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
     googleOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
 });

// Configure Identity services
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        // Setting RequireUniqueEmail to False causes an internal server error because a SQL query returns more than one element
        options.User.RequireUniqueEmail = true;
});

// Configure Cookie services
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    options.Cookie.Name = ".MoralityMatrix.Identity";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Uncomment when instantiating the database but remove in production
// Required for role and admin account seeding
/* 
using(var scope = app.Services.CreateScope())
{
    // Seed Roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    // Seed Users
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string username = "admin";
    string email = "admin@admin.com";
    string password = "#Admin2024";

    if(await userManager.FindByNameAsync(username) == null)
    {
        var user = new ApplicationUser();
        user.UserName = username;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}
*/

app.Run();
