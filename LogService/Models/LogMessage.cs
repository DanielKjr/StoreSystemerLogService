namespace LogService.Models
{
    public class LogMessage
    {
        public string Username { get; set; }

        public string LootBoxName { get; set; }

        public string Version { get; set; }

        public int Quantity { get; set; }

        public DateTime Created { get; set; }
    }
}
