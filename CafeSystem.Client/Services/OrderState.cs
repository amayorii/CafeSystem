using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CafeSystem.Shared.Models;

namespace CafeSystem.Client.Services
{
    public class CartItem
    {
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
        public bool IsExpanded { get; set; }
    }

    public class OrderState
    {
        private readonly HttpClient _http;

        public OrderState(HttpClient http)
        {
            _http = http;
        }

        // Cart state
        public List<CartItem> CurrentOrder { get; private set; } = new();

        public decimal Subtotal => Math.Round(CurrentOrder.Sum(i => i.Product.Price * i.Quantity), 2, MidpointRounding.AwayFromZero);
        public decimal Tax => Math.Round(Subtotal * 0.08m, 2, MidpointRounding.AwayFromZero);
        public decimal Total => Math.Round(Subtotal + Tax, 2, MidpointRounding.AwayFromZero);

        // Checkout UI state
        public bool IsCheckingOut { get; private set; } = false;
        public string SelectedPaymentMethod { get; private set; } = "Card";
        public string CashInputString { get; private set; } = "";
        public decimal CashTendered { get; private set; } = 0;
        public decimal Change => Math.Max(0, CashTendered - Total);

        // Async submit state
        public bool IsSubmitting { get; private set; } = false;
        public string? SubmitError { get; private set; }
        public Order? LastCompletedOrder { get; private set; }
        public decimal LastChangeGiven { get; private set; }
        public bool ShowSuccessModal { get; private set; } = false;

        public bool CanSubmitOrder
        {
            get
            {
                if (IsSubmitting || !CurrentOrder.Any()) return false;
                if (SelectedPaymentMethod == "Cash")
                {
                    return CashTendered >= Total && Total > 0;
                }
                return Total > 0;
            }
        }

        public event Action? OnChange;

        // Cart methods

        public void AddToOrder(Product product)
        {
            var existingItem = CurrentOrder.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                CurrentOrder.Add(new CartItem { Product = product, Quantity = 1, IsExpanded = false });
            }
            NotifyStateChanged();
        }

        public void ToggleExpand(CartItem item)
        {
            if (IsCheckingOut) return;

            foreach (var orderItem in CurrentOrder)
            {
                if (orderItem != item)
                    orderItem.IsExpanded = false;
            }

            item.IsExpanded = !item.IsExpanded;
            NotifyStateChanged();
        }

        public void IncreaseQuantity(CartItem item)
        {
            item.Quantity++;
            NotifyStateChanged();
        }

        public void DecreaseQuantity(CartItem item)
        {
            item.Quantity--;
            if (item.Quantity <= 0)
            {
                CurrentOrder.Remove(item);
            }
            NotifyStateChanged();
        }

        public void ClearOrder()
        {
            CurrentOrder.Clear();
            NotifyStateChanged();
        }

        // Checkout methods

        public void StartCheckout()
        {
            IsCheckingOut = true;
            SubmitError = null;
            CashInputString = "";
            CashTendered = 0;

            foreach (var item in CurrentOrder)
            {
                item.IsExpanded = false;
            }
            NotifyStateChanged();
        }

        public void CancelCheckout()
        {
            IsCheckingOut = false;
            SubmitError = null;
            NotifyStateChanged();
        }

        public void SelectPaymentMethod(string method)
        {
            if (SelectedPaymentMethod != method)
            {
                SelectedPaymentMethod = method;
                CashInputString = "";
                CashTendered = 0;
                SubmitError = null;
                NotifyStateChanged();
            }
        }

        public void TypeNumpad(string key)
        {
            if (key == "." && CashInputString.Contains(".")) return;

            if (CashInputString.Contains("."))
            {
                var parts = CashInputString.Split('.');
                if (parts.Length > 1 && parts[1].Length >= 2) return;
            }

            string tempInput = CashInputString + key;
            if (decimal.TryParse(tempInput, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var amount))
            {
                if (amount > 10000m) return;

                CashInputString = tempInput;
                CashTendered = amount;
            }
            NotifyStateChanged();
        }

        public void SetCash(decimal amount)
        {
            if (amount > 10000m) amount = 10000m;
            CashTendered = amount;
            CashInputString = amount.ToString("0.##");
            NotifyStateChanged();
        }

        public void ClearNumpad()
        {
            CashInputString = "";
            CashTendered = 0;
            NotifyStateChanged();
        }

        public async Task<bool> SubmitOrderAsync()
        {
            if (!CanSubmitOrder) return false;

            IsSubmitting = true;
            SubmitError = null;
            NotifyStateChanged();

            try
            {
                var change = SelectedPaymentMethod == "Cash" ? Change : 0m;
                var request = new CreateOrderRequest
                {
                    PaymentMethod = SelectedPaymentMethod == "Cash" ? PaymentMethod.Cash : PaymentMethod.Card,
                    CashTendered = SelectedPaymentMethod == "Cash" ? CashTendered : Total,
                    ChangeGiven = change,
                    TableNumber = 1,
                    Items = CurrentOrder.Select(i => new CreateOrderItemDto
                    {
                        ProductId = i.Product.Id,
                        Quantity = i.Quantity,
                        UnitPrice = i.Product.Price
                    }).ToList()
                };

                var response = await _http.PostAsJsonAsync("api/orders/checkout", request);
                if (response.IsSuccessStatusCode)
                {
                    var createdOrder = await response.Content.ReadFromJsonAsync<Order>();
                    LastCompletedOrder = createdOrder;
                    LastChangeGiven = change;
                    ShowSuccessModal = true;

                    ClearOrder();
                    CashTendered = 0;
                    CashInputString = "";
                    IsCheckingOut = false;
                    SelectedPaymentMethod = "Card";
                    IsSubmitting = false;
                    NotifyStateChanged();
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    SubmitError = string.IsNullOrWhiteSpace(error) ? "Server error occurred during checkout" : error;
                    IsSubmitting = false;
                    NotifyStateChanged();
                    return false;
                }
            }
            catch (Exception ex)
            {
                SubmitError = $"Connection error: {ex.Message}";
                IsSubmitting = false;
                NotifyStateChanged();
                return false;
            }
        }

        public void DismissSuccessModal()
        {
            ShowSuccessModal = false;
            LastCompletedOrder = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
