namespace networkperformancemonitor.Entities
{
    public class Tester
    {
        public Tester()
        {
            TestResults = new List<TestResult>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string IpAddress { get; set; }
        public List<TestResult> TestResults { get; set; }
    }
}
