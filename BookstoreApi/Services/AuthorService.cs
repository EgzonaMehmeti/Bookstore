using BookstoreApi.DTOs;
using BookstoreApi.Middleware;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Interfaces;

namespace BookstoreApi.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<AuthorResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var authors = await _authorRepository
                .GetAllAsync(cancellationToken);

            return authors
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<AuthorResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var author = await _authorRepository
                .GetByIdAsync(id, cancellationToken);

            return author is null
                ? null
                : MapToResponse(author);
        }

        public async Task<AuthorResponse> CreateAsync(
            CreateAuthorRequest request,
            CancellationToken cancellationToken)
        {
            var authorName = request.Name.Trim();

            var existingAuthor = await _authorRepository
                .GetByNameAsync(authorName, cancellationToken);

            if (existingAuthor is not null)
            {
                throw new ConflictException(
                    "An author with this name already exists.");
            }

            var author = new Author
            {
                Name = authorName
            };

            await _authorRepository.AddAsync(
                author,
                cancellationToken);

            return MapToResponse(author);
        }

        private static AuthorResponse MapToResponse(Author author)
        {
            return new AuthorResponse
            {
                AuthorId = author.AuthorId,
                Name = author.Name
            };
        }
    }
}
