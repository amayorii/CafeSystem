using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class SupplyPurchasesController : BaseController<SupplyPurchase, int>
{
    public SupplyPurchasesController(SupplyPurchaseService service) : base(service)
    {

    }
}