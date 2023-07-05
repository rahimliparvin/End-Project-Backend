﻿using Microsoft.EntityFrameworkCore;
using Savoy.Models;

namespace Savoy.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }



    }
}