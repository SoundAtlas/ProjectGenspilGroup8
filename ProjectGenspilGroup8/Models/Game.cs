using System.Text.Json.Serialization;

namespace ProjectGenspilGroup8.Models
{
    public class Game
    {
        // Private backing fields (used internally to enforce encapsulation)
        private string _name;
        private string _genre;
        private string _numberOfPlayers;
        private List<StockItem> _stockItems;

        public string Name => _name;
        public string Genre => _genre;
        public string NumberOfPlayers => _numberOfPlayers;
        public List<StockItem> StockItems => new List<StockItem>(_stockItems); // Return copy to protect internal state

        // Constructor for creating new games
        public Game(string name, string genre, string numberOfPlayers)
        {
            _name = name?.Trim() ?? "";
            _genre = genre?.Trim() ?? "";
            _numberOfPlayers = numberOfPlayers?.Trim() ?? "";
            _stockItems = new List<StockItem>();
        }

        // Constructor used by JSON deserialization
        [JsonConstructor]
        public Game(string name, string genre, string numberOfPlayers, List<StockItem> stockItems)
        {
            _name = name?.Trim() ?? "";
            _genre = genre?.Trim() ?? "";
            _numberOfPlayers = numberOfPlayers?.Trim() ?? "";
            _stockItems = stockItems ?? new List<StockItem>();
        }

        // Public accessors (keeps fields private while exposing data safely)
        public string GetName() => Name;
        public string GetGenre() => Genre;
        public string GetNumberOfPlayers() => NumberOfPlayers;
        public List<StockItem> GetStockItems() => StockItems;

        // Add/remove stock items (modifies internal list)
        public void AddStockItem(StockItem stockItem)
        {
            if (stockItem == null) return; // Prevent null entries
            _stockItems.Add(stockItem);
        }

        public void RemoveStockItem(StockItem stockItem)
        {
            if (stockItem == null) return; // Prevent invalid remove
            _stockItems.Remove(stockItem);
        }

        // Calculates total quantity across all stock items
        public int GetTotalQuantity()
        {
            return _stockItems.Sum(item => item.GetQuantity());
        }
    }
}