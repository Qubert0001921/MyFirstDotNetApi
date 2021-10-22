using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPI.Entities
{
    public class RestaurantDbContext : DbContext    
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        private string _connectionString = "Data Source=DESKTOP-BHF858D;Initial Catalog=RestaurantDb;Integrated Security=True;";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<Dish>()
                .Property(r => r.Name)
                .IsRequired();

            modelBuilder.Entity<Address>()
                .Property(r => r.City)
                .IsRequired()
                .HasMaxLength(50);
            
            modelBuilder.Entity<Address>()
                .Property(r => r.Street)
                .IsRequired()
                .HasMaxLength(50);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}
