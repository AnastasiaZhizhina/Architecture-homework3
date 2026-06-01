using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Smartphone> Smartphones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=electronics.db");
    }

    public void InitializeDatabase()
    {
        Database.EnsureCreated();

        if (!Manufacturers.Any())
        {
            var manufacturers = new[]
            {
                new Manufacturer { Name = "Apple" },
                new Manufacturer { Name = "Samsung" },
                new Manufacturer { Name = "Xiaomi" },
                new Manufacturer { Name = "Google" }
            };
            Manufacturers.AddRange(manufacturers);
            SaveChanges();
        }

        if (!Smartphones.Any())
        {
            var apple = Manufacturers.First(m => m.Name == "Apple");
            var samsung = Manufacturers.First(m => m.Name == "Samsung");
            var xiaomi = Manufacturers.First(m => m.Name == "Xiaomi");
            var google = Manufacturers.First(m => m.Name == "Google");

            var smartphones = new[]
            {
                new Smartphone { Model = "iPhone 14", Price = 65000, ManufacturerId = apple.Id },
                new Smartphone { Model = "iPhone 15", Price = 80000, ManufacturerId = apple.Id },
                new Smartphone { Model = "iPhone 13", Price = 50000, ManufacturerId = apple.Id },
                new Smartphone { Model = "Galaxy S23", Price = 60000, ManufacturerId = samsung.Id },
                new Smartphone { Model = "Galaxy A54", Price = 30000, ManufacturerId = samsung.Id },
                new Smartphone { Model = "Galaxy Z Fold5", Price = 150000, ManufacturerId = samsung.Id },
                new Smartphone { Model = "Xiaomi 13 Pro", Price = 70000, ManufacturerId = xiaomi.Id },
                new Smartphone { Model = "Redmi Note 12", Price = 25000, ManufacturerId = xiaomi.Id },
                new Smartphone { Model = "Poco F5", Price = 35000, ManufacturerId = xiaomi.Id },
                new Smartphone { Model = "Pixel 7", Price = 55000, ManufacturerId = google.Id },
                new Smartphone { Model = "Pixel 8", Price = 70000, ManufacturerId = google.Id },
                new Smartphone { Model = "Pixel 6a", Price = 35000, ManufacturerId = google.Id }
            };
            Smartphones.AddRange(smartphones);
            SaveChanges();
        }
    }
}