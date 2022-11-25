using BlogProject.Services;
using BlogProject.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using TheBlogProject.Services;
using BlogProject.ViewModels;
using BlogProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using BlogProject.Helpers;

var builder = WebApplication.CreateBuilder(args);

//Adding controller support
builder.Services.AddControllers();

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//var connectionString = builder.Configuration.GetSection("pgSettings")["pgConnection"];



builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var connectionString = ConnectionHelper.GetConnectionString(builder.Configuration);
builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddDefaultUI()
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseNpgsql(connectionString));

//Register my custom DataService class
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<BlogSearchService>();

//Register a preconfigured instance of the MailSettings class
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IBlogEmailSender, EmailService>();



//Register our Image Service
builder.Services.AddScoped<IImageService, BasicImageService>();

//Register the Slug Service
builder.Services.AddScoped<ISlugService, BasicSlugService>();
//___________________________________________________________________
//END OF CUSTOM SERVICES 

//Builds the web application
var app = builder.Build();

var scope = app.Services.CreateScope();

// database update with the latest migrations
await DataHelper.ManageDataAsync(scope.ServiceProvider);



//Pull out my registered DataService
var dataService = app.Services
                        .CreateScope()
                       .ServiceProvider
                       .GetRequiredService<DataService>();

await dataService.ManageDateAsync();


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

app.UseStatusCodePagesWithReExecute("/Home/HandleError/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=BlogPostIndex}/{id?}");
app.MapRazorPages();

app.Run();
