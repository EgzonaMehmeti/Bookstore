using BookstoreApi.DTOs;
using BookstoreApi.Middleware;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApi.Controllers
{
    [ApiController]
    [Route("bookstore/v1/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            var authors = await _authorService
                .GetAllAsync(cancellationToken);

            return Ok(authors);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorResponse>> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            var author = await _authorService
                .GetByIdAsync(id, cancellationToken);

            if (author is null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorResponse>> Create(
            CreateAuthorRequest request,
            CancellationToken cancellationToken)
        {
            var author = await _authorService
            .CreateAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = author.AuthorId },
                author);
        }
    }
}
