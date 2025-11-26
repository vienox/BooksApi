using System.ComponentModel.DataAnnotations;
using LibraryApi.Dtos.Books;

namespace LibraryApi.Dtos.Authors;

public record AuthorDto(
    int Id,
    string FirstName,
    string LastName,
    DateTime? DateOfBirth,
    string? Biography,
    List<BookSummaryDto> Books);

public record AuthorSimpleDto(int Id, string FirstName, string LastName);

public class AuthorCreateUpdateDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(2000)]
    public string? Biography { get; set; }
}
