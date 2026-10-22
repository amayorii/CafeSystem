using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class ProductModifiersController : BaseController<ProductModifier, int>
{
    public ProductModifiersController(ProductModifierService service) : base(service)
    {
    }
}
