﻿using Microsoft.EntityFrameworkCore;

namespace RedisExample.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "kalem1" },
                new Product() { Id = 2, Name = "kalem2" },
                new Product() { Id = 3, Name = "kalem3" });
            base.OnModelCreating(modelBuilder);
        }
    }
}
