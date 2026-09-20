using BookstoreApi.DTOs;

namespace BookstoreApi.Services.Interfaces
{
    public interface IBookService
    {
        Task<List<BookResponse>> GetAllAsync(
        CancellationToken cancellationToken);

        Task<BookResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken);

        Task<PagedResult<BookResponse>> SearchAsync(
            string? title,
            string? author,
            int page,
            int pageSize,
            CancellationToken cancellationToken);

        Task<BookResponse?> CreateAsync(
            CreateBookRequest request,
            CancellationToken cancellationToken);

        Task<bool> UpdateAsync(
            int id,
            UpdateBookRequest request,
            CancellationToken cancellationToken);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken);
    }
}
