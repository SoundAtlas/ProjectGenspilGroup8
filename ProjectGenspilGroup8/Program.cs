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
                // Responsible for reading/writing data from storage
                Persistence.FileHandler fileHandler = new Persistence.FileHandler();

                // Central service that holds in-memory state and business logic
                InventoryManager inventoryManager = new InventoryManager();

                // Load persisted games into the in-memory inventory
                // This ensures that the application starts with existing data if available
                foreach (var game in fileHandler.LoadGames())
                {
                    inventoryManager.AddGame(game);
                }

                // Load persisted requests into the inventory
                foreach (var request in fileHandler.LoadRequests())
                {
                    inventoryManager.AddRequest(request);
                }

                // Start the UI layer, passing dependencies explicitly
                // Keeps Program.cs as the composition root, while Menu.cs handles user interaction
                Menu.MenuMain(inventoryManager, fileHandler);
            }

            catch (Exception ex)
            {
                // Catch-all to prevent the app from crashing unexpectedly
                // Useful for a console app where no global handler exists
                Console.WriteLine("En uventet fejl opstod.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                // Pause so the user can read the error before the app exits
                Console.WriteLine("Tryk på en tast for at afslutte...");
                Console.ReadKey();
            }
        }

    }
}