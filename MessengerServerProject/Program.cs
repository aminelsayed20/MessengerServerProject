using MessengerServerProject.Components;
using MessengerServerProject.Components.Account;
using MessengerServerProject.Data;
using MessengerServerProject.Hubs;
using MessengerServerProject.Repository;
using MessengerServerProject.Repository.Interfaces;
using MessengerServerProject.Service;
using MessengerServerProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddSingleton<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();


builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();



// Dependency injections
builder.Services.AddTransient<UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Register IUserRepository with UserRepository
builder.Services.AddScoped<IConnectionUserRepository, ConnectionUserRepository>(); // Register IConnectionUserRepository with ConnectionUserRepository
builder.Services.AddScoped<IMessageRepository, MessageRepository>(); // Register IMessageRepository with MessageRepository
builder.Services.AddTransient<UserChatServices>();


builder.Services.AddTransient<SignalRService>();



//builder.Services.AddHttpContextAccessor(); //////



builder.Services.AddBlazorBootstrap();

var app = builder.Build();
app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//app.UseAuthorization();////



// Add additional endpoints required by the Identity /Account Razor components.
//app.MapAdditionalIdentityEndpoints();/////

//app.UseResponseCompression();////


app.UseResponseCompression();///


app.MapHub<MessengerHub>("/messengerhub");




app.Run();
