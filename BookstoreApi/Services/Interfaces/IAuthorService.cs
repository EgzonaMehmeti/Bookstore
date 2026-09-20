using BookstoreApi.DTOs;

namespace BookstoreApi.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorResponse>> GetAllAsync(
       CancellationToken cancellationToken);

        Task<AuthorResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken);

        Task<AuthorResponse> CreateAsync(
            CreateAuthorRequest request,
            CancellationToken cancellationToken);
    }
}
