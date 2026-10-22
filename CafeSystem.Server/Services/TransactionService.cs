using CafeSystem.Server.Data;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Services;

public class TransactionService : BaseService<Transaction, int>
{
    public TransactionService(CafeDbContext db) : base(db)
    {
    }
}
