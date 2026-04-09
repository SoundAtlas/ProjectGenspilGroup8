using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace ProjectGenspilGroup8.Models
{
    public class Game
    {
        private string _name;
        private string _genre;
        private string _numberOfPlayers;
        private List<StockItem> _stockItems;

        public string Name { get; init; }
        public string Genre { get; init; }
        public string NumberOfPlayers { get; init; }
        public List<StockItem> StockItems { get; init; }

        // Constructors
        public Game(string name, string genre, string numberOfPlayers)
        {
            Name = name;
            Genre = genre;
            NumberOfPlayers = numberOfPlayers;
            StockItems = new List<StockItem>();

            _name = Name;
            _genre = Genre;
            _numberOfPlayers = NumberOfPlayers;
            _stockItems = StockItems;
        }

        [JsonConstructor]
        public Game(string name, string genre, string numberOfPlayers, List<StockItem> stockItems)
        {
            Name = name;
            Genre = genre;
            NumberOfPlayers = numberOfPlayers;
            StockItems = stockItems ?? new List<StockItem>();

            // Keep private fields in sync
            _name = Name;
            _genre = Genre;
            _numberOfPlayers = NumberOfPlayers;
            _stockItems = StockItems;
        }

        // Properties
        public string GetName() => Name;
        public string GetGenre() => Genre;
        public string GetNumberOfPlayers() => NumberOfPlayers;
        public List<StockItem> GetStockItems() => StockItems;

        // Methods
        public void AddStockItem(StockItem stockItem) => _stockItems.Add(stockItem);
        public void RemoveStockItem(StockItem stockItem) => _stockItems.Remove(stockItem);
        public int GetTotalQuantity()
        {
            return _stockItems.Sum(item => item.GetQuantity());
        }
    }
}