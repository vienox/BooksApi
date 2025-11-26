using LibraryApi.Data;
using LibraryApi.Dtos.Authors;
using LibraryApi.Dtos.Books;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryContext _context;

    public AuthorsController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        var authors = await _context.Authors
            .AsNoTracking()
            .Include(a => a.Books)
            .ToListAsync();

        return authors.Select(MapAuthorDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
    {
        var author = await _context.Authors
            .AsNoTracking()
            .Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author is null)
            return NotFound();

        return MapAuthorDto(author);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor([FromBody] AuthorCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var author = new Author
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Biography = dto.Biography
        };

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        await _context.Entry(author).Collection(a => a.Books).LoadAsync();

        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, MapAuthorDto(author));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuthorDto>> UpdateAuthor(int id, [FromBody] AuthorCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
        if (author is null)
            return NotFound();

        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        author.DateOfBirth = dto.DateOfBirth;
        author.Biography = dto.Biography;

        await _context.SaveChangesAsync();

        return Ok(MapAuthorDto(author));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author is null)
            return NotFound();

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static AuthorDto MapAuthorDto(Author author) =>
        new(
            author.Id,
            author.FirstName,
            author.LastName,
            author.DateOfBirth,
            author.Biography,
            author.Books.Select(b => new BookSummaryDto(b.Id, b.Title)).ToList());
}
