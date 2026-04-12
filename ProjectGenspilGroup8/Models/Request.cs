using System;
using System.Text.Json.Serialization;

namespace ProjectGenspilGroup8.Models
{
    public class Request
    {
        // Private backing fields (single source of truth)
        private string _customerName;
        private string _gameName;
        private string _status;

        // Read-only properties for JSON + safe access
        public string CustomerName => _customerName;
        public string GameName => _gameName;
        public string Status => _status;

        // Empty constructor (required for deserialization)
        public Request()
        {
            _customerName = "";
            _gameName = "";
            _status = "";
        }

        // Constructor for creating new requests
        public Request(string customerName, string gameName, string status)
        {
            _customerName = customerName?.Trim() ?? "";
            _gameName = gameName?.Trim() ?? "";
            _status = status?.Trim() ?? "";
        }

        // Method for updating status (controlled mutation)
        public void SetStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status)) return; // Prevent invalid state
            _status = status.Trim();
        }
    }
}