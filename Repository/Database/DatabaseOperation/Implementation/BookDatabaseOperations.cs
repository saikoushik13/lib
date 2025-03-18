using Database;
using Database.Entities;
using DatabaseOperations.Interface;
using Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseOperations.Implementation
{
    public class BookDatabaseOperations : IBookDatabaseOperations
    {
        private readonly AppDbContext _context;

        public BookDatabaseOperations(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.Include(b => b.Genres).ToListAsync();
        }

        public async Task<Book> GetByISBNAsync(string isbn)
        {
            return await _context.Books
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<List<Book>> GetBooksByDynamicQueryAsync(string dsql)
        {
            IQueryable<Book> query = _context.Books.Include(b => b.Genres);

            if (!string.IsNullOrWhiteSpace(dsql))
            {
                string dynamicQuery = DsqlDynamicQueryTransformer.Transform(dsql);

                try
                {
                    query = query.Where(dynamicQuery);
                }
                catch
                {
                    throw new Exception("Invalid query syntax.");
                }
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(Book book)
        {
            // ✅ Attach existing genres to avoid duplicates
            var genreIds = book.Genres.Select(g => g.Id).ToList();
            var existingGenres = await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
            book.Genres = existingGenres;

            await _context.Books.AddAsync(book);
        }

        public async Task UpdateAsync(Book book)
        {
            var existingBook = await _context.Books
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.ISBN == book.ISBN);

            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Description = book.Description;
                existingBook.Author = book.Author;
                existingBook.Publisher = book.Publisher;
                existingBook.PublicationYear = book.PublicationYear;

                // ✅ Update genres correctly
                var genreIds = book.Genres.Select(g => g.Id).ToList();
                var existingGenres = await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
                existingBook.Genres.Clear();
                existingBook.Genres = existingGenres;

                _context.Books.Update(existingBook);
            }
        }

        public async Task DeleteAsync(string isbn)
        {
            var book = await _context.Books.Include(b => b.Genres).FirstOrDefaultAsync(b => b.ISBN == isbn);
            if (book != null)
            {
                book.Genres.Clear(); // ✅ Remove genre relationships first
                _context.Books.Remove(book);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
