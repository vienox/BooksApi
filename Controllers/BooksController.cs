using LibraryApi.Data;
using LibraryApi.Dtos.Authors;
using LibraryApi.Dtos.Books;
using LibraryApi.Dtos.Copies;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] int? authorId)
    {
        var query = _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Copies)
            .AsQueryable();

        if (authorId.HasValue)
            query = query.Where(b => b.AuthorId == authorId.Value);

        var books = await query.ToListAsync();

        return books.Select(MapBookDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        var book = await _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Copies)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null)
            return NotFound();

        return MapBookDto(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookCreateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var authorExists = await _context.Authors.AnyAsync(a => a.Id == dto.AuthorId);
        if (!authorExists)
            return BadRequest($"Author {dto.AuthorId} not found.");

        var book = new Book
        {
            Title = dto.Title,
            Description = dto.Description,
            PublishedYear = dto.PublishedYear,
            Isbn = dto.Isbn,
            AuthorId = dto.AuthorId
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        await _context.Entry(book).Reference(b => b.Author).LoadAsync();
        await _context.Entry(book).Collection(b => b.Copies).LoadAsync();

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, MapBookDto(book));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] BookUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var book = await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Copies)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null)
            return NotFound();

        var authorExists = await _context.Authors.AnyAsync(a => a.Id == dto.AuthorId);
        if (!authorExists)
            return BadRequest($"Author {dto.AuthorId} not found.");

        book.Title = dto.Title;
        book.Description = dto.Description;
        book.PublishedYear = dto.PublishedYear;
        book.Isbn = dto.Isbn;
        book.AuthorId = dto.AuthorId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null)
            return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static BookDto MapBookDto(Book book) =>
        new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            PublishedYear = book.PublishedYear,
            Isbn = book.Isbn,
            Author = new AuthorSimpleDto
            {
                Id = book.AuthorId,
                FirstName = book.Author.FirstName,
                LastName = book.Author.LastName
            },
            Copies = book.Copies.Select(c => new BookCopyDto
            {
                Id = c.Id,
                InventoryNumber = c.InventoryNumber,
                Condition = c.Condition,
                IsAvailable = c.IsAvailable,
                Book = new BookSummaryDto
                {
                    Id = book.Id,
                    Title = book.Title
                }
            }).ToList()
        };
}
