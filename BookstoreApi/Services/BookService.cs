using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Middleware;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookstoreDbContext _context;

        public BookService(
            IBookRepository bookRepository,
            BookstoreDbContext context)
        {
            _bookRepository = bookRepository;
            _context = context;
        }

        public async Task<List<BookResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var books = await _bookRepository
                .GetAllAsync(cancellationToken);

            return books
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<BookResponse?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var book = await _bookRepository
                .GetByIdAsync(id, cancellationToken);

            if (book is null)
            {
                return null;
            }

            return MapToResponse(book);
        }

        public async Task<PagedResult<BookResponse>> SearchAsync(
            string? title,
            string? author,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var (books, totalCount) = await _bookRepository.SearchAsync(
                title,
                author,
                page,
                pageSize,
                cancellationToken);

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            return new PagedResult<BookResponse>
            {
                Items = books
                    .Select(MapToResponse)
                    .ToList(),

                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public async Task<BookResponse?> CreateAsync(
            CreateBookRequest request,
            CancellationToken cancellationToken)
        {
            var authorExists = await _context.Authors
                .AnyAsync(
                    a => a.AuthorId == request.AuthorId,
                    cancellationToken);

            if (!authorExists)
            {
                return null;
            }

            var title = request.Title.Trim();

            var bookExists = await _bookRepository.ExistsAsync(
                request.AuthorId,
                title,
                null,
                cancellationToken);

            if (bookExists)
            {
                throw new ConflictException(
                    "This author already has a book with this title.");
            }

            var book = new Book
            {
                AuthorId = request.AuthorId,
                Title = title,
                SubTitle = request.SubTitle?.Trim()
            };

            await _bookRepository.AddAsync(
                book,
                cancellationToken);

            var createdBook = await _bookRepository
                .GetByIdAsync(
                    book.BookId,
                    cancellationToken);

            return createdBook is null
                ? null
                : MapToResponse(createdBook);
        }

        public async Task<bool> UpdateAsync(
            int id,
            UpdateBookRequest request,
            CancellationToken cancellationToken)
        {
            var book = await _bookRepository
                .GetByIdAsync(id, cancellationToken);

            if (book is null)
            {
                return false;
            }

            var authorExists = await _context.Authors
                .AnyAsync(
                    a => a.AuthorId == request.AuthorId,
                    cancellationToken);

            if (!authorExists)
            {
                return false;
            }

            var title = request.Title.Trim();

            var bookExists = await _bookRepository.ExistsAsync(
                request.AuthorId,
                title,
                id,
                cancellationToken);

            if (bookExists)
            {
                throw new ConflictException(
                    "This author already has a book with this title.");
            }

            book.AuthorId = request.AuthorId;
            book.Title = title;
            book.SubTitle = request.SubTitle?.Trim();

            await _bookRepository.UpdateAsync(
                book,
                cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var book = await _bookRepository
                .GetByIdAsync(id, cancellationToken);

            if (book is null)
            {
                return false;
            }

            await _bookRepository.DeleteAsync(
                book,
                cancellationToken);

            return true;
        }

        private static BookResponse MapToResponse(Book book)
        {
            return new BookResponse
            {
                BookId = book.BookId,
                Author = new AuthorResponse
                {
                    AuthorId = book.Author.AuthorId,
                    Name = book.Author.Name
                },
                Title = book.Title,
                SubTitle = book.SubTitle
            };
        }
    }
}
