using Microsoft.EntityFrameworkCore;
using networkperformancemonitor.Entities;

namespace networkperformancemonitor.DAL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public ApplicationDbContext()
        {
        }
        public virtual DbSet<Tester> Testers { get; set; }
        public virtual DbSet<TestResult> TestResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=nl1-wsq2.my-hosting-panel.com;Initial Catalog=npmonitor;User Id=rikobgrff;Password=Riko1234!");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
