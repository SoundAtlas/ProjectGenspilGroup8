using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.UI
{
    internal class Menu
    {
        public static void MenuMain()
        {
            string[] menuMain =
            {
                "SØG I LAGERBEHOLDNING",
                "SE SPIL",
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
                    //SearchGames
                }

                if (choice == 1)
                {
                    //ViewGames
                }

                if (choice == 2)
                {
                    //AddGame
                }

                if (choice == 3)
                {
                    //RemoveGame
                }

                if (choice == 4)
                {
                    //RegisterRequest
                }

                if (choice == 5)
                {
                    //PrintInventory
                }

                if (choice == 6)
                {
                    Console.Clear();
                    break;
                }
            }
        }
    }
}
