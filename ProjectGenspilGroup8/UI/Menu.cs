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
                    List<Game> results = inventoryManager.SearchGames(
                        gameName ?? "",
                        genre ?? "",
                        numberOfPlayers ?? "",
                        condition,
                        minPrice,
                        maxPrice);

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

                    string searchName = ConsoleHelpers.GetRequiredString("Indtast navnet på det spil, du vil redigere: ");
                    List<Game> matches = inventoryManager.FindGamesByName(searchName);

                    if (matches.Count == 0)
                    {
                        Console.WriteLine("Ingen spil fundet.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    Console.WriteLine("\nFundne spil:");
                    for (int i = 0; i < matches.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {matches[i].GetName()} ({matches[i].GetGenre()})");
                    }

                    int selectedNumber;
                    while (true)
                    {
                        Console.Write($"\nVælg et spil (1-{matches.Count}): ");
                        string? input = Console.ReadLine();

                        if (int.TryParse(input, out selectedNumber) &&
                            selectedNumber >= 1 &&
                            selectedNumber <= matches.Count)
                        {
                            break;
                        }

                        Console.WriteLine("Ugyldigt valg. Prøv igen.");
                    }

                    Game gameToEdit = matches[selectedNumber - 1];

                    Console.Clear();
                    Console.WriteLine("Efterlad tom for at beholde nuværende værdi.\n");

                    // Current game info
                    Console.WriteLine($"Nuværende navn: {gameToEdit.GetName()}");
                    Console.Write("Nyt navn: ");
                    string? newName = Console.ReadLine()?.Trim();

                    Console.WriteLine($"\nNuværende genre: {gameToEdit.GetGenre()}");
                    Console.Write("Ny genre: ");
                    string? newGenre = Console.ReadLine()?.Trim();

                    Console.WriteLine($"\nNuværende antal spillere: {gameToEdit.GetNumberOfPlayers()}");
                    Console.Write("Nyt antal spillere: ");
                    string? newNumberOfPlayers = Console.ReadLine()?.Trim();

                    // Get current stock item
                    List<StockItem> existingStockItems = gameToEdit.GetStockItems();

                    if (existingStockItems.Count == 0)
                    {
                        Console.WriteLine("\nSpillet har ingen lagerdata og kan ikke redigeres korrekt.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    StockItem currentStockItem = existingStockItems[0];

                    // Condition
                    Console.WriteLine($"\nNuværende stand: {currentStockItem.GetCondition()}");
                    Console.WriteLine("Vil du ændre stand? (Y/N)");
                    var changeCondition = Console.ReadKey(true).Key;

                    Condition finalCondition = currentStockItem.GetCondition();

                    if (changeCondition == ConsoleKey.Y)
                    {
                        Console.Clear();
                        Condition? selectedCondition = ConsoleHelpers.SelectCondition("Vælg ny stand (ESC for at beholde nuværende):");

                        if (selectedCondition.HasValue)
                        {
                            finalCondition = selectedCondition.Value;
                        }

                        Console.Clear();
                        Console.WriteLine("Efterlad tom for at beholde nuværende værdi.\n");
                    }

                    // Price
                    Console.WriteLine($"\nNuværende pris: {currentStockItem.GetPrice():0.00}");
                    Console.Write("Ny pris: ");
                    string? priceInput = Console.ReadLine()?.Trim();

                    decimal finalPrice = currentStockItem.GetPrice();
                    if (!string.IsNullOrWhiteSpace(priceInput))
                    {
                        if (decimal.TryParse(priceInput, out decimal parsedPrice) && parsedPrice >= 0)
                        {
                            finalPrice = parsedPrice;
                        }
                        else
                        {
                            Console.WriteLine("Ugyldig pris. Nuværende pris beholdes.");
                        }
                    }

                    // Quantity
                    Console.WriteLine($"\nNuværende antal: {currentStockItem.GetQuantity()}");
                    Console.Write("Nyt antal: ");
                    string? quantityInput = Console.ReadLine()?.Trim();

                    int finalQuantity = currentStockItem.GetQuantity();
                    if (!string.IsNullOrWhiteSpace(quantityInput))
                    {
                        if (int.TryParse(quantityInput, out int parsedQuantity) && parsedQuantity >= 0)
                        {
                            finalQuantity = parsedQuantity;
                        }
                        else
                        {
                            Console.WriteLine("Ugyldigt antal. Nuværende antal beholdes.");
                        }
                    }

                    // Final values
                    string finalName = string.IsNullOrWhiteSpace(newName) ? gameToEdit.GetName() : newName;
                    string finalGenre = string.IsNullOrWhiteSpace(newGenre) ? gameToEdit.GetGenre() : newGenre;
                    string finalPlayers = string.IsNullOrWhiteSpace(newNumberOfPlayers) ? gameToEdit.GetNumberOfPlayers() : newNumberOfPlayers;

                    // Build updated game
                    Game updatedGame = new Game(finalName, finalGenre, finalPlayers);
                    StockItem updatedStockItem = new StockItem(finalCondition, finalPrice, finalQuantity);
                    updatedGame.AddStockItem(updatedStockItem);

                    bool updated = inventoryManager.UpdateGame(gameToEdit, updatedGame);

                    if (!updated)
                    {
                        Console.WriteLine("\nKunne ikke opdatere spillet.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

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

                    List<Game> results = inventoryManager.SearchGames(
                        gameName ?? "",
                        "",
                        "",
                        condition: null,
                        minPrice: 0,
                        maxPrice: decimal.MaxValue);

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

                    string customerName = ConsoleHelpers.GetRequiredString("Kunde Navn: ");
                    string gameName = ConsoleHelpers.GetRequiredString("Spil Navn: ");

                    Game? requestedGame = null;

                    bool gameAlreadyExists = inventoryManager.GetAllGames().Any(game =>
                        string.Equals(game.GetName(), gameName, StringComparison.OrdinalIgnoreCase));

                    if (!gameAlreadyExists)
                    {
                        Console.WriteLine("\nSpillet findes ikke i lageret endnu.");
                        Console.WriteLine("Det oprettes nu som et forespurgt spil med antal 0.\n");

                        Console.Write("Genre: ");
                        string? genre = Console.ReadLine()?.Trim();

                        Console.Write("Antal spillere: ");
                        string? numberOfPlayers = Console.ReadLine()?.Trim();

                        requestedGame = new Game(gameName, genre, numberOfPlayers);
                        StockItem stockItem = new StockItem(Condition.New, 0, 0);
                        requestedGame.AddStockItem(stockItem);
                    }

                    Request request = new Request(customerName, gameName, "Under behandling");

                    inventoryManager.AddRequest(request, requestedGame);

                    fileHandler.SaveRequests(inventoryManager.GetAllRequests());
                    fileHandler.SaveGames(inventoryManager.GetAllGames());

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

                    string[] sortOptions =
                    {
                        "Sortér efter navn",
                        "Sortér efter genre",
                        "Ingen sortering"
                    };

                    int? sortChoice = ConsoleHelpers.Navigation("Vælg sortering for eksportfilen:", sortOptions);

                    if (sortChoice == null)
                    {
                        Console.Clear();
                        continue;
                    }

                    List<Game> games;

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

                    string content = GamePrinter.FormatGameDetails(games);

                    if (string.IsNullOrWhiteSpace(content) || content == "Ingen spil fundet.")
                    {
                        Console.WriteLine("Ingen data at eksportere.");
                        Console.WriteLine("Tryk på en tast for at fortsætte...");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    string filePath = "..\\..\\..\\Data\\exported_inventory.txt";
                    fileHandler.ExportToFile(content, filePath);

                    Console.WriteLine("Lagerliste eksporteret til fil!");
                    Console.WriteLine($"Filsti: {filePath}");
                    Console.WriteLine($"Sortering: {sortOptions[sortChoice.Value]}");

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