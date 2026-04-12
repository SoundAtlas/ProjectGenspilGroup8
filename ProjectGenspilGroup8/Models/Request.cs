namespace ProjectGenspilGroup8.Models
{
    public class Request
    {
        // Allowed status values
        public const string StatusPending = "Under behandling";
        public const string StatusCompleted = "Fuldført";
        public const string StatusCancelled = "Annulleret";

        // Auto-properties
        public string CustomerName { get; set; }
        public string GameName { get; set; }
        public string Status { get; set; }

        // Constructors
        public Request(string customerName, string gameName, string status)
        {
            CustomerName = customerName;
            GameName = gameName;
            Status = status;
        }

        // Parameterless constructor needed for JSON deserialization
        public Request()
        {
            CustomerName = string.Empty;
            GameName = string.Empty;
            Status = StatusPending;
        }
    }
}
