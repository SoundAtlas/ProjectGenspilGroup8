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
                    "SOEG I LAGERBEHOLDNING",
                    "TILFOEJ SPIL",
                    "FJERN SPIL",
                    "REGISTRER FORESPOERGELSE",
                    "PRINT LAGERLISTE",
                    "AFSLUT",
            };

            Navigation("HOVEDMENU", menuMain);
        }

        public static int? Navigation(string title, string[] options)
        {
            int selected = 0;
            Console.CursorVisible = false;
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(title + "\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                    {
                        Console.WriteLine($" > {options[i]}  ");
                    }
                    else
                    {
                        Console.WriteLine($"   {options[i]}   ");
                    }
                }

                var key = Console.ReadKey(true).Key;
                int lastIndex = options.Length - 1;

                if (key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    return null;
                }

                if (key == ConsoleKey.UpArrow || key == ConsoleKey.W)
                {
                    selected--;

                    if (selected < 0)
                    {
                        selected = lastIndex;
                    }
                }

                if (key == ConsoleKey.DownArrow || key == ConsoleKey.S)
                {
                    selected++;

                    if (selected > lastIndex)
                    {
                        selected = 0;
                    }
                }

                if (key == ConsoleKey.Enter)
                {
                    return selected;
                }
            }
        }
    }
}
