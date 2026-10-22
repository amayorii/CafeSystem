using CafeSystem.Server.Services;
using CafeSystem.Shared.Models;

namespace CafeSystem.Server.Controllers;

public class CategoriesController : BaseController<Category, int>
{
    public CategoriesController(CategoryService service) : base(service)
    {
    }
}
