using ProjectGenspilGroup8.Models;
using ProjectGenspilGroup8.Services;
using System.Text;

namespace ProjectGenspilGroup8.UI
{
    internal class GamePrinter
    {
        private const string Header = "Navn            Genre           Spillere     Stand        Pris       Antal/Status";

        public static void PrintGameDetails(List<Game> games, InventoryManager inventoryManager)
        {
            Console.WriteLine(FormatGameDetails(games, inventoryManager));
        }

        public static void PrintFilteredResults(List<Game> games, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            if (games == null || games.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                return;
            }

            Console.WriteLine(Header);
            Console.WriteLine(new string('-', 85));

            foreach (Game game in games)
            {
                foreach (StockItem item in game.GetStockItems() ?? new List<StockItem>())
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
                        Console.WriteLine(FormatGameLine(game, item, false));
                    }
                }
            }
        }

        public static string FormatGameDetails(List<Game> games, InventoryManager inventoryManager)
        {
            if (games == null || games.Count == 0)
            {
                return "Ingen spil fundet.";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Header);
            sb.AppendLine(new string('-', 85));

            foreach (Game game in games)
            {
                bool showRequested = game.GetTotalQuantity() == 0 &&
                                     inventoryManager.HasRequestsForGame(game.GetName());

                foreach (StockItem item in game.GetStockItems() ?? new List<StockItem>())
                {
                    sb.AppendLine(FormatGameLine(game, item, showRequested));
                }
            }

            return sb.ToString();
        }

        private static string FormatGameLine(Game game, StockItem item, bool showRequested)
        {
            string quantityOrStatus = showRequested
                ? "Anmodet om"
                : item.GetQuantity().ToString();

            return
                $"{game.GetName(),-15} " +
                $"{game.GetGenre(),-15} " +
                $"{game.GetNumberOfPlayers(),-12} " +
                $"{item.GetCondition(),-12} " +
                $"{item.GetPrice(),-10:0.00} " +
                $"{quantityOrStatus,-12}";
        }
    }
}