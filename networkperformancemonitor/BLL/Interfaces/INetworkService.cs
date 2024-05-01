using networkperformancemonitor.Entities;

namespace networkperformancemonitor.BLL.Interfaces
{
    public interface INetworkService
    {
        public Task<double> GetLatencyAsync(string url);
        public Task<double> GetNetworkUsageAsync(string url);
        public Task<double> GetPacketLossRateAsync(string url);
        public Task<double> GetDownloadSpeedAsync(string url);
        public Task<double> GetResponseTimeVariationAsync(string url);
        public Task<double> GetServerProcessingTimeAsync(string url);
        public Task<double> GetPageLoadTimeAsync(string url);
        public Task<double> GetDNSResolutionTimeAsync(string url);
        public Task<double> GetConnectionTime(string url);
        public Task<double> GetTimeToFirstByteAsync(string url);
        public Task<int> GetNumberOfHTTPRequestsAsync(string url);
        public Task<double> GetContentDeliveryTimeAsync(string url);
    }
}
