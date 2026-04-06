using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.Models
{
    public class StockItem
    {
        private string _condition;
        private decimal _price;
        private int _quantity;

        // Constructor
        public StockItem(string condition, decimal price, int quantity)
        {
            _condition = condition;
            _price = price;
            _quantity = quantity;
        }

        // Methods
        public string GetCondition() => _condition;
        public decimal GetPrice() => _price;
        public int GetQuantity() => _quantity;

        public void SetCondition(string condition) => _condition = condition;
        public void SetPrice(decimal price) => _price = price;
        public void SetQuantity(int quantity) => _quantity = quantity;
    }
}
