using BookstoreApi.Data;
using BookstoreApi.Models;
using BookstoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookstoreDbContext _context;

        public AuthorRepository(BookstoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return await _context.Authors
                .ToListAsync(cancellationToken);
        }

        public async Task<Author?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(
                    a => a.AuthorId == id,
                    cancellationToken);
        }

        public async Task<Author?> GetByNameAsync(
            string name,
            CancellationToken cancellationToken)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(
                    a => a.Name == name,
                    cancellationToken);
        }

        public async Task AddAsync(
            Author author,
            CancellationToken cancellationToken)
        {
            _context.Authors.Add(author);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
