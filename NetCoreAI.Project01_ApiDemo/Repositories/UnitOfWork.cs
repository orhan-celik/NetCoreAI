using NetCoreAI.Project01_ApiDemo.Context;
using NetCoreAI.Project01_ApiDemo.Entities;
using System.Runtime.InteropServices.Marshalling;

namespace NetCoreAI.Project01_ApiDemo.Repositories
{

    public class UnitOfWork : IUnitOfWork
    {

        protected readonly AppDbContext _context;

        private ICustomerRepository _customers;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customers
            => _customers ??= new CustomerRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
