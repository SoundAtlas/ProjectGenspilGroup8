using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.Models
{
    public class Request
    {
        // Properties (needed for JSON)
        public string CustomerName { get; set; }
        public string GameName { get; set; }
        public string Status { get; set; }

        // Empty constructor (required for deserialization)
        public Request() { }

        // Constructor
        public Request(string customerName, string gameName, string status)
        {
            CustomerName = customerName;
            GameName = gameName;
            Status = status;
        }

        // Method
        public void SetStatus(string status)
        {
            Status = status;
        }
    }
}
