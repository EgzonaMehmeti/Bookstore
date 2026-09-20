using BookstoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApi.Data
{
    public class BookstoreDbContext : DbContext
    {
        public BookstoreDbContext(
        DbContextOptions<BookstoreDbContext> options)
        : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();

        public DbSet<Author> Authors => Set<Author>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(BookstoreDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
