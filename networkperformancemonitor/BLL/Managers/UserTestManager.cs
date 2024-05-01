using networkperformancemonitor.BLL.Interfaces;
using networkperformancemonitor.BLL.Services;
using networkperformancemonitor.DAL.Service;
using networkperformancemonitor.Entities;
using networkperformancemonitor.Models;
using Newtonsoft.Json.Linq;
using System;

namespace networkperformancemonitor.BLL.Managers
{
    public class UserTestManager
    {
        private readonly INetworkService service;
        private readonly TesterService testerService;
        private readonly IPServices ipServices;
        private const int loop = 1;
        public UserTestManager()
        {
            service = new NetworkManager();
            testerService = new TesterService(new DAL.Repository.TesterRepository());
            ipServices = new IPServices();
        }
        private async Task<Tester> CreateOrGetTester(string email, string fullaname, string ip)
        {
            var tester = (await testerService.GetAllAsync()).FirstOrDefault(x => x.Email == email);
            if (tester is null)
            {
                var newTester = new Tester()
                {
                    Email = email,
                    FullName = fullaname,
                    IpAddress = ip,
                };
                await testerService.InsertAsync(newTester);
                return newTester;
            }

            else
            {
                tester
                    .TestResults
                    .ForEach(x => x.IsLatestTest = false);
                await testerService.UpdateAsync(tester);
                return tester;
            }
        }
        private async Task CreateTestResultRecord(Tester tester, string testName, double testValue, string url, string valueType)
        {
            var testResult = new TestResult()
            {
                TestDateTime = DateTime.Now,
                TestName = testName,
                Url = url,
                UrlToIp = ipServices.GetIPAdressOfUrl(url),
                TestType = "Individual Test",
                IsLatestTest = true,
            };
            var value = Math.Round(testValue, 2);
            testResult.TestValue = $"{value} {valueType}";
            tester.TestResults.Add(testResult);
            await testerService.UpdateAsync(tester);
        }
        private async Task CreateTestResultRecord(Tester tester, string testName, double testValue, string url, bool isFullTest, string valueType)
        {
            var testResult = new TestResult()
            {
                TestDateTime = DateTime.Now,
                TestName = testName,
                Url = url,
                UrlToIp = ipServices.GetIPAdressOfUrl(url),
                IsLatestTest = true,
            };
            var value = Math.Round(testValue, 2);
            testResult.TestValue = $"{value} {valueType}";
            if (isFullTest)
            {
                testResult.TestType = "Full Test";
            }
            tester.TestResults.Add(testResult);
            await testerService.UpdateAsync(tester);
        }
        public async Task<int> SimulateTestOperations(TestViewModel model)
        {
            if (model is null)
            {
                return 0;
            }
            var testerId = 0;
            if (model.FullTest)
            {
                testerId = await RunFullTestAsync(model);
            }
            else
            {
                testerId = await RunIndividualTestsAsync(model);
            }

            return testerId;
        }
        private async Task<int> RunFullTestAsync(TestViewModel model)
        {
            var serviceType = service.GetType();
            var tester = await CreateOrGetTester(model.Email, model.FullName, model.TesterIpAddress);
            for (int i = 0; i < loop; i++)
            {
                foreach (var methodInfo in serviceType.GetMethods())
                {
                    if (methodInfo.ReturnType == typeof(Task<int>) || methodInfo.ReturnType == typeof(Task<double>))
                    {
                        var testName = methodInfo.Name.Replace("Get", "").Replace("Async", "").Trim();
                        var result = await (dynamic)methodInfo.Invoke(service, new object[] { model.Url });
                        string valueType = "";
                        if (testName == "ConnectionTime" || testName == "Latency" || testName == "ContentDeliveryTime" || testName == "DNSResolutionTime"|| testName == "PageLoadTime" || testName == "ResponseTimeVariation" || testName == "TimeToFirstByte" || testName == "ServerProcessingTime")
                        {
                            valueType = "ms";
                        }
                        if (testName == "DownloadSpeed" || testName == "NetworkUsage")
                        {
                            valueType = "bytes/second";
                        }
                        if(testName == "NumberOfHTTPRequests")
                        {
                            valueType = "requests";
                        }
                        if(testName == "PacketLossRate")
                        {
                            valueType = "%";
                        }
                        await CreateTestResultRecord(tester, testName, result, model.Url, true, valueType);
                    }
                }
            }
            return tester.Id;
        }
        private async Task<int> RunIndividualTestsAsync(TestViewModel model)
        {
            var tester = await CreateOrGetTester(model.Email, model.FullName, model.TesterIpAddress);

            if (model.ConnectionTime)
            {
                var res = await service.GetConnectionTime(model.Url);
                await CreateTestResultRecord(tester, "ConnectionTime", res, model.Url, "ms");
            }

            if (model.Latency)
            {
                var res = await service.GetLatencyAsync(model.Url);
                await CreateTestResultRecord(tester, "Latency", res, model.Url, "ms");
            }

            if (model.ContentDeliveryTime)
            {
                var res = await service.GetContentDeliveryTimeAsync(model.Url);
                await CreateTestResultRecord(tester, "ContentDeliveryTime", res, model.Url, "ms");
            }

            if (model.DNSResolutionTime)
            {
                var res = await service.GetDNSResolutionTimeAsync(model.Url);
                await CreateTestResultRecord(tester, "DNSResolutionTime", res, model.Url, "ms");
            }

            if (model.DownloadSpeed)
            {

                var res = await service.GetDownloadSpeedAsync(model.Url);
                await CreateTestResultRecord(tester, "DownloadSpeed", res, model.Url, "bytes/second");
            }


            if (model.PageLoadTime)
            {
                var res = await service.GetPageLoadTimeAsync(model.Url);
                await CreateTestResultRecord(tester, "PageLoadTime", res, model.Url, "ms");

            }
            if (model.ResponseTimeVariation)
            {
                var res = await service.GetResponseTimeVariationAsync(model.Url);
                await CreateTestResultRecord(tester, "ResponseTimeVariation", res, model.Url, "ms");

            }
            if (model.NetworkUsage)
            {
                var res = await service.GetNetworkUsageAsync(model.Url);
                await CreateTestResultRecord(tester, "NetworkUsage", res, model.Url, "bytes/seconds");

            }
            if (model.NumberOfHttpRequests)
            {
                var res = await service.GetNumberOfHTTPRequestsAsync(model.Url);
                await CreateTestResultRecord(tester, "NumberOfHTTPRequests", res, model.Url, "requests");

            }
            if (model.PacketLossRate)
            {
                var res = await service.GetPacketLossRateAsync(model.Url);
                await CreateTestResultRecord(tester, "PacketLossRate", res, model.Url, "%");

            }
            if (model.TimeToFirstByte)
            {
                var res = await service.GetTimeToFirstByteAsync(model.Url);
                await CreateTestResultRecord(tester, "TimeToFirstByte", res, model.Url, "ms");

            }

            if (model.ServerProcessingTime)
            {
                var res = await service.GetServerProcessingTimeAsync(model.Url);
                await CreateTestResultRecord(tester, "ServerProcessingTime", res, model.Url, "ms");
            }
            return tester.Id;
        }
    }
}