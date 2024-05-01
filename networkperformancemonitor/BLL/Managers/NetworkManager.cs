using Humanizer;
using networkperformancemonitor.BLL.Interfaces;
using networkperformancemonitor.BLL.Services;
using networkperformancemonitor.Entities;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace networkperformancemonitor.BLL.Managers
{
    public class NetworkManager : INetworkService
    {
        private readonly IPServices ipServices;
        public NetworkManager()
        {
            ipServices = new IPServices();
        }
         public async Task<double> GetNetworkUsageAsync(string requestUri)
        {
            requestUri = "https://www." + requestUri;
            using (var httpClient = new HttpClient())
            {
                var stopwatch = Stopwatch.StartNew();
                var response = await httpClient.GetAsync(requestUri);
                stopwatch.Stop();

                if (response.IsSuccessStatusCode)
                {
                    long responseSize = response.Content.Headers.ContentLength ?? 0;
                    double elapsedTime = stopwatch.ElapsedMilliseconds / 1000.0;

                    double networkUsage = (responseSize / elapsedTime);
                    return networkUsage;
                }
                else
                {
                    return -1;
                }
            }
        }
        public async Task<double> GetPacketLossRateAsync(string host)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var pingOptions = new PingOptions
                    {
                        DontFragment = true
                    };

                    const int timeout = 1000;
                    const int bufferSize = 32;

                    var reply = await ping.SendPingAsync(host, timeout, new byte[bufferSize], pingOptions);
                    if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        return reply.Status == IPStatus.Success ? 0 : 100;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<double> GetDownloadSpeedAsync(string url)
        {
            url = "https://www." + url;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var stopwatch = Stopwatch.StartNew();
                    var response = await httpClient.GetAsync(url);
                    stopwatch.Stop();

                    if (response.IsSuccessStatusCode)
                    {
                        long fileSize = response.Content.Headers.ContentLength ?? 0;
                        double elapsedTime = stopwatch.ElapsedMilliseconds / 1000.0;
                        double downloadSpeed = (fileSize / elapsedTime);
                        return downloadSpeed;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<double> GetResponseTimeVariationAsync(string url)
        {
            var numberOfRequests = 10;
            url = "https://www." + url;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    List<long> responseTimes = new List<long>();
                    for (int i = 0; i < numberOfRequests; i++)
                    {
                        var stopwatch = Stopwatch.StartNew();
                        var response = await httpClient.GetAsync(url);
                        stopwatch.Stop();

                        if (response.IsSuccessStatusCode)
                        {
                            responseTimes.Add(stopwatch.ElapsedMilliseconds);
                        }
                        else
                        {
                            numberOfRequests++;
                        }
                    }
                    double mean = responseTimes.Average();
                    double sumOfSquaresOfDifferences = responseTimes.Select(val => (val - mean) * (val - mean)).Sum();
                    double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / numberOfRequests);
                    return standardDeviation;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<double> GetServerProcessingTimeAsync(string url)
        {
            url = "https://www." + url;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var stopwatch = Stopwatch.StartNew();
                    var response = await httpClient.GetAsync(url);
                    stopwatch.Stop();

                    if (response.IsSuccessStatusCode)
                    {
                        double processingTime = stopwatch.ElapsedMilliseconds;
                        return processingTime;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<double> GetPageLoadTimeAsync(string url)
        {
            url = "https://www." + url;
            Stopwatch stopwatch = Stopwatch.StartNew();
            using (var webClient = new WebClient())
            {
                await webClient.DownloadStringTaskAsync(url);
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        public async Task<double> GetDNSResolutionTimeAsync(string url)
        {

            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                var ipAddresses = await Dns.GetHostAddressesAsync(url);
            }
            catch (Exception ex)
            {
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        public async Task<double> GetConnectionTime(string url)
        {
            url = "https://www." + url;
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                using (var client = new WebClient())
                {
                    await client.DownloadDataTaskAsync(url);
                }
            }
            catch (Exception ex)
            {
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        public async Task<double> GetTimeToFirstByteAsync(string url)
        {
            url = "https://www." + url;
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                using (var client = new WebClient())
                {
                    await client.DownloadDataTaskAsync(url);
                }
            }
            catch (Exception ex)
            {
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        public async Task<int> GetNumberOfHTTPRequestsAsync(string url)
        {
            url = "https://www." + url;
            try
            {
                using (WebClient client = new WebClient())
                {
                    string html = await client.DownloadStringTaskAsync(url);
                    int count = Regex.Matches(html, @"<img\b|<script\b|<link\b").Count;
                    return count;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<double> GetContentDeliveryTimeAsync(string url)
        {
            url = "https://www." + url;
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                using (var webClient = new WebClient())
                {
                    await webClient.DownloadStringTaskAsync(url);
                }
            }
            catch (Exception ex)
            {
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
        public async Task<double> GetLatencyAsync(string url)
        {
            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = await ping.SendPingAsync(url);
                    if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                    {
                        return reply.RoundtripTime;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
