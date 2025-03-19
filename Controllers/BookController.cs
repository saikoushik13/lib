using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IValidator<BookDto> _bookValidator;

        public BookController(IBookService bookService, IValidator<BookDto> bookValidator)
        {
            _bookService = bookService;
            _bookValidator = bookValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string dsql = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (books, totalCount) = string.IsNullOrWhiteSpace(dsql)
                    ? await _bookService.GetBooksPaginatedAsync(page, pageSize)
                    : await _bookService.GetBooksByDynamicQueryAsync(dsql, page, pageSize);

                return Ok(new { TotalCount = totalCount, Page = page, PageSize = pageSize, Books = books });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = "An internal error occurred while processing your request.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            try
            {
                var book = await _bookService.GetBookByISBNAsync(isbn);
                if (book == null)
                    return NotFound(new { message = "Book not found" });

                return Ok(book);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Author")]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            try
            {
                var validationResult = _bookValidator.Validate(bookDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
                }

                await _bookService.AddBookAsync(bookDto);
                return Ok(new { message = "Book added successfully" });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = "Validation failed.", details = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpDelete("{isbn}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            try
            {
                await _bookService.DeleteBookAsync(isbn);
                return Ok(new { message = "Book deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
    }
}
