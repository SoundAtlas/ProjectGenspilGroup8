using ProjectGenspilGroup8.Models;
using ProjectGenspilGroup8.Persistence;

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

        // Read-only copies to protect internal state
        public List<Game> GetAllGames() => new List<Game>(_games);
        public List<Request> GetAllRequests() => new List<Request>(_requests);

        // Replace in-memory data when loading from persistence
        public void SetGames(List<Game> games)
        {
            _games = games ?? new List<Game>();
        }

        public void SetRequests(List<Request> requests)
        {
            _requests = requests ?? new List<Request>();
        }

        // Startup load methods
        public void LoadData(FileHandler fileHandler)
        {
            if (fileHandler == null)
            {
                return;
            }

            SetGames(fileHandler.LoadGames());
            SetRequests(fileHandler.LoadRequests());
        }

        // Save methods
        public void SaveGames(FileHandler fileHandler)
        {
            if (fileHandler == null)
            {
                return;
            }

            fileHandler.SaveGames(_games);
        }

        public void SaveRequests(FileHandler fileHandler)
        {
            if (fileHandler == null)
            {
                return;
            }

            fileHandler.SaveRequests(_requests);
        }

        public void SaveAll(FileHandler fileHandler)
        {
            if (fileHandler == null)
            {
                return;
            }

            fileHandler.SaveGames(_games);
            fileHandler.SaveRequests(_requests);
        }

        // Add/remove operations
        public void AddGame(Game game)
        {
            if (game == null) return;
            _games.Add(game);
        }

        public bool UpdateGame(Game oldGame, Game updatedGame)
        {
            if (oldGame == null || updatedGame == null)
            {
                return false;
            }

            int index = _games.IndexOf(oldGame);

            if (index == -1)
            {
                return false;
            }

            _games[index] = updatedGame;
            return true;
        }

        public void RemoveGame(Game game)
        {
            if (game == null) return;
            _games.Remove(game);
        }

        public List<Game> FindGamesByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<Game>();
            }

            return _games
                .Where(game => game.GetName()
                    .Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void AddRequest(Request request, Game? requestedGame = null)
        {
            if (request == null) return;

            _requests.Add(request);

            bool gameAlreadyExists = _games.Any(game =>
                string.Equals(game.GetName(), request.GameName, StringComparison.OrdinalIgnoreCase));

            if (!gameAlreadyExists && requestedGame != null)
            {
                _games.Add(requestedGame);
            }
        }

        public Game? FindGameByExactName(string gameName)
        {
            if (string.IsNullOrWhiteSpace(gameName))
            {
                return null;
            }

            return _games.FirstOrDefault(game =>
                string.Equals(game.GetName(), gameName, StringComparison.OrdinalIgnoreCase));
        }

        public int GetTotalQuantityForGame(string gameName)
        {
            Game? game = FindGameByExactName(gameName);

            if (game == null)
            {
                return 0;
            }

            return game.GetTotalQuantity();
        }

        public string GetStockStatusForGame(string gameName)
        {
            Game? game = FindGameByExactName(gameName);

            if (game == null)
            {
                return "Ikke i lager";
            }

            int totalQuantity = game.GetTotalQuantity();

            if (totalQuantity > 0)
            {
                return $"På lager ({totalQuantity})";
            }

            return "Ikke på lager (0)";
        }

        // Filters games based on multiple optional criteria
        public List<Game> SearchGames(string name, string genre, string players, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            if (minPrice > maxPrice)
            {
                return new List<Game>();
            }

            List<Game> results = new List<Game>();

            foreach (Game game in _games)
            {
                bool gameMatches = true;

                if (!string.IsNullOrEmpty(name) &&
                    !game.GetName().Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(genre) &&
                    !string.Equals(game.GetGenre(), genre, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(players) &&
                    game.GetNumberOfPlayers() != players)
                {
                    continue;
                }

                if (condition.HasValue || minPrice > 0 || maxPrice < decimal.MaxValue)
                {
                    bool hasMatchingStock = false;

                    foreach (StockItem item in game.GetStockItems())
                    {
                        bool itemMatches = true;

                        if (condition.HasValue &&
                            item.GetCondition() != condition.Value)
                        {
                            itemMatches = false;
                        }

                        if (minPrice > 0 &&
                            item.GetPrice() < minPrice)
                        {
                            itemMatches = false;
                        }

                        if (maxPrice < decimal.MaxValue &&
                            item.GetPrice() > maxPrice)
                        {
                            itemMatches = false;
                        }

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