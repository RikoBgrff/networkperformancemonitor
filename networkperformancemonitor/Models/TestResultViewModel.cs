using System.Diagnostics;

namespace networkperformancemonitor.Models
{
	public class TestResultViewModel
	{
		public TestResultViewModel()
		{
			Tests = new List<TestResultHelper>();
		}
		public string TesterEmail { get; set; }
		public string TesterFullName { get; set; }
		public string TesterIP { get; set; }
		public List<TestResultHelper> Tests { get; set; }
	}
}
