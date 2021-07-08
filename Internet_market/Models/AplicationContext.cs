using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Internet_market.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
           // Database.OpenConnection();
            Database.EnsureDeleted();
            Database.EnsureCreated();  // создаем базу данных при первом обращении
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var password = "admin";
            var sha256 = new SHA256Managed();
            var passwordHash = Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));


            User user1 = new User
            {
                Id = 1,
                Email = "example@gmail.com",
                Password = passwordHash,
                FirstName = "Вася",
                LastName = "Пупкин"

            };

            modelBuilder.Entity<User>().HasData(user1);

            base.OnModelCreating(modelBuilder);
        }
    }
        
}
