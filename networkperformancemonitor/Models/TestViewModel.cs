namespace networkperformancemonitor.Models
{
    public class TestViewModel
    {
        public bool FullTest { get; set; }
        public string Url { get; set; }
        public bool NetworkUsage { get; set; }
        public bool ResponseTimeVariation { get; set; }
        public bool DNSResolutionTime { get; set; }
        public bool NumberOfHttpRequests { get; set; }
        public bool PacketLossRate { get; set; }
        public bool ServerProcessingTime { get; set; }
        public bool ConnectionTime { get; set; }
        public bool ContentDeliveryTime { get; set; }
        public bool DownloadSpeed { get; set; }
        public bool PageLoadTime { get; set; }
        public bool TimeToFirstByte { get; set; }
        public bool Latency { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string TesterIpAddress { get; set; }
    }
}
