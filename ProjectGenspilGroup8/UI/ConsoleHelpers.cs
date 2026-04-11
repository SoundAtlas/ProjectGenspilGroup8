using ProjectGenspilGroup8.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.UI
{
    internal class ConsoleHelpers
    {

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

        public static Condition? SelectCondition(string title)
        {
            string[] options = Enum.GetNames(typeof(Condition));

            int? choice = Navigation(title, options);

            if (choice == null)
                return null;

            return (Condition)choice.Value;
        }
    }
}