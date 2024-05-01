using Microsoft.AspNetCore.Mvc.Razor;
using networkperformancemonitor.DAL.Service;
using networkperformancemonitor.Entities;
using networkperformancemonitor.Models;

namespace networkperformancemonitor.BLL.Managers
{
    public class TestResultManager
    {
        private readonly TesterService testerService;
        public TestResultManager()
        {
            testerService = new TesterService(new DAL.Repository.TesterRepository());
        }
        public async Task<TestResultViewModel> GetLatestResults(int testerId)
        {
            var tester = await testerService
                .GetByIdAsync(testerId);
            if (tester is not null)
            {
                var model = new TestResultViewModel()
                {
                    TesterEmail = tester.Email,
                    TesterFullName = tester.FullName,
                    TesterIP = tester.IpAddress,
                };
                var list = new List<TestResultHelper>();
                foreach (var item in tester.TestResults.Where(x => x.IsLatestTest))
                {
                    list.Add(new TestResultHelper()
                    {
                        ResourceIP = item.UrlToIp,
                        ResourceUrl = item.Url,
                        TestDate = item.TestDateTime,
                        TestName = item.TestName,
                        TestType = item.TestType,
                        TestValue = item.TestValue,
                    });
                }
                model.Tests = list;
                return model;
            }
            else return new TestResultViewModel();
        }

        public async Task<DashboardViewModel> GetAllRecords(string currentUserIp)
        {
            var model = new DashboardViewModel();
            model.Testers = await testerService.GetAllAsync();
            if (!string.IsNullOrEmpty(currentUserIp))
            {
                model.CurrentUserIP = currentUserIp;
                var tester = (await testerService.GetAllAsync()).FirstOrDefault(x => x.IpAddress == currentUserIp);
                model.CurrentUserFullname = tester.FullName;
                model.CurrentUserEmail = tester.Email;
            }
            return model;
        }
    }
}
