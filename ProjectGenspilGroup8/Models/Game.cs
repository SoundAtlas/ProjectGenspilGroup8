using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectGenspilGroup8.Models
{
    public class Game
    {
        // Properties (needed for JSON)
        public string Name { get; set; }
        public string Genre { get; set; }
        public string NumberOfPlayers { get; set; }
        public List<StockItem> StockItems { get; set; }

        // Empty constructor (required for deserialization)
        public Game()
        {
            StockItems = new List<StockItem>();
        }

        // Constructor
        public Game(string name, string genre, string numberOfPlayers)
        {
            Name = name;
            Genre = genre;
            NumberOfPlayers = numberOfPlayers;
            StockItems = new List<StockItem>();
        }

        // Methods
        public void AddStockItem(StockItem stockItem) => StockItems.Add(stockItem);
        public void RemoveStockItem(StockItem stockItem) => StockItems.Remove(stockItem);
        public int GetTotalQuantity()
        {
            return StockItems.Sum(item => item.GetQuantity());
        }
    }
}
