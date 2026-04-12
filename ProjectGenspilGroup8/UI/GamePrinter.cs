using System;
using System.Collections.Generic;
using System.Text;
using ProjectGenspilGroup8.Models;

namespace ProjectGenspilGroup8.UI
{
    internal class GamePrinter
    {
        public static void PrintGameDetails(List<Game> games)
        {
            if (games == null || games.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                return;
            }

            Console.WriteLine(
                $"{"Navn",-15} {"Genre",-15} {"Spillere",-12} {"Stand", -10} {"Pris",-10} {"Antal",-6}");
            Console.WriteLine(new string('-', 75));

            foreach (Game game in games)
            {
                foreach (StockItem item in game.GetStockItems())
                {
                    Console.WriteLine(
                        $"{game.GetName(),-15} " +
                        $"{game.GetGenre(),-15} " +
                        $"{game.GetNumberOfPlayers(),-12} " +
                        $"{item.GetCondition(),-10} " +
                        $"{item.GetPrice(),-10} " +
                        $"{item.GetQuantity(),-6}");
                }
            }
        }

        public static void PrintFilteredResults(List<Game> games, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            if (games == null || games.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                return;
            }

            Console.WriteLine(
                $"{"Navn",-15} {"Genre",-15} {"Spillere",-12} {"Stand",-10} {"Pris",-10} {"Antal",-6}");
            Console.WriteLine(new string('-', 75));

            foreach (Game game in games)
            {
                foreach (StockItem item in game.GetStockItems())
                {
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

                    if (matches)
                    {
                        Console.WriteLine(
                            $"{game.GetName(),-15} " +
                            $"{game.GetGenre(),-15} " +
                            $"{game.GetNumberOfPlayers(),-12} " +
                            $"{item.GetCondition(),-10} " +
                            $"{item.GetPrice(),-10} " +
                            $"{item.GetQuantity(),-6}");
                    }
                }
            }
        }

        public static string FormatGameDetails(List<Game> games)
        {
            if (games == null || games.Count == 0)
            {
                return "Ingen spil fundet.";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{"Navn",-15} {"Genre",-15} {"Spillere",-12} {"Stand",-10} {"Pris",-10} {"Antal",-6}");
            sb.AppendLine(new string('-', 75));

            foreach (Game game in games)
            {
                foreach (StockItem item in game.GetStockItems())
                {
                    sb.AppendLine(
                        $"{game.GetName(),-15} " +
                        $"{game.GetGenre(),-15} " +
                        $"{game.GetNumberOfPlayers(),-12} " +
                        $"{item.GetCondition(),-10} " +
                        $"{item.GetPrice(),-10} " +
                        $"{item.GetQuantity(),-6}\n");
                }
            }

            return sb.ToString();
        }
    }
}