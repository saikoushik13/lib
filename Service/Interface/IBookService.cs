using Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllBooksAsync();
        Task<List<BookDto>> GetBooksByDynamicQueryAsync(string dsql);
        Task<BookDto> GetBookByISBNAsync(string isbn);
        Task AddBookAsync(BookDto bookDto);
        Task UpdateBookAsync(BookDto bookDto);
        Task DeleteBookAsync(string isbn);
    }
}
