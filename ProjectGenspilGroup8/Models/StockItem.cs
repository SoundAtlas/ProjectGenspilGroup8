using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ProjectGenspilGroup8.Models
{
    public class StockItem
    {
        private Condition _condition;
        private decimal _price;
        private int _quantity;

        public Condition Condition { get; init; }
        public decimal Price { get; init; }
        public int Quantity { get; init; }

        // Constructor
        [JsonConstructor]
        public StockItem(Condition condition, decimal price, int quantity)
        {
            Condition = condition;
            Price = price;
            Quantity = quantity;

            _condition = Condition;
            _price = Price;
            _quantity = Quantity;
        }

        // Properties
        public Condition GetCondition() => _condition;
        public decimal GetPrice() => _price;
        public int GetQuantity() => _quantity;

        public void SetCondition(Condition condition) => _condition = condition;
        public void SetPrice(decimal price) => _price = price;
        public void SetQuantity(int quantity) => _quantity = quantity;
    }
}