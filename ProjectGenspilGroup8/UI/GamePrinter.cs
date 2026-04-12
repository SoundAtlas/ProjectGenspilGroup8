using ProjectGenspilGroup8.Models;
using System.Text;

namespace ProjectGenspilGroup8.UI
{
    internal class GamePrinter
    {
        private const string Header = "Navn            Genre           Spillere     Stand      Pris       Antal ";

        public static void PrintGameDetails(List<Game> games)
        {
            Console.WriteLine(FormatGameDetails(games));
        }

        public static void PrintFilteredResults(List<Game> games, Condition? condition, decimal minPrice, decimal maxPrice)
        {
            if (games == null || games.Count == 0)
            {
                Console.WriteLine("Ingen spil fundet.");
                return;
            }

            Console.WriteLine(Header);
            Console.WriteLine(new string('-', 75));

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
                        Console.WriteLine(FormatGameLine(game, item));
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
            sb.AppendLine(Header);
            sb.AppendLine(new string('-', 75));

            foreach (Game game in games)
            {
                foreach (StockItem item in game.GetStockItems() ?? new List<StockItem>())
                {
                    sb.AppendLine(FormatGameLine(game, item));
                }
            }

            return sb.ToString();
        }

        private static string FormatGameLine(Game game, StockItem item)
        {
            return
                $"{game.GetName(),-15} " +
                $"{game.GetGenre(),-15} " +
                $"{game.GetNumberOfPlayers(),-12} " +
                $"{item.GetCondition(),-10} " +
                $"{item.GetPrice(),-10:0.00} " +
                $"{item.GetQuantity(),-6}";
        }
    }
}