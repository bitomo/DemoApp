using Microsoft.EntityFrameworkCore;
using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Ecom.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
/**
 * [TT]: Required for Identity Razor pages to work.
 */
builder.Services.AddRazorPages();

/**
 * [TT]: Register custom ApplicationDbContext class as a service for DbContext.
 * rel: ApplicationDbContext.cs
 */
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    /**
     * [TT]: Builder has access to appsettings.json, so we can get the connection string with the following method.
     * rel: appsettings.json
     */
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
/**
 * [TT]: Required if not using AddDefaultIdentity.
 */
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(configure =>
{
    configure.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    configure.LoginPath = $"/Identity/Account/Login";
    configure.LogoutPath = $"/Identity/Account/Logout";
});

/**
 * [TT]: Whenever an unhandled exception states "Unable to resolve service for type...", first check if the service is even registered with the dependency injection container.
 * TheMoreYouKnow: 
    * Transient - returns new instance every time it is requested
    * Scoped - reuse the instance for a single http request
    * Singleton - same instance for the entire lifespan of the application
 */
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

/**
 * [TT]: If encountering the following error
     * Some services are not able to be constructed 
     * (Error while validating the service descriptor 'ServiceType: Ecom.DataAccess.Repository.IUnitOfWork Lifetime: 
     * Scoped ImplementationType: Ecom.DataAccess.Repository.UnitOfWork': 
     * Unable to resolve service for type 'Ecom.DataAccess.Data.ApplicationDbContext' while attempting to activate 'Ecom.DataAccess.Repository.UnitOfWork'.)'
 * Delete Areas.Identity.Data.ApplicationDbContext.cs
 */
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

/**
 * [TT]: Required for Identity Razor pages to work.
 */
app.MapRazorPages();

app.Run();
