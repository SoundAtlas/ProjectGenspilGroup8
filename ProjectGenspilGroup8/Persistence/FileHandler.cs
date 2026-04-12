using System;
using System.Collections.Generic;
using System.Text;
using ProjectGenspilGroup8.Services;
using ProjectGenspilGroup8.Models;
using System.Text.Json;

namespace ProjectGenspilGroup8.Persistence
{
    internal class FileHandler
    {
        string gamePath = "..\\..\\..\\Data\\games.json";
        string requestPath = "..\\..\\..\\Data\\requests.json";

        public List<Game> LoadGames()
        {
            if (!File.Exists(gamePath))
            {
                File.WriteAllText(gamePath, "[]");
                return new List<Game>();
            }

            try
            {
                string json = File.ReadAllText(gamePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<Game>();
                }

                return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved indlæsning af spildata.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                return new List<Game>();
            }
        }

        public void SaveGames(List<Game> games)
        {
            string json = JsonSerializer.Serialize(games, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            try
            {
                File.WriteAllText(gamePath, json);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved gemning af spildata.");
                Console.WriteLine($"Detaljer: {ex.Message}");
            }
        }

        public List<Request> LoadRequests()
        {
            if (!File.Exists(requestPath))
            {
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
                Console.WriteLine("Fejl ved indlæsning af forespørgsler.");
                Console.WriteLine($"Detaljer: {ex.Message}");

                return new List<Request>();
            }
        }

        public void SaveRequests(List<Request> requests)
        {
            string json = JsonSerializer.Serialize(requests, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            try
            {
                File.WriteAllText(requestPath, json);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved gemning af forespørgsler.");
                Console.WriteLine($"Detaljer: {ex.Message}");
            }
        }

        public void ExportToFile(string content, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, content);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved eksport af fil.");
                Console.WriteLine($"Detaljer: {ex.Message}");
            }
        }
    }
}