using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorisation.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Userr> Userr { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Userr>().ToTable("Userr");  // Явно указываем имя таблицы
            modelBuilder.Entity<Role>().ToTable("Role");

            // Настраиваем связь между Userr и Role
            modelBuilder.Entity<Userr>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Userrs)
                .HasForeignKey(u => u.ID_Role);  // Указываем точное имя столбца внешнего ключа
        }
    }
}
