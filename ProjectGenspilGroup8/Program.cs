using ProjectGenspilGroup8.Persistence;
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
                FileHandler fileHandler = new FileHandler();
                InventoryManager inventoryManager = new InventoryManager();

                // Load all persisted data through the service layer
                inventoryManager.LoadData(fileHandler);

                // Start menu
                Menu.MenuMain(inventoryManager, fileHandler);
            }
            catch (Exception ex)
            {
                Console.WriteLine("En uventet fejl opstod.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                Console.WriteLine("\nStackTrace:");
                Console.WriteLine(ex.StackTrace);

                Console.WriteLine("\nTryk på en tast for at afslutte...");
                Console.ReadKey();
            }
        }
    }
}