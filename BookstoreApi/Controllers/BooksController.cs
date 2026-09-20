using BookstoreApi.DTOs;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApi.Controllers
{
    [ApiController]
    [Route("bookstore/v1/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [Authorize(Policy = "BooksPolicy")]
        [HttpGet]
        public async Task<ActionResult<List<BookResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllAsync(cancellationToken);

            return Ok(books);
        }

        [Authorize(Policy = "SearchPolicy")]
        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<BookResponse>>> Search(
            [FromQuery] string? title,
            [FromQuery] string? author,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (page < 1)
            {
                return BadRequest("Page must be greater than 0.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(
                    "Page size must be between 1 and 100.");
            }

            var result = await _bookService.SearchAsync(
                title,
                author,
                page,
                pageSize,
                cancellationToken);

            return Ok(result);
        }

        [Authorize(Policy = "BooksPolicy")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(
                id,
                cancellationToken);

            if (book is null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [Authorize(Policy = "BooksPolicy")]
        [HttpPost]
        public async Task<ActionResult<BookResponse>> Create(
            CreateBookRequest request,
            CancellationToken cancellationToken)
        {

            var book = await _bookService.CreateAsync(
                request,
                cancellationToken);

            if (book is null)
            {
                return BadRequest("The specified author does not exist.");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = book.BookId },
                book);
        }

        [Authorize(Policy = "BooksPolicy")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateBookRequest request,
            CancellationToken cancellationToken)
        {
            var updated = await _bookService.UpdateAsync(
            id,
            request,
            cancellationToken);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();

        }

        [Authorize(Policy = "BooksPolicy")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var deleted = await _bookService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
