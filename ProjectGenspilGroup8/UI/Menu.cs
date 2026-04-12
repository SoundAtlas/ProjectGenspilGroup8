using ProjectGenspilGroup8.Models;
using ProjectGenspilGroup8.Services;

namespace ProjectGenspilGroup8.UI
{
    internal class Menu
    {
        // Main menu loop (entry point for all user interaction)
        public static void MenuMain(InventoryManager inventoryManager, Persistence.FileHandler fileHandler)
        {
            // Menu options displayed to the user
            string[] menuMain =
            {
                "SØG I LAGERBEHOLDNING",
                "TILFØJ SPIL",
                "OPDATER SPIL",
                "SLET SPIL",
                "SE LAGERLISTE",
                "REGISTRER FORESPØRGELSE",
                "SE FORESPØRGSLER",
                "EXPORTER LAGERLISTE TIL FIL",
                "AFSLUT",
            };

            // Loop runs until user chooses to exit
            while (true)
            {
                // Navigation helper returns selected index or null
                int? choice = ConsoleHelpers.Navigation("HOVEDMENU", menuMain);

                // Exit if user presses ESC
                if (choice == null)
                {
                    return;
                }

                // SEARCH INVENTORY
                if (choice == 0)
                {
                    Console.Clear();

                    // Collect optional search filters
                    Console.Write("Navn: ");
                    string? gameName = Console.ReadLine()?.Trim();

                    Console.Write("Genre: ");
                    string? genre = Console.ReadLine()?.Trim();

                    Console.Write("Antal spillere: ");
                    string? numberOfPlayers = Console.ReadLine()?.Trim();

                    // Optional condition filter (ESC = skip)
                    Condition? condition = ConsoleHelpers.SelectCondition("\n\n\n\nVælg stand (ESC for at springe over):");

                    // Optional price filters (default = no limit)
                    decimal minPrice = ConsoleHelpers.GetOptionalDecimal("\nMinimum pris (tryk Enter for ingen): ", 0);
                    decimal maxPrice = ConsoleHelpers.GetOptionalDecimal("Maximum pris (tryk Enter for ingen): ", decimal.MaxValue);

                    // Perform search in service layer
                    List<Game> results = inventoryManager.SearchGames(gameName ?? "", genre ?? "", numberOfPlayers ?? "", condition, minPrice, maxPrice);
                    Console.Clear();

                    // Handle no results
                    if (results.Count == 0)
                    {
                        Console.WriteLine("Ingen spil fundet, der matcher dine kriterier.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"Fundet {results.Count} spil.\n");
                        Console.WriteLine("Søgeresultater (filtreret):\n");

                        // Print filtered results
                        GamePrinter.PrintFilteredResults(results, condition, minPrice, maxPrice);
                        Console.ReadKey();
                    }

                    Console.Clear();
                }

                // ADD GAME
                if (choice == 1)
                {
                    Console.Clear();

                    // Required input for new game
                    string gameName = ConsoleHelpers.GetRequiredString("Spil Navn: ");

                    Console.Write("Genre: ");
                    string? genre = Console.ReadLine()?.Trim();

                    Console.Write("Antal spillere: ");
                    string? numberOfPlayers = Console.ReadLine()?.Trim();

                    // Create base game object
                    Game game = new Game(gameName, genre, numberOfPlayers);
                    Console.Clear();

                    // Add initial stock item
                    Condition? condition = ConsoleHelpers.SelectCondition("Vælg spillets tilstand:");

                    // Cancel if no condition selected
                    if (condition == null)
                    {
                        Console.WriteLine("Annulleret.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    // Required stock data
                    decimal price = ConsoleHelpers.GetDecimal("\nPris: ");
                    int quantity = ConsoleHelpers.GetInt("Antal: ");

                    // Create and attach stock item to game
                    StockItem stockItem = new StockItem(condition.Value, price, quantity);
                    game.AddStockItem(stockItem);

                    // Save game through service + persistence
                    inventoryManager.AddGame(game);
                    fileHandler.SaveGames(inventoryManager.GetAllGames());

                    Console.Clear();

                    Console.WriteLine("Spil tilføjet! Tryk på en vilkårlig tast for at fortsætte.");
                    Console.ReadKey();
                    Console.Clear();
                }

                // EDIT GAME
                if (choice == 2)
                {
                    Console.Clear();

                    // Find game by name
                    string searchName = ConsoleHelpers.GetRequiredString("Indtast navnet på det spil, du vil redigere: ");

                    Game? gameToEdit = inventoryManager.FindGameByName(searchName);

                    // Handle not found
                    if (gameToEdit == null)
                    {
                        Console.WriteLine("Spil ikke fundet.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    Console.Clear();

                    // Show current values and allow optional edits
                    Console.WriteLine("Efterlad tom for at beholde nuværende værdi.\n");
                    Console.WriteLine($"Nuværende navn: {gameToEdit.GetName()}");
                    Console.Write("Nyt navn: ");
                    string? newName = Console.ReadLine()?.Trim();

                    Console.WriteLine($"\nNuværende genre: {gameToEdit.GetGenre()}");
                    Console.Write("Ny genre: ");
                    string? newGenre = Console.ReadLine()?.Trim();

                    Console.WriteLine($"\nNuværende antal spillere: {gameToEdit.GetNumberOfPlayers()}");
                    Console.Write("Nyt antal spillere: ");
                    string? newNumberOfPlayers = Console.ReadLine()?.Trim();

                    // Use existing values if input is empty
                    string finalName = string.IsNullOrWhiteSpace(newName) ? gameToEdit.GetName() : newName;
                    string finalGenre = string.IsNullOrWhiteSpace(newGenre) ? gameToEdit.GetGenre() : newGenre;
                    string finalPlayers = string.IsNullOrWhiteSpace(newNumberOfPlayers) ? gameToEdit.GetNumberOfPlayers() : newNumberOfPlayers;

                    // Create updated game instance
                    Game updatedGame = new Game(finalName, finalGenre, finalPlayers);

                    // Preserve existing stock items
                    foreach (StockItem item in gameToEdit.GetStockItems() ?? new List<StockItem>())
                    {
                        updatedGame.AddStockItem(item);
                    }

                    // Replace old game in inventory
                    inventoryManager.RemoveGame(gameToEdit);
                    inventoryManager.AddGame(updatedGame);

                    // Persist changes
                    fileHandler.SaveGames(inventoryManager.GetAllGames());

                    Console.WriteLine("\nSpil opdateret! Tryk på en vilkårlig tast for at fortsætte.");
                    Console.ReadKey();
                    Console.Clear();
                }

                // REMOVE GAME
                if (choice == 3)
                {
                    Console.Clear();

                    // Search for games by name
                    Console.Write("Spil Navn: ");
                    string? gameName = Console.ReadLine()?.Trim();

                    List<Game> results = inventoryManager.SearchGames(gameName ?? "", "", "", condition: null, minPrice: 0, maxPrice: decimal.MaxValue);
                    Console.Clear();

                    // Handle no results
                    if (results.Count == 0)
                    {
                        Console.WriteLine("Ingen spil fundet.");
                        Console.ReadKey();
                    }

                    else
                    {
                        // Build display list for selection
                        List<string> gameOptions = new List<string>();

                        foreach (Game game in results)
                        {
                            gameOptions.Add($"- {game.GetName()} | Genre: {game.GetGenre()} | Antal spillere: {game.GetNumberOfPlayers()} | På lager: {game.GetTotalQuantity()}");
                        }

                        // Let user pick which game to delete
                        int? deletionChoice = ConsoleHelpers.Navigation("Vælg det spil, der skal slettes:", gameOptions.ToArray());

                        if (deletionChoice.HasValue)
                        {
                            Game selectedGame = results[deletionChoice.Value];

                            Console.WriteLine($"\nEr du sikker på, at du vil slette '{selectedGame.GetName()}'? (Y/N)");
                            var confirm = Console.ReadKey(true).Key;

                            if (confirm != ConsoleKey.Y)
                            {
                                Console.WriteLine("\nSletning annulleret.");
                                Console.ReadKey();
                                Console.Clear();
                                continue;
                            }

                            // Remove from inventory
                            inventoryManager.RemoveGame(selectedGame);

                            // Persist deletion
                            fileHandler.SaveGames(inventoryManager.GetAllGames());

                            Console.WriteLine("\nSpil slettet!");
                            Console.WriteLine("Tryk på en tast for at fortsætte...");
                        }
                        Console.ReadKey();
                    }

                    Console.Clear();
                }


                // PRINT INVENTORY
                if (choice == 4)
                {
                    Console.Clear();

                    // Sorting options
                    string[] sortOptions =
                    {
                        "Sortér efter navn",
                        "Sortér efter genre",
                        "Ingen sortering"
                    };

                    int? sortChoice = ConsoleHelpers.Navigation("Vælg sortering for lagerlisten:", sortOptions);

                    List<Game> games;

                    // Apply selected sorting
                    if (sortChoice == 0)
                    {
                        games = inventoryManager.SortGamesByName();
                    }
                    else if (sortChoice == 1)
                    {
                        games = inventoryManager.SortGamesByGenre();
                    }
                    else
                    {
                        games = inventoryManager.GetAllGames();
                    }

                    Console.Clear();

                    Console.WriteLine("LAGERLISTE:\n");

                    // Print full inventory
                    GamePrinter.PrintGameDetails(games);

                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }


                // REGISTER REQUEST
                if (choice == 5)
                {
                    Console.Clear();

                    // Collect request info
                    string customerName = ConsoleHelpers.GetRequiredString("Kunde Navn: ");
                    string gameName = ConsoleHelpers.GetRequiredString("Spil Navn: ");

                    // Create request with default status
                    Request request = new Request(customerName, gameName, "Under behandling");


                    // Save request
                    inventoryManager.AddRequest(request);
                    fileHandler.SaveRequests(inventoryManager.GetAllRequests());

                    Console.Clear();

                    Console.WriteLine("Forespørgsel registreret. Tryk på en vilkårlig tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }

                // VIEW REQUESTS
                if (choice == 6)
                {
                    Console.Clear();

                    var requests = inventoryManager.GetAllRequests();

                    // Handle empty request list
                    if (requests.Count == 0)
                    {
                        Console.WriteLine("Ingen forespørgsler fundet.");
                    }

                    else
                    {
                        Console.WriteLine("FORESPØRGSLER:\n");

                        // Print all requests
                        foreach (var request in requests)
                        {
                            Console.WriteLine($"Kunde: {request.CustomerName}");
                            Console.WriteLine($"Spil: {request.GameName}");
                            Console.WriteLine($"Status: {request.Status}");
                            Console.WriteLine(new string('-', 30));
                        }
                    }

                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }

                // EXPORT INVENTORY
                if (choice == 7)
                {
                    Console.Clear();

                    var games = inventoryManager.GetAllGames();

                    // Build export content
                    string content = GamePrinter.FormatGameDetails(games);

                    if (string.IsNullOrWhiteSpace(content) || content == "Ingen spil fundet.")
                    {
                        Console.WriteLine("Ingen data at eksportere.");
                        Console.WriteLine("Tryk på en tast for at fortsætte...");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    // Fixed export path (relative to project root)
                    string filePath = "..\\..\\..\\Data\\exported_inventory.txt";

                    // Save to file
                    fileHandler.ExportToFile(content, filePath);

                    Console.WriteLine("Lagerliste eksporteret til fil!");
                    Console.WriteLine($"Filsti: {filePath}");

                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }

                // EXIT
                if (choice == 8)
                {
                    break;
                }
            }
        }
    }
}