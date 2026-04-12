using ProjectGenspilGroup8.Models;
using System.Text.Json;

namespace ProjectGenspilGroup8.Persistence
{
    public class FileHandler
    {
        // Relative paths
        private readonly string gamePath = "..\\..\\..\\Data\\games.json";
        private readonly string requestPath = "..\\..\\..\\Data\\requests.json";

        // Shared JSON options for consistent save/load behavior
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public List<Game> LoadGames()
        {
            return LoadList<Game>(gamePath, "spildata");
        }

        public void SaveGames(List<Game> games)
        {
            SaveList(gamePath, games ?? new List<Game>(), "spildata");
        }

        public List<Request> LoadRequests()
        {
            return LoadList<Request>(requestPath, "forespørgsler");
        }

        public void SaveRequests(List<Request> requests)
        {
            SaveList(requestPath, requests ?? new List<Request>(), "forespørgsler");
        }

        public void ExportToFile(string content, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Ugyldig filsti.");
                return;
            }

            try
            {
                EnsureDirectoryExists(filePath);
                File.WriteAllText(filePath, content ?? "");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved eksport af fil.");
                Console.WriteLine($"Detaljer: {ex.Message}");
                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private List<T> LoadList<T>(string path, string dataName)
        {
            try
            {
                EnsureFileExists(path);

                string json = File.ReadAllText(path);

                if (string.IsNullOrWhiteSpace(json))
                {
                    ResetFileToEmptyArray(path);
                    return new List<T>();
                }

                List<T>? items = JsonSerializer.Deserialize<List<T>>(json, jsonOptions);

                if (items == null)
                {
                    ResetFileToEmptyArray(path);
                    return new List<T>();
                }

                return items;
            }
            catch (JsonException)
            {
                Console.WriteLine($"Fejl ved indlæsning af {dataName}. Filen indeholder ugyldig JSON.");
                BackupCorruptFile(path);
                ResetFileToEmptyArray(path);

                Console.WriteLine("Filen er nulstillet til en tom liste, så programmet kan fortsætte.");
                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();

                return new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved indlæsning af {dataName}.");
                Console.WriteLine($"Detaljer: {ex.Message}");
                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();

                return new List<T>();
            }
        }

        private void SaveList<T>(string path, List<T> items, string dataName)
        {
            try
            {
                EnsureDirectoryExists(path);

                string json = JsonSerializer.Serialize(items, jsonOptions);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved gemning af {dataName}.");
                Console.WriteLine($"Detaljer: {ex.Message}");
                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void EnsureFileExists(string path)
        {
            EnsureDirectoryExists(path);

            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
            }
        }

        private void ResetFileToEmptyArray(string path)
        {
            EnsureDirectoryExists(path);
            File.WriteAllText(path, "[]");
        }

        private void BackupCorruptFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return;
                }

                string backupPath = path + ".backup";
                File.Copy(path, backupPath, true);
            }
            catch
            {
                // Backup failure should not crash the program
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            string? directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}