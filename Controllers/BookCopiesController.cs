using LibraryApi.Data;
using LibraryApi.Dtos.Books;
using LibraryApi.Dtos.Copies;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BookCopiesController : ControllerBase
{
    private readonly LibraryContext _context;

    public BookCopiesController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookCopyDto>>> GetCopies()
    {
        var copies = await _context.BookCopies
            .AsNoTracking()
            .Include(c => c.Book)
            .ToListAsync();

        return copies.Select(MapCopyDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookCopyDto>> GetCopy(int id)
    {
        var copy = await _context.BookCopies
            .AsNoTracking()
            .Include(c => c.Book)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (copy is null)
            return NotFound();

        return MapCopyDto(copy);
    }

    [HttpPost]
    public async Task<ActionResult<BookCopyDto>> CreateCopy([FromBody] BookCopyCreateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var book = await _context.Books.FindAsync(dto.BookId);
        if (book is null)
            return NotFound($"Book {dto.BookId} not found.");

        var copy = new BookCopy
        {
            BookId = dto.BookId,
            Condition = dto.Condition,
            InventoryNumber = dto.InventoryNumber,
            IsAvailable = dto.IsAvailable
        };

        _context.BookCopies.Add(copy);
        await _context.SaveChangesAsync();
        await _context.Entry(copy).Reference(c => c.Book).LoadAsync();

        return CreatedAtAction(nameof(GetCopy), new { id = copy.Id }, MapCopyDto(copy));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookCopyDto>> UpdateCopy(int id, [FromBody] BookCopyUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var copy = await _context.BookCopies
            .Include(c => c.Book)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (copy is null)
            return NotFound();

        var bookExists = await _context.Books.AnyAsync(b => b.Id == dto.BookId);
        if (!bookExists)
            return NotFound($"Book {dto.BookId} not found.");

        copy.InventoryNumber = dto.InventoryNumber;
        copy.Condition = dto.Condition;
        copy.IsAvailable = dto.IsAvailable;
        copy.BookId = dto.BookId;

        await _context.SaveChangesAsync();

        return Ok(MapCopyDto(copy));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCopy(int id)
    {
        var copy = await _context.BookCopies.FindAsync(id);
        if (copy is null)
            return NotFound();

        _context.BookCopies.Remove(copy);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static BookCopyDto MapCopyDto(BookCopy copy) =>
        new(
            copy.Id,
            copy.InventoryNumber,
            copy.Condition,
            copy.IsAvailable,
            new BookSummaryDto(copy.BookId, copy.Book.Title));
}
