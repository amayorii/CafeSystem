using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class ProductsController : BaseController<Product, int>
{
    public ProductsController(ProductService service) : base(service)
    {
    }
}