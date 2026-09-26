using Microsoft.AspNetCore.Components;
using CafeSystem.Client.Services;
using System;

namespace CafeSystem.Client.Pages
{
    public partial class Home : ComponentBase, IDisposable
    {
        [Inject]
        public OrderState OrderState { get; set; } = default!;

        protected override void OnInitialized()
        {
            OrderState.OnChange += StateHasChanged;
        }

        public void Dispose()
        {
            OrderState.OnChange -= StateHasChanged;
        }
    }
}
