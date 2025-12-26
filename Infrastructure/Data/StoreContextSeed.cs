using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        if (!context.Products.Any())
        {
            var productData = await File.ReadAllTextAsync(path + @"/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productData);

            if (products == null) return;
            
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }

        if (!context.DeliveryMethods.Any())
        {
            var deliveryMethodData = await File.ReadAllTextAsync(path + @"/Data/SeedData/delivery.json");
            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);

            if (deliveryMethods == null) return;
            
            context.DeliveryMethods.AddRange(deliveryMethods);
            await context.SaveChangesAsync();
        }
    }
}
