using AutoMapper;
using Database.Entities;
using DatabaseOperations.Interface;
using Models.DTO;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class BookService : IBookService
    {
        private readonly IBookDatabaseOperations _bookDbOperations;
        private readonly IGenreDatabaseOperations _genreDbOperations;
        private readonly IMapper _mapper;

        public BookService(IBookDatabaseOperations bookDbOperations, IGenreDatabaseOperations genreDbOperations, IMapper mapper)
        {
            _bookDbOperations = bookDbOperations;
            _genreDbOperations = genreDbOperations;
            _mapper = mapper;
        }

        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookDbOperations.GetAllAsync();
            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<List<BookDto>> GetBooksByDynamicQueryAsync(string dsql)
        {
            var books = await _bookDbOperations.GetBooksByDynamicQueryAsync(dsql);
            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<BookDto> GetBookByISBNAsync(string isbn)
        {
            var book = await _bookDbOperations.GetByISBNAsync(isbn);
            return _mapper.Map<BookDto>(book);
        }

        public async Task AddBookAsync(BookDto bookDto)
        {
            // ✅ Validate that genres exist before adding the book
            var genres = await _genreDbOperations.GetGenresByIdsAsync(bookDto.GenreIds);
            if (genres.Count != bookDto.GenreIds.Count)
                throw new Exception("One or more genres are invalid.");

            var book = _mapper.Map<Book>(bookDto);
            book.Genres = genres; // ✅ Assign valid genres

            await _bookDbOperations.AddAsync(book);
            await _bookDbOperations.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(BookDto bookDto)
        {
            var existingBook = await _bookDbOperations.GetByISBNAsync(bookDto.ISBN);
            if (existingBook == null)
                throw new Exception("Book not found.");

            existingBook.Title = bookDto.Title;
            existingBook.Author = bookDto.Author;
            existingBook.Description = bookDto.Description;
            existingBook.Publisher = bookDto.Publisher;
            existingBook.PublicationYear = bookDto.PublicationYear;

            // ✅ Update genres
            var genres = await _genreDbOperations.GetGenresByIdsAsync(bookDto.GenreIds);
            existingBook.Genres.Clear();
            existingBook.Genres = genres;

            await _bookDbOperations.UpdateAsync(existingBook);
            await _bookDbOperations.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(string isbn)
        {
            await _bookDbOperations.DeleteAsync(isbn);
            await _bookDbOperations.SaveChangesAsync();
        }
    }
}
