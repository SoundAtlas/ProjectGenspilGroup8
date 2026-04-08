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

                    List<Game> results = inventoryManager.SearchGames(gameName, genre, numberOfPlayers, condition: "", minPrice: 0, maxPrice: decimal.MaxValue);
                    Console.Clear();

                    if (results.Count == 0)
                    {
                        Console.WriteLine("Ingen spil fundet, der matcher dine kriterier.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Søgeresultater:");
                        foreach (Game game in results)
                        {
                            Console.WriteLine($"- {game.Name} | Genre: {game.Genre} | Antal spillere: {game.NumberOfPlayers} | På lager: {game.GetTotalQuantity()}");
                        }
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

                    List<Game> results = inventoryManager.SearchGames(gameName, "", "", condition: "", minPrice: 0, maxPrice: decimal.MaxValue);
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
                            games.Add($"- {game.Name} | Genre: {game.Genre} | Antal spillere: {game.NumberOfPlayers} | På lager: {game.GetTotalQuantity()}");
                        }

                        int? deletionChoice = ConsoleHelpers.Navigation("Vælg det spil, der skal slettes:", games.ToArray());

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
                    //PrintInventory
                }

                if (choice == 5)
                {
                    break;
                }
            }
        }
    }
}
