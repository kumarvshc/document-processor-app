namespace DocumentProcessor.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdWithScanResultsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);       
    }
}
