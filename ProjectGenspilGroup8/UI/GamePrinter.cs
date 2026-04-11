using System;
using System.Collections.Generic;
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
    }
}