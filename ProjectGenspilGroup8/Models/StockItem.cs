using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.Models
{
    public class StockItem
    {
        private Condition _condition;
        private decimal _price;
        private int _quantity;

        // Constructor
        public StockItem(Condition condition, decimal price, int quantity)
        {
            _condition = condition;
            _price = price;
            _quantity = quantity;
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
