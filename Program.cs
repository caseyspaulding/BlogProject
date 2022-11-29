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
using System;
using Microsoft.AspNetCore.Authentication.Twitter;
using TwitterOptions = Microsoft.AspNetCore.Authentication.Twitter.TwitterOptions;

var builder = WebApplication.CreateBuilder(args);

//Adding controller support
builder.Services.AddControllers();

//var connectionString = builder.Configuration.GetSection("pgSettings")["pgConnection"];

var connectionString = ConnectionHelper.GetConnectionString(builder.Configuration);

builder.Services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddDefaultUI()
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<ApplicationDbContext>();

var services = builder.Services;
var configuration = builder.Configuration;
builder.Services.AddAuthentication()
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = "d9cc4224-0405-4305-a1c9-778698deb25f";
        microsoftOptions.ClientSecret = "aspnet-BlogProject-48C3A131-B5D2-497E-B4BB-0783925C7F62";
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = "74931028186-4oaal9qvrsa22lnbj77aigj1duqd5467.apps.googleusercontent.com";
        googleOptions.ClientSecret = "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";
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = "447271047381191";
        facebookOptions.AppSecret = "6af4f8605b86589832b4205f04a5d34d";
    });


builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


//Builds the web application
var app = builder.Build();


var scope = app.Services.CreateScope();

await DataHelper.ManageDataAsync(scope.ServiceProvider);

// database update with the latest migrations

//Pull out my registered DataService
var dataService = app.Services
                        .CreateScope()
                       .ServiceProvider
                       .GetRequiredService<DataService>();

await dataService.ManageDateAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    //app.UseMigrationsEndPoint();
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


await DataHelper.ManageDataAsync(scope.ServiceProvider);


app.Run();