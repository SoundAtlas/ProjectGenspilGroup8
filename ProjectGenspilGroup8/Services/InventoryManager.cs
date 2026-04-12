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

        public void AddGame(Game game)
        {
            if (game == null) return;
            _games.Add(game);
        }

        public bool AddGameWithStock(string name, string? genre, string? numberOfPlayers, Condition condition, decimal price, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (price < 0 || quantity < 0)
            {
                return false;
            }

            string trimmedName = name.Trim();
            string? trimmedGenre = genre?.Trim();
            string normalizedPlayers = NormalizeNumberOfPlayers(numberOfPlayers);

            Game? existingGame = FindGameByExactName(trimmedName);

            // Case 1: same game exists as a requested placeholder
            if (existingGame != null &&
                existingGame.GetTotalQuantity() == 0 &&
                HasRequestsForGame(trimmedName))
            {
                string finalGenre = string.IsNullOrWhiteSpace(trimmedGenre)
                    ? existingGame.GetGenre()
                    : trimmedGenre;

                string finalPlayers = string.IsNullOrWhiteSpace(normalizedPlayers)
                    ? existingGame.GetNumberOfPlayers()
                    : normalizedPlayers;

                Game updatedGame = new Game(trimmedName, finalGenre, finalPlayers);
                StockItem stockItem = new StockItem(condition, price, quantity);
                updatedGame.AddStockItem(stockItem);

                int index = _games.IndexOf(existingGame);
                if (index == -1)
                {
                    return false;
                }

                _games[index] = updatedGame;
                return true;
            }

            // Case 2: game already exists normally
            // Merge into existing title instead of creating duplicate game objects
            if (existingGame != null)
            {
                string finalGenre = string.IsNullOrWhiteSpace(trimmedGenre)
                    ? existingGame.GetGenre()
                    : trimmedGenre;

                string finalPlayers = string.IsNullOrWhiteSpace(normalizedPlayers)
                    ? existingGame.GetNumberOfPlayers()
                    : normalizedPlayers;

                Game mergedGame = new Game(trimmedName, finalGenre, finalPlayers);

                bool mergedIntoExistingStock = false;

                foreach (StockItem item in existingGame.GetStockItems())
                {
                    if (item.GetCondition() == condition && item.GetPrice() == price)
                    {
                        mergedGame.AddStockItem(new StockItem(item.GetCondition(), item.GetPrice(), item.GetQuantity() + quantity));
                        mergedIntoExistingStock = true;
                    }
                    else
                    {
                        mergedGame.AddStockItem(item);
                    }
                }

                if (!mergedIntoExistingStock)
                {
                    mergedGame.AddStockItem(new StockItem(condition, price, quantity));
                }

                int existingIndex = _games.IndexOf(existingGame);
                if (existingIndex == -1)
                {
                    return false;
                }

                _games[existingIndex] = mergedGame;
                return true;
            }

            // Case 3: completely new title
            Game game = new Game(trimmedName, trimmedGenre, normalizedPlayers);
            StockItem newStockItem = new StockItem(condition, price, quantity);
            game.AddStockItem(newStockItem);

            _games.Add(game);
            return true;
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

        public Game? FindGameByExactName(string gameName)
        {
            if (string.IsNullOrWhiteSpace(gameName))
            {
                return null;
            }

            return _games.FirstOrDefault(game =>
                string.Equals(game.GetName(), gameName, StringComparison.OrdinalIgnoreCase));
        }

        public bool GameExists(string gameName)
        {
            return FindGameByExactName(gameName) != null;
        }

        public bool HasRequestsForGame(string gameName)
        {
            if (string.IsNullOrWhiteSpace(gameName))
            {
                return false;
            }

            return _requests.Any(request =>
                string.Equals(request.GameName, gameName, StringComparison.OrdinalIgnoreCase));
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

        public bool RegisterRequest(string customerName, string gameName, string? genre, string? numberOfPlayers)
        {
            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(gameName))
            {
                return false;
            }

            string trimmedCustomerName = customerName.Trim();
            string trimmedGameName = gameName.Trim();

            bool gameAlreadyExists = GameExists(trimmedGameName);

            if (!gameAlreadyExists)
            {
                string normalizedPlayers = NormalizeNumberOfPlayers(numberOfPlayers);
                Game requestedGame = new Game(trimmedGameName, genre?.Trim(), normalizedPlayers);
                StockItem stockItem = new StockItem(Condition.Ny, 0, 0);
                requestedGame.AddStockItem(stockItem);
                _games.Add(requestedGame);
            }

            Request request = new Request(trimmedCustomerName, trimmedGameName, Request.StatusPending);
            _requests.Add(request);

            return true;
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

        public List<Game> SearchGames(string name, string genre, string players, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            if (minPrice > maxPrice)
            {
                return new List<Game>();
            }

            name = name?.Trim() ?? "";
            genre = genre?.Trim() ?? "";
            players = NormalizeNumberOfPlayers(players);

            List<Game> results = new List<Game>();

            foreach (Game game in _games)
            {
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
                    !MatchesPlayers(game.GetNumberOfPlayers(), players))
                {
                    continue;
                }

                if (condition.HasValue || minPrice > 0 || maxPrice < decimal.MaxValue)
                {
                    bool hasMatchingStock = false;

                    foreach (StockItem item in game.GetStockItems())
                    {
                        bool itemMatches = true;

                        if (condition.HasValue && item.GetCondition() != condition.Value)
                        {
                            itemMatches = false;
                        }

                        if (minPrice > 0 && item.GetPrice() < minPrice)
                        {
                            itemMatches = false;
                        }

                        if (maxPrice < decimal.MaxValue && item.GetPrice() > maxPrice)
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
                        continue;
                    }
                }

                results.Add(game);
            }

            return results;
        }

        public string NormalizeNumberOfPlayers(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }

            string value = input.Trim().ToLower();

            value = value.Replace("spillere", "")
                         .Replace("spiller", "")
                         .Replace("players", "")
                         .Replace("player", "")
                         .Trim();

            // Remove all spaces first
            value = value.Replace(" ", "");

            // Standardize separators
            value = value.Replace("to", "-")
                         .Replace("til", "-");

            // Clean up accidental double separators
            while (value.Contains("--"))
            {
                value = value.Replace("--", "-");
            }

            return value;
        }

        private bool MatchesPlayers(string storedValue, string searchValue)
        {
            string normalizedStored = NormalizeNumberOfPlayers(storedValue);
            string normalizedSearch = NormalizeNumberOfPlayers(searchValue);

            return string.Equals(normalizedStored, normalizedSearch, StringComparison.OrdinalIgnoreCase);
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