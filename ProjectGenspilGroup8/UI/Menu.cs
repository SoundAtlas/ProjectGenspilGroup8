using System;
using System.Collections.Generic;
using System.Text;
using ProjectGenspilGroup8.Services;
using ProjectGenspilGroup8.Models;

namespace ProjectGenspilGroup8.UI
{
    internal class Menu
    {
        public static void MenuMain(InventoryManager inventoryManager, Persistence.FileHandler fileHandler)
        {
            string[] menuMain =
            {
                "SØG I LAGERBEHOLDNING",
                "TILFØJ SPIL",
                "REDIGER SPIL",
                "FJERN SPIL",
                "REGISTRER FORESPØRGELSE",
                "SE FORESPØRGSELER",
                "PRINT LAGERLISTE",
                "EXPORTER LAGERLISTE TIL FIL",
                "AFSLUT",
            };

            while (true)

            {
                int? choice = ConsoleHelpers.Navigation("HOVEDMENU", menuMain);

                if (choice == null)
                {
                    return;
                }

                if (choice == 0)
                {
                    Console.Clear();

                    Console.Write("Navn: ");
                    string? gameName = Console.ReadLine();

                    Console.Write("Genre: ");
                    string? genre = Console.ReadLine();

                    Console.Write("Antal spillere: ");
                    string? numberOfPlayers = Console.ReadLine();

                    Condition? condition = ConsoleHelpers.SelectCondition("\n\n\n\nVælg stand (ESC for at springe over):");

                    decimal minPrice = ConsoleHelpers.GetOptionalDecimal("\nMinimum pris (tryk Enter for ingen): ", 0);
                    decimal maxPrice = ConsoleHelpers.GetOptionalDecimal("Maximum pris (tryk Enter for ingen): ", decimal.MaxValue);

                    List<Game> results = inventoryManager.SearchGames(gameName, genre, numberOfPlayers, condition, minPrice, maxPrice);
                    Console.Clear();

                    if (results.Count == 0)
                    {
                        Console.WriteLine("Ingen spil fundet, der matcher dine kriterier.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"Fundet {results.Count} spil.\n"); 
                        Console.WriteLine("Søgeresultater (filtreret):\n");
                        GamePrinter.PrintFilteredResults(results, condition, minPrice, maxPrice);
                        Console.ReadKey();
                    }
                    
                    Console.Clear();
                }

                if (choice == 1)
                {
                    Console.Clear();

                    string gameName = ConsoleHelpers.GetRequiredString("Spil Navn: ");

                    Console.Write("Genre: ");
                    string? genre = Console.ReadLine();

                    Console.Write("Antal spillere: ");
                    string? numberOfPlayers = Console.ReadLine();

                    Game game = new Game(gameName, genre, numberOfPlayers);
                    Console.Clear();

                    // Add stock item
                    Condition? condition = ConsoleHelpers.SelectCondition("Vælg spillets tilstand:");

                    if (condition == null)
                    {
                        Console.WriteLine("Annulleret.");
                        Console.ReadKey();
                        return;
                    }

                    decimal price = ConsoleHelpers.GetDecimal("\nPris: ");
                    int quantity = ConsoleHelpers.GetInt("Antal: ");

                    StockItem stockItem = new StockItem(condition.Value, price, quantity);
                    game.AddStockItem(stockItem);

                    // Save game
                    inventoryManager.AddGame(game);
                    fileHandler.SaveGames(inventoryManager.GetAllGames());

                    Console.Clear();

                    Console.WriteLine("Spil tilføjet! Tryk på en vilkårlig tast for at fortsætte.");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (choice == 2)
                {
                    Console.Clear();

                    string searchName = ConsoleHelpers.GetRequiredString("Indtast navnet på det spil, du vil redigere: ");

                    Game? gameToEdit = inventoryManager.FindGameByName(searchName);

                    if (gameToEdit == null)
                    {
                        Console.WriteLine("Spil ikke fundet.");
                        Console.ReadKey();
                        Console.Clear();
                        continue;
                    }

                    Console.Clear();

                    Console.WriteLine("Efterlad tom for at beholde nuværende værdi.\n");
                    Console.WriteLine($"Nuværende navn: {gameToEdit.GetName()}");
                    Console.Write("Nyt navn: ");
                    string? newName = Console.ReadLine();

                    Console.WriteLine($"\nNuværende genre: {gameToEdit.GetGenre()}");
                    Console.Write("Ny genre: ");
                    string? newGenre = Console.ReadLine();

                    Console.WriteLine($"\nNuværende antal spillere: {gameToEdit.GetNumberOfPlayers()}");
                    Console.Write("Nyt antal spillere: ");
                    string? newNumberOfPlayers = Console.ReadLine();

                    // Use old values if new ones are empty
                    string finalName = string.IsNullOrWhiteSpace(newName) ? gameToEdit.GetName() : newName;
                    string finalGenre = string.IsNullOrWhiteSpace(newGenre) ? gameToEdit.GetGenre() : newGenre;
                    string finalPlayers = string.IsNullOrWhiteSpace(newNumberOfPlayers) ? gameToEdit.GetNumberOfPlayers() : newNumberOfPlayers;

                    // Create a new Game object with the updated values (or old values if not changed)
                    Game updatedGame = new Game(finalName, finalGenre, finalPlayers);

                    // Preserve existing stock items
                    foreach (StockItem item in gameToEdit.GetStockItems())
                    {
                        updatedGame.AddStockItem(item);
                    }

                    // Replace old game
                    inventoryManager.RemoveGame(gameToEdit);
                    inventoryManager.AddGame(updatedGame);

                    // Save changes
                    fileHandler.SaveGames(inventoryManager.GetAllGames());

                    Console.WriteLine("\nSpil opdateret! Tryk på en vilkårlig tast for at fortsætte.");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (choice == 3)
                {
                    Console.Clear();

                    Console.Write("Spil Navn: ");
                    string ? gameName = Console.ReadLine();

                    List<Game> results = inventoryManager.SearchGames(gameName, "", "", condition:null, minPrice: 0, maxPrice: decimal.MaxValue);
                    Console.Clear();

                    if (results.Count == 0)
                    {
                        Console.WriteLine("Ingen spil fundet.");
                        Console.ReadKey();
                    }
                    else
                    {
                        List<string> games = new List<string>();

                        foreach (Game game in results)
                        {
                            games.Add($"- {game.GetName()} | Genre: {game.GetGenre()} | Antal spillere: {game.GetNumberOfPlayers()} | På lager: {game.GetTotalQuantity()}");
                        }

                        int? deletionChoice = ConsoleHelpers.Navigation("Vælg det spil, der skal slettes:", games.ToArray());

                        if (deletionChoice.HasValue)
                        {
                            Game selectedGame = results[deletionChoice.Value];

                            inventoryManager.RemoveGame(selectedGame);

                            // This updates the JSON file
                            fileHandler.SaveGames(inventoryManager.GetAllGames());
                            // OPTIONAL: Add confirmation (do you want to remove this game?)
                            
                            Console.WriteLine("\nSpil slettet!");

                        }
                        Console.ReadKey();
                    }

                    Console.Clear();
                }

                if (choice == 4)
                {
                    Console.Clear();

                    string customerName = ConsoleHelpers.GetRequiredString("Kunde Navn: ");

                    string gameName = ConsoleHelpers.GetRequiredString("Spil Navn: ");

                    Request request = new Request(customerName, gameName, "Under behandling");

                    inventoryManager.AddRequest(request);
                    fileHandler.SaveRequests(inventoryManager.GetAllRequests());

                    Console.Clear(); 

                    Console.WriteLine("Forespørgsel registreret. Tryk på en vilkårlig tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (choice == 5)
                {
                    Console.Clear();

                    var requests = inventoryManager.GetAllRequests();

                    if (requests.Count == 0)
                    {
                        Console.WriteLine("Ingen forespørgsler fundet.");
                    }
                    else
                    {
                        Console.WriteLine("FORESPØRGSLER:\n");

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

                if (choice == 6)
                {
                    Console.Clear();

                    string[] sortOptions =
                    {
                        "Sortér efter navn",
                        "Sortér efter genre",
                        "Ingen sortering"
                    };

                    int? sortChoice = ConsoleHelpers.Navigation("Vælg sortering for lagerlisten:", sortOptions);

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

                    Console.Clear();

                    Console.WriteLine("LAGERLISTE:\n");
                    GamePrinter.PrintGameDetails(games);

                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (choice == 7)
                {
                    Console.Clear();

                    var games = inventoryManager.GetAllGames();

                    string content = GamePrinter.FormatGameDetails(games);

                    string filePath = "..\\..\\..\\Data\\exported_inventory.txt";

                    fileHandler.ExportToFile(content, filePath);

                    Console.WriteLine("Lagerliste eksporteret til fil!");
                    Console.WriteLine($"Filsti: {filePath}");

                    Console.WriteLine("\nTryk på en tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (choice == 8)
                {
                    break;
                }
            }
        }
    }
}