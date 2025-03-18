using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string dsql = null)
        {
            if (string.IsNullOrWhiteSpace(dsql))
            {
                // ✅ If no DSQL is provided, return all books
                return Ok(await _bookService.GetAllBooksAsync());
            }

            try
            {
                // ✅ If DSQL is provided, apply the filter
                return Ok(await _bookService.GetBooksByDynamicQueryAsync(dsql));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn);
            if (book == null)
                return NotFound(new { message = "Book not found" });

            return Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            await _bookService.AddBookAsync(bookDto);
            return Ok(new { message = "Book added successfully" });
        }

        [HttpPut("{isbn}")]
        [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> UpdateBook(string isbn, [FromBody] BookDto bookDto)
        {
            await _bookService.UpdateBookAsync(bookDto);
            return Ok(new { message = "Book updated successfully" });
        }

        [HttpDelete("{isbn}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            await _bookService.DeleteBookAsync(isbn);
            return Ok(new { message = "Book deleted successfully" });
        }
    }
}
