using ProjectGenspilGroup8.Services;
using ProjectGenspilGroup8.UI;
using ProjectGenspilGroup8.Persistence;

namespace ProjectGenspilGroup8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Wrap entire startup to prevent hard crash on unexpected errors
            try
            {
                // Handles reading/writing data (file system access)
                FileHandler fileHandler = new FileHandler();

                // Holds in-memory state + business logic
                InventoryManager inventoryManager = new InventoryManager();

                // Load saved games into memory
                foreach (var game in fileHandler.LoadGames())
                {
                    inventoryManager.AddGame(game);
                }

                // Load saved requests into memory
                foreach (var request in fileHandler.LoadRequests())
                {
                    inventoryManager.AddRequest(request);
                }

                // Start UI loop (Program acts as composition root)
                Menu.MenuMain(inventoryManager, fileHandler);
            }

            catch (Exception ex)
            {
                // Last-resort safety net (not a substitute for proper error handling in layers)
                Console.WriteLine("En uventet fejl opstod.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                Console.WriteLine("\n\nStackTrace:");
                Console.WriteLine(ex.StackTrace);

                // Keep console open so user can read the error
                Console.WriteLine("Tryk på en tast for at afslutte...");
                Console.ReadKey();
            }
        }
    }
}