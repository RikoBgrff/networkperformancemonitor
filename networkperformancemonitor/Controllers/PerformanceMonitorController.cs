using Microsoft.AspNetCore.Mvc;
using networkperformancemonitor.BLL.Managers;
using networkperformancemonitor.BLL.Services;
using networkperformancemonitor.Models;
using System.Net;
using System.Security.Policy;

namespace networkperformancemonitor.Controllers
{
    public class PerformanceMonitorController : Controller
    {
        private readonly ILogger<PerformanceMonitorController> _logger;
        private readonly UserTestManager manager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TestResultManager testResultManager;
        public PerformanceMonitorController(ILogger<PerformanceMonitorController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            manager = new UserTestManager();
            _httpContextAccessor = httpContextAccessor;
            testResultManager = new TestResultManager();
        }

        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var model = await testResultManager.GetAllRecords(ip);
                return View(model);

            }
            catch (Exception ex)
            {
                var model = await testResultManager.GetAllRecords("");
                return View(model);
            }
        }
        public IActionResult Test()
        {
            var model = new TestViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Test(TestViewModel model)
        {
            try
            {
                model.Url = model
                    .Url.Replace("http://", "")
                    .Replace("https://", "")
                    .Replace("www.", "");
                model.TesterIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                var id = await manager.SimulateTestOperations(model);
                if (id is not 0)
                {
                    return Redirect($"TestResults/{id}");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Info = "Url is broken";
                ViewBag.State = 0;
                model.Url = String.Empty;
                Console.WriteLine(ex.Message);
                return View(model);
            }
        }
        public async Task<IActionResult> TestResults(int id)
        {
            var model = await testResultManager.GetLatestResults(id);
            if (model is not null)
            {
                return View(model);
            }
            else return View();
        }



    }
}
