using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CafeSystem.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace CafeSystem.Client.Components
{
    public partial class ProductGrid : ComponentBase
    {
        [Inject]
        public HttpClient Http { get; set; } = default!;

        protected Product[]? Products;
        protected List<string> Categories = [];
        protected string SelectedCategory = "All";

        protected IEnumerable<Product> FilteredProducts =>
            SelectedCategory == "All"
                ? (Products ?? [])
                : (Products?.Where(p => p.Category == SelectedCategory) ?? []);

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Products = await Http.GetFromJsonAsync<Product[]>("api/products");
                if (Products != null)
                {
                    Categories = [.. Products.Select(p => p.Category).Distinct()];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                Products = [];
            }
        }

        protected void SelectCategory(string category)
        {
            SelectedCategory = category;
        }
    }
}
