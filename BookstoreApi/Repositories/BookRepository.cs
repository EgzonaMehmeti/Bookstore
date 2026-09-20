using BookstoreApi.Data;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookstoreDbContext _context;

        public BookRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Books
                .Include(b => b.Author)
                .ToListAsync(cancellationToken);
        }

        public async Task<Book?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(
                    b => b.BookId == id,
                    cancellationToken);
        }

        public async Task<(List<Book> Items, int TotalCount)> SearchAsync(
            string? title,
            string? author,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(b =>
                    EF.Functions.Like(b.Title, $"%{title.Trim()}%"));
            }

            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(b =>
                    EF.Functions.Like(
                        b.Author.Name,
                        $"%{author.Trim()}%"));
            }

            var totalCount = await query.CountAsync(
                cancellationToken);

            var items = await query
                .OrderBy(b => b.BookId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task AddAsync(
            Book book,
            CancellationToken cancellationToken)
        {
            _context.Books.Add(book);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            int authorId,
            string title,
            int? excludeBookId,
            CancellationToken cancellationToken)
        {
            return await _context.Books
                .AnyAsync(
                    b =>
                        b.AuthorId == authorId &&
                        b.Title == title &&
                        (!excludeBookId.HasValue ||
                         b.BookId != excludeBookId.Value),
                    cancellationToken);
        }

        public async Task UpdateAsync(
            Book book,
            CancellationToken cancellationToken)
        {
            _context.Books.Update(book);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(
            Book book,
            CancellationToken cancellationToken)
        {
            _context.Books.Remove(book);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
