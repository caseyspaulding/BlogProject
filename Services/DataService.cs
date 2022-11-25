using BlogProject.Data;
using BlogProject.Enums;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject.Services;

public class DataService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<BlogUser> _userManager;

    public DataService(ApplicationDbContext dbContext,
                        RoleManager<IdentityRole> roleManager,
                        UserManager<BlogUser> userManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task ManageDateAsync()
    {
        //Task: Create the DB from the Migrations
        await _dbContext.Database.MigrateAsync();

        // Task 1: Seed a few Roles into the system
        await SeedRolesAsync();

        // Task 2: Seed a few users into the system
        await SeedUsersAsync();
    }

    private async Task SeedRolesAsync()
    {
        //If there are already Roles in the system, do nothing. 
        if (_dbContext.Roles.Any())
        { //communication with the ROLES table.
            return;

        }

        foreach (var role in Enum.GetNames(typeof(BlogRole)))
        {
            // I need to use the Role Manager to create roles
            await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private async Task SeedUsersAsync()
    {
        //If there are already Users in the system, do nothing.
        if (_dbContext.Users.Any()) // Checking Users Table for users
        {
            return;
        }

        //Step 1: Creates a new instance of BlogUser
        var adminUser = new BlogUser()
        {
            Email = "casey.spaulding@me.com",
            UserName = "casey.spaulding@me.com",
            FirstName = "Casey",
            LastName = "Spaulding",
            DisplayName = "Casey Spaulding",
            EmailConfirmed = true

        };

        //Step 2: Use the UserManager to create a new user that is defined by adminUser

        await _userManager.CreateAsync(adminUser, "NavyChief1893!!");

        //Step 3; Add this new user to the Administrator role

        await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());
        //BlogRole is the Enum to assign a role

        //Step 1 repeat: Create the moderator user
        var modUser = new BlogUser()
        {
            Email = "AndrewRussell@me.com",
            UserName = "AndrewRussell@me.com",
            FirstName = "Andrew",
            LastName = "Russell",
            DisplayName = "Andrew Russell",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(modUser, "NavyChief1893!!");
        await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());

    }




}
