using System;
using Microsoft.EntityFrameworkCore;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class FactoryDeviceDbContext : DbContext
    {
        public FactoryDeviceDbContext(DbContextOptions<FactoryDeviceDbContext> options) : base(options) { }

        public DbSet<FactoryDevice> FactoryDevices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FactoryDevice>()
                .HasKey(FactoryDevice => new { FactoryDevice.Id });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbHost = Environment.GetEnvironmentVariable("DB_HOST")!;
                string dbPort = Environment.GetEnvironmentVariable("DB_PORT")!;
                string dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE")!;
                string dbUser = Environment.GetEnvironmentVariable("DB_USER")!;
                string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD")!;

                string connectionString = $"Host={dbHost};Port={dbPort};Database={dbDatabase};username={dbUser};Password={dbPassword};";

                optionsBuilder.UseNpgsql(connectionString);
            }
        }
    }
}
