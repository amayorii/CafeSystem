using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Server.Services;

public class OrderService : BaseService<Order, int>
{
    public OrderService(CafeDbContext db) : base(db)
    {
    }

    public override async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _db.Orders
            .Include(o => o.Employee)
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();
    }

    public override async Task<Order?> GetByIdAsync(int id)
    {
        return await _db.Orders
            .Include(o => o.Employee)
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> CheckoutAsync(CreateOrderRequest request)
    {
        // shift and employee
        Shift? shift = null;
        if (request.ShiftId.HasValue)
        {
            shift = await _db.Shifts.FindAsync(request.ShiftId.Value);
        }

        if (shift == null)
        {
            shift = await _db.Shifts.OrderByDescending(s => s.StartTime).FirstOrDefaultAsync();
            if (shift == null)
            {
                var employee = await _db.Employees.FirstOrDefaultAsync()
                    ?? new Employee
                    {
                        FirstName = "Alex",
                        LastName = "Barista",
                        Role = EmployeeRole.Barista,
                        PinCode = "1234",
                        BaseSalary = 800m,
                        RevenuePercentage = 13m
                    };

                if (employee.Id == 0)
                {
                    _db.Employees.Add(employee);
                    await _db.SaveChangesAsync();
                }

                shift = new Shift
                {
                    EmployeeId = employee.Id,
                    StartTime = DateTime.UtcNow,
                    EndTIme = DateTime.UtcNow.AddHours(8)
                };
                _db.Shifts.Add(shift);
                await _db.SaveChangesAsync();
            }
        }

        var employeeId = request.EmployeeId ?? shift.EmployeeId;

        // create order
        var order = new Order
        {
            EmployeeId = employeeId,
            ShiftId = shift.Id,
            CustomerId = request.CustomerId,
            TableNumber = request.TableNumber > 0 ? request.TableNumber : 1,
            TipAmount = request.TipAmount,
            Status = OrderStatus.Closed,
            CreatedAt = DateTime.UtcNow,
            Items = []
        };

        foreach (var itemDto in request.Items)
        {
            var unitPrice = itemDto.UnitPrice;
            if (unitPrice <= 0)
            {
                var product = await _db.Products.FindAsync(itemDto.ProductId);
                unitPrice = product?.Price ?? 0m;
            }

            order.Items.Add(new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity > 0 ? itemDto.Quantity : 1,
                UnitPrice = unitPrice,
                Notes = itemDto.Notes ?? string.Empty
            });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        // create transaction
        var transaction = new Transaction
        {
            OrderId = order.Id,
            TotalAmount = order.TotalAmount,
            Method = request.PaymentMethod,
            ChangeGiven = request.ChangeGiven,
            CreatedAt = DateTime.UtcNow
        };
        _db.Transactions.Add(transaction);

        // update shift revenue and tips
        shift.TotalRevenue += order.TotalAmount;
        shift.TotalTips += order.TipAmount;
        _db.Shifts.Update(shift);

        await _db.SaveChangesAsync();

        return await _db.Orders
            .Include(o => o.Employee)
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstAsync(o => o.Id == order.Id);
    }
}
