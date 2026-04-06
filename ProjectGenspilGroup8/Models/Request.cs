using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectGenspilGroup8.Models
{
    public class Request
    {
        private string _customerName;
        private string _gameName;
        private string _status;

        // Constructor
        public Request(string customerName, string gameName, string status)
        {
            _customerName = customerName;
            _gameName = gameName;
            _status = status;
        }

        // Methods
        public string GetCustomerName() => _customerName;
        public string GetGameName() => _gameName;
        public string GetStatus() => _status;

        public void SetStatus(string status) => _status = status;
    }
}
