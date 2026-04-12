using System;
using System.Text.Json.Serialization;

namespace ProjectGenspilGroup8.Models
{
    public class StockItem
    {
        // Private backing fields (single source of truth)
        private Condition _condition;
        private decimal _price;
        private int _quantity;

        // Read-only properties for safe external access
        public Condition Condition => _condition;
        public decimal Price => _price;
        public int Quantity => _quantity;

        // Constructor used for both manual creation and JSON deserialization
        [JsonConstructor]
        public StockItem(Condition condition, decimal price, int quantity)
        {
            _condition = condition;
            
            // Ensure valid values (prevent negative data)
            _price = price < 0 ? 0 : price;
            _quantity = quantity < 0 ? 0 : quantity;
        }

        // Accessors (keeps usage consistent with rest of codebase)
        public Condition GetCondition() => _condition;
        public decimal GetPrice() => _price;
        public int GetQuantity() => _quantity;

        // Controlled updates (ensures valid state)
        public void SetCondition(Condition condition)
        {
            _condition = condition;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0) return; // Prevent invalid price
            _price = price;
        }

        public void SetQuantity(int quantity)
        {
            if (quantity < 0) return; // Prevent invalid quantity
            _quantity = quantity;
        }
    }
}