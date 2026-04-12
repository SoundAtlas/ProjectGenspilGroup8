using ProjectGenspilGroup8.Models;

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

        public static decimal GetDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Feltet må ikke være tomt, prøv igen.");
                    continue;
                }

                // Use invariant culture to avoid comma/dot issues
                if (decimal.TryParse(input, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal value) && value >= 0)
                {
                    return value;
                }

                Console.WriteLine("Ugyldigt input. Prøv igen.");
            }
        }

        public static decimal GetOptionalDecimal(string prompt, decimal defaultValue)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                // Empty = use default (no filter)
                if (string.IsNullOrWhiteSpace(input))
                {
                    return defaultValue;
                }

                if (decimal.TryParse(input, out decimal value) && value >= 0)
                {
                    return value;
                }

                Console.WriteLine("Ugyldigt input. Indtast et positivt tal eller tryk Enter for at springe over.");
            }
        }

        public static int GetInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Feltet må ikke være tomt, prøv igen.");
                    continue;
                }

                if (int.TryParse(input, out int value) && value >= 0)
                {
                    return value;
                }

                Console.WriteLine("Ugyldigt input. Indtast et positivt heltal.");
            }
        }

        public static string GetRequiredString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Feltet må ikke være tomt, prøv igen.");
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim(); // Remove accidental spaces
                }
            }
        }
    }
}