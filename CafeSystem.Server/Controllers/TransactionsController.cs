using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class TransactionsController : BaseController<Transaction, Guid>
{
    public TransactionsController(TransactionService service) : base(service)
    {
    }
}
