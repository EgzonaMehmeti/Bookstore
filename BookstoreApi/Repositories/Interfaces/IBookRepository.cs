using BookstoreApi.Models;

namespace BookstoreApi.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(CancellationToken cancellationToken);

        Task<(List<Book> Items, int TotalCount)> SearchAsync(
            string? title,
            string? author,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        Task<Book?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken);

        Task AddAsync(
            Book book,
            CancellationToken cancellationToken);

        Task<bool> ExistsAsync(
            int authorId,
            string title,
            int? excludeBookId,
            CancellationToken cancellationToken);

        Task UpdateAsync(
            Book book,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Book book,
            CancellationToken cancellationToken);
    }
}
