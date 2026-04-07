using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ProjectGenspilGroup8.Models
{
    public class Game
    {
        private string _name;
        private string _genre;
        private string _numberOfPlayers;
        private List<StockItem> _stockItems;

        // Constructor
        public Game(string name, string genre, string numberOfPlayers)
        {
            this._name = name;
            this._genre = genre;
            this._numberOfPlayers = numberOfPlayers;
            _stockItems = new List<StockItem>();
        }

        // Properties
        public string GetName() => _name;
        public string GetGenre() => _genre;
        public string GetNumberOfPlayers() => _numberOfPlayers;
        public List<StockItem> GetStockItems() => _stockItems;

        // Methods
        public void AddStockItem(StockItem stockItem) => _stockItems.Add(stockItem);
        public void RemoveStockItem(StockItem stockItem) => _stockItems.Remove(stockItem);
        public int GetTotalQuantity()
        {
            return _stockItems.Sum(item => item.GetQuantity());
        }
    }
}
