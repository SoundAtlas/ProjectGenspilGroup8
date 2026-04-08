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

        public void RemoveGame(string gameName)
        {
            var game = FindGameByName(gameName);
            if (game != null)
                _games.Remove(game);
            // OPTIONAL: Add confirmation (do you want to remove this game?)
            // OPTIONAL: Add else-statement to give message that no such game could be found or something
        }

        // Method to finding a game by name - the question mark means it will either return a Game OR null
        public Game? FindGameByName (string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return _games.FirstOrDefault(game => game.Name.ToLower() == name.ToLower());
            }
            else
            {
                return null;
            }
        }

        public void AddRequest(Request request) => _requests.Add(request);


        public List<Game> SearchGames(string name, string genre, string players, string condition, decimal minPrice, decimal maxPrice)
        {
            List<Game> results = new List<Game>();

            foreach (Game game in _games)
            {
                bool gameMatches = true;

                // Name filter
                if (!string.IsNullOrEmpty(name))
                {
                    if (!game.Name.ToLower().Contains(name.ToLower()))
                    {
                        gameMatches = false;
                    }
                }

                // Genre
                if (!string.IsNullOrEmpty(genre))
                {
                    if (game.Genre.ToLower() != genre.ToLower())
                    {
                        gameMatches = false;
                    }
                }

                // Players
                if (!string.IsNullOrEmpty(players))
                {
                    if (game.NumberOfPlayers != players)
                    {
                        gameMatches = false;
                    }
                }

                // Condition + Price (check stock items)
                if (!string.IsNullOrEmpty(condition) || minPrice > 0 || maxPrice > 0)
                {
                    bool stockMatch = true;

                    foreach (StockItem item in game.StockItems)
                    {
                        bool itemMatches = true;

                        // Condition
                        if (!string.IsNullOrEmpty(condition))
                        {
                            if (item.GetCondition().ToLower() != condition.ToLower())
                            {
                                itemMatches = false;
                            }
                        }

                        // Min price
                        if (minPrice > 0)
                        {
                            if (item.GetPrice() < minPrice)
                            {
                                itemMatches = false;
                            }
                        }

                        // Max price
                        if (maxPrice > 0)
                        {
                            if (item.GetPrice() > maxPrice)
                            {
                                itemMatches = false;
                            }
                        }

                        if (!itemMatches)
                        {
                            stockMatch = false;
                        }
                    }

                    if (!stockMatch)
                    {
                        gameMatches = false;
                    }

                    if (gameMatches)
                    {
                        results.Add(game);
                    }
                }
            }

            return results;
        }

        public List<Game> SortGamesByName()
        {
            return _games.OrderBy(game => game.Name).ToList();
        }

        public List<Game> SortGamesByGenre()
        {
            return _games.OrderBy(game => game.Genre).ToList();
        }
    }
}
