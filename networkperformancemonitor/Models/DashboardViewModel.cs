using networkperformancemonitor.Entities;

namespace networkperformancemonitor.Models
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Testers = new List<Tester>();
        }
        public string CurrentUserIP { get; set; }
        public string CurrentUserEmail { get; set; }
        public string CurrentUserFullname { get; set; }
        public List<Tester> Testers { get; set; }
    }
}
