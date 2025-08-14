namespace NetCoreAI.Project01_ApiDemo.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }

        Task<int> SaveChangesAsync();
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}
