using Microsoft.EntityFrameworkCore;
using networkperformancemonitor.DAL.Context;
using networkperformancemonitor.DAL.Interface;
using networkperformancemonitor.Entities;

namespace networkperformancemonitor.DAL.Repository
{
    public class TesterRepository : IRepository<Tester>
    {
        private readonly ApplicationDbContext context;
        public TesterRepository()
        {
            context = new ApplicationDbContext();
        }
        public async Task AddAsync(Tester entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tester entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<Tester>> GetAllAsync()
        {
            var list = await context.Testers
                .Include(x=>x.TestResults)
                .ToListAsync();
            if (list is not null)
            {
                return list;
            }
            else return new List<Tester>();
        }

        public async Task<Tester> GetByIdAsync(int id)
        {
            var ip = await context.Testers
                .Include(x=>x.TestResults)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (ip is not null)
            {
                return ip;
            }
            else return new Tester();
        }

        public async Task UpdateAsync(Tester entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}