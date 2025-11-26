using System.ComponentModel.DataAnnotations;
using LibraryApi.Dtos.Authors;
using LibraryApi.Dtos.Copies;

namespace LibraryApi.Dtos.Books;

public record BookDto(
    int Id,
    string Title,
    string? Description,
    int PublishedYear,
    string? Isbn,
    AuthorSimpleDto Author,
    List<BookCopyDto> Copies);

public class BookCreateDto
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Range(0, 2200)]
    public int PublishedYear { get; set; }

    [StringLength(32)]
    public string? Isbn { get; set; }

    [Required]
    public int AuthorId { get; set; }
}

public class BookUpdateDto : BookCreateDto
{
}
