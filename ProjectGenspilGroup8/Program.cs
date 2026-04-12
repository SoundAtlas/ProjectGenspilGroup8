using ProjectGenspilGroup8.Services;
using ProjectGenspilGroup8.UI;

namespace ProjectGenspilGroup8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Persistence.FileHandler fileHandler = new Persistence.FileHandler();
                InventoryManager inventoryManager = new InventoryManager();

                foreach (var game in fileHandler.LoadGames())
                {
                    inventoryManager.AddGame(game);
                }

                foreach (var request in fileHandler.LoadRequests())
                {
                    inventoryManager.AddRequest(request);
                }

                Menu.MenuMain(inventoryManager, fileHandler);
            }

            catch (Exception ex)
            {
                Console.WriteLine("En uventet fejl opstod.");
                Console.WriteLine($"Detaljer: {ex.Message}");
                Console.WriteLine("Tryk på en tast for at afslutte...");
                Console.ReadKey();
            }
        }

    }
}