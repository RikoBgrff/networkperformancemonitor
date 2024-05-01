namespace networkperformancemonitor.Entities
{
    public class TestResult
    {
        public TestResult()
        {
            Tester = new Tester();
        }

        public int Id { get; set; }
        public int TesterId { get; set; }
        public string TestName { get; set; }
        public string TestValue { get; set; }
        public DateTime TestDateTime { get; set; }
        public Tester Tester { get; set; }
        public string Url { get; set; }
        public string UrlToIp { get; set; }
        public string TestType { get; set; }
        public bool IsLatestTest { get; set; }
    }
}
