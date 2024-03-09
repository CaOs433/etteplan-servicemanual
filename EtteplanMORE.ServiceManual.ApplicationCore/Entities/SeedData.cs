using System;
using System.IO;
using System.Linq;
using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var dbContext = new FactoryDeviceDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FactoryDeviceDbContext>
                >()
            );
            try
            {
                if (dbContext.FactoryDevices.Any())
                {
                    Console.WriteLine("Database already seeded.");
                    return;
                }

                string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "seeddata.csv");
                if (!File.Exists(filepath))
                {
                    Console.WriteLine($"Seed data file not found (filePath: {filepath}).");
                    return;
                }

                // Read and parse data from the CSV-file
                string[] lines = File.ReadAllLines(filepath);
                foreach (var line in lines.Skip(1)) // Skip header row
                {
                    string[] fields = line.Split(',');
                    if (fields.Length >= 3)
                    {
                        string name = fields[0];
                        int year = int.Parse(fields[1]);
                        string type = fields[2];

                        // Add to the database
                        dbContext.FactoryDevices.Add(new FactoryDevice() { Name = name, Year = year, Type = type });
                    }
                }

                dbContext.SaveChanges();

                Console.WriteLine("Data seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }
    }
}
