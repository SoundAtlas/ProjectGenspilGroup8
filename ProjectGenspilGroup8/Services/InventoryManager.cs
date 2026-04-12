using ProjectGenspilGroup8.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.Services
{
    public class InventoryManager
    {
        // In-memory storage for games and requests
        private List<Game> _games;
        private List<Request> _requests;

        // Constructor
        public InventoryManager()
        {
            _games = new List<Game>();
            _requests = new List<Request>();
        }

        // Properties
        public List<Game> GetAllGames() => new List<Game>(_games); // Return copy to protect internal state
        public List<Request> GetAllRequests() => new List<Request>(_requests);

        // Add/remove operations
        public void AddGame(Game game)
        {
            if (game == null) return; // Prevent null entries
            _games.Add(game);
        }

        public void RemoveGame(Game game)
        {
            if (game == null) return; // Avoid invalid remove call
            _games.Remove(game);
        }

        // Returns matching game or null if not found / invalid input
        public Game? FindGameByName (string name)
        {
            // Prevent unnecessary search on invalid input
            if (!string.IsNullOrEmpty(name))
            {
                return _games.FirstOrDefault(game =>
                    string.Equals(game.GetName(), name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return null;
            }
        }

        public void AddRequest(Request request)
        {
            if (request == null) return; // Prevent null entries
            _requests.Add(request);
        }

        // Filters games based on multiple optional criteria
        public List<Game> SearchGames(string name, string genre, string players, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            // Invalid price range -> no results
            if (minPrice > maxPrice)
            {
                return new List<Game>();
            }

            List<Game> results = new List<Game>();

            foreach (Game game in _games)
            {
                // Assume match until a filter fails
                bool gameMatches = true;

                // Name filter (partial match)
                if (!string.IsNullOrEmpty(name) &&
                    !game.GetName().Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Genre filter (exact match)
                if (!string.IsNullOrEmpty(genre) &&
                    !string.Equals(game.GetGenre(), genre, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Players filter
                if (!string.IsNullOrEmpty(players) &&
                    game.GetNumberOfPlayers() != players)
                {
                    continue;
                }

                // Apply stock filters only if at least one is active
                if (condition.HasValue || minPrice > 0 || maxPrice < decimal.MaxValue)
                {
                    bool hasMatchingStock = false;

                    // Check if ANY stock item satisfies filters
                    foreach (StockItem item in game.GetStockItems())
                    {
                        bool itemMatches = true;

                        // Condition
                        if (condition.HasValue &&
                            item.GetCondition() != condition.Value)
                        {
                            itemMatches = false;
                        }

                        // Min price
                        if (minPrice > 0 &&
                            item.GetPrice() < minPrice)
                        {
                            itemMatches = false;
                        }

                        // Max price
                        if (maxPrice < decimal.MaxValue &&
                            item.GetPrice() > maxPrice)
                        {
                            itemMatches = false;
                        }

                        // At least ONE matching stock item is enough
                        if (itemMatches)
                        {
                            hasMatchingStock = true;
                            break;
                        }
                    }

                    if (!hasMatchingStock)
                    {
                        gameMatches = false;
                    }
                }

                if (gameMatches)
                {
                    results.Add(game);
                }
            }

            return results;
        }

        // Returns sorted copies (does not modify original list)
        public List<Game> SortGamesByName()
        {
            return _games.OrderBy(game => game.GetName()).ToList();
        }

        public List<Game> SortGamesByGenre()
        {
            return _games.OrderBy(game => game.GetGenre()).ToList();
        }
    }
}