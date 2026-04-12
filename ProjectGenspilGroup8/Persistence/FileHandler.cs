using System;
using System.Collections.Generic;
using ProjectGenspilGroup8.Models;
using System.Text.Json;

namespace ProjectGenspilGroup8.Persistence
{
    internal class FileHandler
    {
        // Relative paths (keeps project portable within solution structure)
        string gamePath = "..\\..\\..\\Data\\games.json";
        string requestPath = "..\\..\\..\\Data\\requests.json";

        public List<Game> LoadGames()
        {
            // Create file if missing to avoid read errors
            if (!File.Exists(gamePath))
            {
                EnsureDirectoryExists(gamePath);
                File.WriteAllText(gamePath, "[]");
                return new List<Game>();
            }

            try
            {
                // Read raw JSON from file
                string json = File.ReadAllText(gamePath);

                // Handle empty/invalid content safely
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Game>();
                }

                // Deserialize, fallback if null (extra safety)
                return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
            }

            catch (Exception ex)
            {
                // Fail-safe: prevent crash on corrupt/unreadable file
                Console.WriteLine("Fejl ved indlæsning af spildata.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();

                return new List<Game>();
            }
        }

        public void SaveGames(List<Game> games)
        {
            // Serialize with formatting for readability
            string json = JsonSerializer.Serialize(games, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            try
            {
                // Ensure directory exists before writing
                EnsureDirectoryExists(gamePath);
                File.WriteAllText(gamePath, json);
            }

            catch (Exception ex)
            {
                // Fail-safe: prevent crash if write fails
                Console.WriteLine("Fejl ved gemning af spildata.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public List<Request> LoadRequests()
        {
            // Same logic as LoadGames for consistency
            if (!File.Exists(requestPath))
            {
                EnsureDirectoryExists(requestPath);
                File.WriteAllText(requestPath, "[]");
                return new List<Request>();
            }

            try
            {
                string json = File.ReadAllText(requestPath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Request>();
                }

                return JsonSerializer.Deserialize<List<Request>>(json) ?? new List<Request>();
            }

            catch (Exception ex)
            {
                // Fail-safe: prevent crash on bad file
                Console.WriteLine("Fejl ved indlæsning af forespørgsler.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();

                return new List<Request>();
            }
        }

        public void SaveRequests(List<Request> requests)
        {
            // Serialize with formatting for readability
            string json = JsonSerializer.Serialize(requests, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            try
            {
                // Ensure directory exists before writing
                EnsureDirectoryExists(requestPath);
                File.WriteAllText(requestPath, json);
            }

            catch (Exception ex)
            {
                // Fail-safe: prevent crash if write fails
                Console.WriteLine("Fejl ved gemning af forespørgsler.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();
            }
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
                // Ensure directory exists before writing
                EnsureDirectoryExists(filePath);
                File.WriteAllText(filePath, content);
            }

            catch (Exception ex)
            {
                // Fail-safe: prevent crash on export errors
                Console.WriteLine("Fejl ved eksport af fil.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                Console.WriteLine("\nTryk på en tast for at fortsætte...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        // Helper method to ensure directory exists before file operations
        private void EnsureDirectoryExists(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        }
    }
}