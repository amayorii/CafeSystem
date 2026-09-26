using System;
using System.Collections.Generic;
using System.Linq;
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
            NotifyStateChanged();
        }

        public void SelectPaymentMethod(string method)
        {
            if (SelectedPaymentMethod != method)
            {
                SelectedPaymentMethod = method;
                CashInputString = "";
                CashTendered = 0;
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

        public void SubmitOrder()
        {
            Console.WriteLine($"Order submitted. Paid via: {SelectedPaymentMethod}");
            ClearOrder();
            CashTendered = 0;
            CashInputString = "";
            IsCheckingOut = false;
            SelectedPaymentMethod = "Card";
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
