using ProjectGenspilGroup8.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.Services
{
    public class InventoryManager
    {
        private List<Game> _games;
        private List<Request> _requests;

        // Constructor
        public InventoryManager()
        {
            _games = new List<Game>();
            _requests = new List<Request>();
        }

        // Properties
        public List<Game> GetAllGames() => _games;
        public List<Request> GetAllRequests() => _requests;

        // Methods
        public void AddGame(Game game)
        {
            _games.Add(game);
        }

        public void RemoveGame(Game game)
        {
            _games.Remove(game);
        }

        // Method to finding a game by name - the question mark means it will either return a Game OR null
        public Game? FindGameByName (string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return _games.FirstOrDefault(game => game.GetName().ToLower() == name.ToLower());
            }
            else
            {
                return null;
            }
        }

        public void AddRequest(Request request) => _requests.Add(request);


        public List<Game> SearchGames(string name, string genre, string players, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            List<Game> results = new List<Game>();

            foreach (Game game in _games)
            {
                bool gameMatches = true;

                // 🔹 Name filter
                if (!string.IsNullOrEmpty(name) &&
                    !game.GetName().ToLower().Contains(name.ToLower()))
                {
                    gameMatches = false;
                }

                // 🔹 Genre filter
                if (!string.IsNullOrEmpty(genre) &&
                    game.GetGenre().ToLower() != genre.ToLower())
                {
                    gameMatches = false;
                }

                // 🔹 Players filter
                if (!string.IsNullOrEmpty(players) &&
                    game.GetNumberOfPlayers() != players)
                {
                    gameMatches = false;
                }

                // 🔹 Stock-based filtering
                if (condition.HasValue || minPrice > 0 || maxPrice < decimal.MaxValue)
                {
                    bool hasMatchingStock = false;

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

                        // ✅ KEY FIX: at least ONE match
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
