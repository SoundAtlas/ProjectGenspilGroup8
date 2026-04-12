using System;
using System.Collections.Generic;
using System.Text;
using ProjectGenspilGroup8.Models;

namespace ProjectGenspilGroup8.UI
{
    internal class GamePrinter
    {
        // Shared table header (static instead of const due to formatting)
        private static readonly string Header =
            string.Format("{0,-15} {1,-15} {2,-12} {3,-10} {4,-10} {5,-6}", "Navn", "Genre", "Spillere", "Stand", "Pris", "Antal");

        // Prints all games with all stock items
        public static void PrintGameDetails(List<Game> games)
        {
            // Handle empty or null input
            if (games == null || games.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                return;
            }

            // Table header
            Console.WriteLine(Header);
            Console.WriteLine(new string('-', 75));

            foreach (Game game in games)
            {
                // Each stock item is printed as a separate row
                foreach (StockItem item in game.GetStockItems() ?? new List<StockItem>())
                {
                    Console.WriteLine(
                        $"{game.GetName(),-15} " +
                        $"{game.GetGenre(),-15} " +
                        $"{game.GetNumberOfPlayers(),-12} " +
                        $"{item.GetCondition(),-10} " +
                        $"{item.GetPrice(),-10:0.00} " + // Format price to 2 decimals
                        $"{item.GetQuantity(),-6}");
                }
            }
        }

        // Prints only stock items that match filter criteria
        public static void PrintFilteredResults(List<Game> games, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            // Handle empty or null input
            if (games == null || games.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                return;
            }

            // Table header
            Console.WriteLine(Header);
            Console.WriteLine(new string('-', 75));

            foreach (Game game in games)
            {
                foreach (StockItem item in game.GetStockItems() ?? new List<StockItem>())
                {
                    // Assume match until a filter condition fails
                    bool matches = true;

                    if (condition.HasValue && item.GetCondition() != condition.Value)
                    {
                        matches = false;
                    }

                    if (minPrice > 0 && item.GetPrice() < minPrice)
                    {
                        matches = false;
                    }

                    if (maxPrice < decimal.MaxValue && item.GetPrice() > maxPrice)
                    {
                        matches = false;
                    }

                    // Only print matching stock items
                    if (matches)
                    {
                        Console.WriteLine(
                            $"{game.GetName(),-15} " +
                            $"{game.GetGenre(),-15} " +
                            $"{game.GetNumberOfPlayers(),-12} " +
                            $"{item.GetCondition(),-10} " +
                            $"{item.GetPrice(),-10:0.00} " + // Format price to 2 decimals
                            $"{item.GetQuantity(),-6}");
                    }
                }
            }
        }

        // Builds formatted string (used for export instead of console output)
        public static string FormatGameDetails(List<Game> games)
        {
            // Handle empty or null input
            if (games == null || games.Count == 0)
            {
                return "Ingen spil fundet.";
            }

            // Use StringBuilder for efficient string construction
            StringBuilder sb = new StringBuilder();

            // Table header
            sb.AppendLine(Header);
            sb.AppendLine(new string('-', 75));

            foreach (Game game in games)
            {
                foreach (StockItem item in game.GetStockItems() ?? new List<StockItem>())
                {
                    sb.AppendLine(
                        $"{game.GetName(),-15} " +
                        $"{game.GetGenre(),-15} " +
                        $"{game.GetNumberOfPlayers(),-12} " +
                        $"{item.GetCondition(),-10} " +
                        $"{item.GetPrice(),-10:0.00} " + // Format price to 2 decimals
                        $"{item.GetQuantity(),-6}");
                }
            }

            return sb.ToString();
        }
    }
}