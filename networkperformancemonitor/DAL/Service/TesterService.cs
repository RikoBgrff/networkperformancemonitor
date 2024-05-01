using networkperformancemonitor.DAL.Interface;
using networkperformancemonitor.DAL.Repository;
using networkperformancemonitor.Entities;

namespace networkperformancemonitor.DAL.Service
{
    public class TesterService
    {
        private readonly IRepository<Tester> repository;
        public TesterService(TesterRepository repository)
        {
            this.repository = repository;
        }
        public async Task InsertAsync(Tester Tester)
        {
            await repository.AddAsync(Tester);
        }
        public async Task UpdateAsync(Tester Tester)
        {
            await repository.UpdateAsync(Tester);
        }
        public async Task RemoveAsync(Tester Tester)
        {
            await repository.DeleteAsync(Tester);
        }
        public async Task<List<Tester>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }
        public async Task<Tester> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }
    }
}