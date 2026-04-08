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
        private const string GamesFile = "games.json";
        private const string RequestsFile = "requests.json";

        public List<Game> LoadGames()
        {
            if (!File.Exists(GamesFile))
            {
                return new List<Game>();
            }
            string json = File.ReadAllText(GamesFile);
            return JsonSerializer.Deserialize<List<Game>>(json) ?? new List<Game>();
        }

        public void SaveGames(List<Game> games)
        {
            string json = JsonSerializer.Serialize(games, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            File.WriteAllText(GamesFile, json);
        }

        public List<Request> LoadRequests()
        {
            if (!File.Exists(RequestsFile))
            {
                return new List<Request>();
            }
            string json = File.ReadAllText(RequestsFile);
            return JsonSerializer.Deserialize<List<Request>>(json) ?? new List<Request>();
        }

        public void SaveRequests(List<Request> requests)
        {
            string json = JsonSerializer.Serialize(requests, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(RequestsFile, json);
        }
    }
}
