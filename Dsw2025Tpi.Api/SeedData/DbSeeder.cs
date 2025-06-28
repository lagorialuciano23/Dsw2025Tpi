using Dsw2025Tpi.Data;
using Dsw2025Tpi.Domain.Entities;
using System.Text.Json;

namespace Dsw2025Tpi.Api.SeedData
{
    public static class DbSeeder
    {
        public static async Task SeedCustomersAsync(Dsw2025TpiContext context)
        {
            if (context.Customers.Any()) return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "SeedData", "customers.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Archivo customers.json no encontrado.");

            var json = await File.ReadAllTextAsync(filePath);
            var customers = JsonSerializer.Deserialize<List<Customer>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (customers != null)
            {
                context.Customers.AddRange(customers);
                await context.SaveChangesAsync();
            }
        }
    }
}