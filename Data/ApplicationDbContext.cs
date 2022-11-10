using BlogProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


         public DbSet<Blog> Blogs { get; set; }         //Table name
         public DbSet<Post> Posts { get; set;}          //Table name
         public DbSet<Comment> Comments { get; set; }   //Table name
         public DbSet<Tag> Tags { get; set; }           //Table name



        
    }
}
