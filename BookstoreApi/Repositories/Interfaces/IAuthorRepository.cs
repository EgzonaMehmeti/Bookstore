using BookstoreApi.Models;

namespace BookstoreApi.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync(
        CancellationToken cancellationToken);

        Task<Author?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken);

        Task<Author?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken);

        Task AddAsync(
            Author author,
            CancellationToken cancellationToken);
    }
}
