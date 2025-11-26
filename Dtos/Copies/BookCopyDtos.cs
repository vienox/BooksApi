using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryApi.Dtos.Books;

namespace LibraryApi.Dtos.Copies;

public class BookCopyDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("inventoryNumber")]
    public string InventoryNumber { get; init; } = string.Empty;

    [JsonPropertyName("condition")]
    public string? Condition { get; init; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; init; }

    [JsonPropertyName("book")]
    public BookSummaryDto Book { get; init; } = new();
}

public class BookCopyCreateDto
{
    [Required]
    [StringLength(50)]
    [JsonPropertyName("inventoryNumber")]
    public string InventoryNumber { get; set; } = string.Empty;

    [StringLength(250)]
    [JsonPropertyName("condition")]
    public string? Condition { get; set; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; set; } = true;

    [Required]
    [JsonPropertyName("bookId")]
    public int BookId { get; set; }
}

public class BookCopyUpdateDto : BookCopyCreateDto
{
}
