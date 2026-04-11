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
                "FJERN SPIL",
                "REGISTRER FORESPØRGELSE",
                "PRINT LAGERLISTE",
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

                    Console.Write("\nMinimum pris (tryk Enter for ingen): ");
                    string? minPriceInput = Console.ReadLine();

                    Console.Write("Maximum pris (tryk Enter for ingen): ");
                    string? maxPriceInput = Console.ReadLine();

                    decimal minPrice = string.IsNullOrWhiteSpace(minPriceInput) ? 0 : decimal.Parse(minPriceInput);
                    decimal maxPrice = string.IsNullOrWhiteSpace(maxPriceInput) ? decimal.MaxValue : decimal.Parse(maxPriceInput);

                    List<Game> results = inventoryManager.SearchGames(gameName, genre, numberOfPlayers, condition, minPrice, maxPrice);
                    Console.Clear();

                    if (results.Count == 0)
                    {
                        Console.WriteLine("Ingen spil fundet, der matcher dine kriterier.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Søgeresultater:\n");
                        GamePrinter.PrintGameDetails(results);
                        Console.ReadKey();
                    }
                    
                    Console.Clear();
                }

                if (choice == 1)
                {
                    Console.Clear();

                    Console.Write("Spil Navn: ");
                    string? gameName = Console.ReadLine();

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

                    Console.Write("\nPris: ");
                    decimal price = decimal.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Antal: ");
                    int quantity = int.Parse(Console.ReadLine() ?? "0");

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

                if (choice == 3)
                {
                    Console.Clear();

                    Console.Write("Kunde Navn: ");
                    string? customerName = Console.ReadLine();

                    Console.Write("Spil Navn: ");
                    string? gameName = Console.ReadLine(); 

                    Request request = new Request(customerName, gameName, "Under behandling");

                    inventoryManager.AddRequest(request);
                    fileHandler.SaveRequests(inventoryManager.GetAllRequests());

                    Console.Clear(); 

                    Console.WriteLine("Forespørgsel registreret. Tryk på en vilkårlig tast for at fortsætte...");
                    Console.ReadKey();
                    Console.Clear();
                }

                if (choice == 4)
                {
                    if (choice == 4)
                    {
                        Console.Clear();

                        var games = inventoryManager.GetAllGames();

                        Console.WriteLine("LAGERLISTE:\n");
                        GamePrinter.PrintGameDetails(games);

                        Console.WriteLine("\nTryk på en tast for at fortsætte...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }

                if (choice == 5)
                {
                    break;
                }
            }
        }
    }
}