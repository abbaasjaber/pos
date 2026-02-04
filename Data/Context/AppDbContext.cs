using Microsoft.EntityFrameworkCore;
using NexusPOS.Models;
using System;
using System.IO;

namespace NexusPOS.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=nexuspos.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(p => p.Barcode);
            modelBuilder.Entity<Product>().HasIndex(p => p.Name);
            modelBuilder.Entity<Invoice>().HasIndex(i => i.InvoiceNo).IsUnique();

            modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Invoice>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Products)
                .HasForeignKey(p => p.GroupId);

            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Username = "admin", 
                    PasswordHash = "admin", 
                    Role = UserRole.Admin,
                    FullName = "System Administrator",
                    CreatedAt = DateTime.Now 
                }
            );
            
            modelBuilder.Entity<ProductGroup>().HasData(
                new ProductGroup { Id = 1, Name = "General", CreatedAt = DateTime.Now }
            );
        }
    }
}