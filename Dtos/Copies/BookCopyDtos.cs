using System.ComponentModel.DataAnnotations;
using LibraryApi.Dtos.Books;

namespace LibraryApi.Dtos.Copies;

public record BookCopyDto(
    int Id,
    string InventoryNumber,
    string? Condition,
    bool IsAvailable,
    BookSummaryDto Book);

public class BookCopyCreateDto
{
    [Required]
    [StringLength(50)]
    public string InventoryNumber { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Condition { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Required]
    public int BookId { get; set; }
}

public class BookCopyUpdateDto : BookCopyCreateDto
{
}
