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
            await context.Set<Shift>().ExecuteDeleteAsync();
            await context.Set<Employee>().ExecuteDeleteAsync();
            await context.Set<Category>().ExecuteDeleteAsync();

            // 1. Seed Categories
            var catCoffee = new Category { Name = "Coffee", Description = "Classic and specialty coffee drinks" };
            var catPastries = new Category { Name = "Pastries", Description = "Freshly baked pastries" };
            var catDesserts = new Category { Name = "Desserts", Description = "Delicious sweet desserts" };
            var catTea = new Category { Name = "Tea", Description = "Hot organic and herbal teas" };
            var catCold = new Category { Name = "Cold Drinks", Description = "Chilled and refreshing drinks" };

            var categories = new List<Category> { catCoffee, catPastries, catDesserts, catTea, catCold };
            await context.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // 2. Seed Employee & Shift for initial orders
            var employee = new Employee
            {
                FirstName = "Alex",
                LastName = "Barista",
                Role = EmployeeRole.Barista,
                PinCode = "1234",
                BaseSalary = 800.00m,
                RevenuePercentage = 13.00m
            };

            var shift = new Shift
            {
                Employee = employee,
                StartTime = DateTime.UtcNow.AddHours(-6),
                EndTIme = DateTime.UtcNow
            };

            // 3. Seed Products with linked Category
            var products = new List<Product>
            {
                new Product { Name = "Espresso", Description = "Strong and bold classic espresso.", Price = 2.50m, Category = catCoffee, IsAvailable = true },
                new Product { Name = "Cappuccino", Description = "Espresso with steamed milk and a thick layer of foam.", Price = 3.80m, Category = catCoffee, IsAvailable = true },
                new Product { Name = "Latte", Description = "Espresso with lots of steamed milk and a light layer of foam.", Price = 4.20m, Category = catCoffee, IsAvailable = true },
                new Product { Name = "Americano", Description = "Espresso diluted with hot water.", Price = 3.00m, Category = catCoffee, IsAvailable = true },
                new Product { Name = "Mocha", Description = "Espresso with chocolate and steamed milk.", Price = 4.50m, Category = catCoffee, IsAvailable = true },
                new Product { Name = "Croissant", Description = "Buttery, flaky, and fresh from the oven.", Price = 3.50m, Category = catPastries, IsAvailable = true },
                new Product { Name = "Blueberry Muffin", Description = "Soft muffin bursting with fresh blueberries.", Price = 3.00m, Category = catPastries, IsAvailable = true },
                new Product { Name = "Cheesecake", Description = "Rich and creamy classic New York cheesecake.", Price = 5.50m, Category = catDesserts, IsAvailable = true },
                new Product { Name = "Green Tea", Description = "Refreshing and healthy hot green tea.", Price = 2.50m, Category = catTea, IsAvailable = true },
                new Product { Name = "Orange Juice", Description = "Freshly squeezed orange juice.", Price = 4.00m, Category = catCold, IsAvailable = true }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            // 4. Seed sample orders
            var order = new Order
            {
                Employee = employee,
                Shift = shift,
                TableNumber = 5,
                Status = OrderStatus.Open,
                Items = new List<OrderItem>
                {
                    new OrderItem { Product = products[1], Quantity = 2, UnitPrice = products[1].Price },
                    new OrderItem { Product = products[5], Quantity = 1, UnitPrice = products[5].Price }
                }
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            var closedOrder = new Order
            {
                Employee = employee,
                Shift = shift,
                TableNumber = 2,
                Status = OrderStatus.Closed,
                Items = new List<OrderItem>
                {
                    new OrderItem { Product = products[2], Quantity = 1, UnitPrice = products[2].Price },
                    new OrderItem { Product = products[7], Quantity = 1, UnitPrice = products[7].Price }
                }
            };
            await context.Orders.AddAsync(closedOrder);
            await context.SaveChangesAsync();

            var transaction = new Transaction
            {
                Order = closedOrder,
                Method = PaymentMethod.Card,
                TotalAmount = closedOrder.TotalAmount,
                ChangeGiven = 0
            };
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
        }

        // 5. Seed supply purchases
        if (!await context.SupplyPurchases.AnyAsync())
        {
            List<SupplyPurchase> supplies = [
                new SupplyPurchase { Name = "Paper cups 250ml (100 pcs)", Category = "Packaging", Quantity = 10, TotalCost = 65.00m },
                new SupplyPurchase { Name = "Black cup lids (100 pcs)", Category = "Packaging", Quantity = 10, TotalCost = 35.00m },
                new SupplyPurchase { Name = "Paper straws (200 pcs)", Category = "Consumables", Quantity = 5, TotalCost = 15.00m },
                new SupplyPurchase { Name = "Arabica coffee beans (1 kg)", Category = "Ingredients", Quantity = 6, TotalCost = 120.00m },
                new SupplyPurchase { Name = "Barista whole milk (case)", Category = "Ingredients", Quantity = 4, TotalCost = 88.00m },
                new SupplyPurchase { Name = "Bar napkins (pack)", Category = "Consumables", Quantity = 8, TotalCost = 24.00m }
            ];

            await context.AddRangeAsync(supplies);
            await context.SaveChangesAsync();
        }
    }
}
