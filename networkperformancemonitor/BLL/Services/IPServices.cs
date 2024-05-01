using System.Net;

namespace networkperformancemonitor.BLL.Services
{
    public class IPServices
    {
        private readonly HttpClient client;
        public IPServices()
        {
            client = new HttpClient();
        }
        public string GetIPAdressOfUrl(string url)
        {
            List<IPAddress> ipAddresses = Dns.GetHostAddresses(url).ToList();
            return ipAddresses.FirstOrDefault().ToString();
        }
    }
}
