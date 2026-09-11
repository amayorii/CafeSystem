using CafeSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Server.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(CafeDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        if (await context.Products.CountAsync() < 10)
        {
            await context.Transactions.ExecuteDeleteAsync();
            await context.OrderItems.ExecuteDeleteAsync();
            await context.Orders.ExecuteDeleteAsync();
            await context.Products.ExecuteDeleteAsync();
            var products = new List<Product>
            {
                new Product { Name = "Espresso", Description = "Strong and bold classic espresso.", Price = 2.50m, Category = "Coffee" },
                new Product { Name = "Cappuccino", Description = "Espresso with steamed milk and a thick layer of foam.", Price = 3.80m, Category = "Coffee" },
                new Product { Name = "Latte", Description = "Espresso with lots of steamed milk and a light layer of foam.", Price = 4.20m, Category = "Coffee" },
                new Product { Name = "Americano", Description = "Espresso diluted with hot water.", Price = 3.00m, Category = "Coffee" },
                new Product { Name = "Mocha", Description = "Espresso with chocolate and steamed milk.", Price = 4.50m, Category = "Coffee" },
                new Product { Name = "Croissant", Description = "Buttery, flaky, and fresh from the oven.", Price = 3.50m, Category = "Pastries" },
                new Product { Name = "Blueberry Muffin", Description = "Soft muffin bursting with fresh blueberries.", Price = 3.00m, Category = "Pastries" },
                new Product { Name = "Cheesecake", Description = "Rich and creamy classic New York cheesecake.", Price = 5.50m, Category = "Desserts" },
                new Product { Name = "Green Tea", Description = "Refreshing and healthy hot green tea.", Price = 2.50m, Category = "Tea" },
                new Product { Name = "Orange Juice", Description = "Freshly squeezed orange juice.", Price = 4.00m, Category = "Cold Drinks" }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // Seed a sample order if there are no orders
            if (!await context.Orders.AnyAsync())
            {
                var order = new Order
                {
                    TableNumber = 5,
                    Status = OrderStatus.Open,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Product = products[1], Quantity = 2 },
                        new OrderItem { Product = products[5], Quantity = 1 }
                    }
                };

                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();

                // Seed a transaction for a closed order just to have one
                var closedOrder = new Order
                {
                    TableNumber = 2,
                    Status = OrderStatus.Closed,
                    Items = new List<OrderItem>
                    {
                        new OrderItem { Product = products[2], Quantity = 1 },
                        new OrderItem { Product = products[7], Quantity = 1 }
                    }
                };
                await context.Orders.AddAsync(closedOrder);
                await context.SaveChangesAsync();

                var transaction = new Transaction
                {
                    OrderId = closedOrder.Id,
                    Method = PaymentMethod.Card,
                    TotalAmount = closedOrder.TotalAmount,
                    ChangeGiven = 0
                };
                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();
            }
        }
    }
}
