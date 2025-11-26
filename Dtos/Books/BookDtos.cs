using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryApi.Dtos.Authors;
using LibraryApi.Dtos.Copies;

namespace LibraryApi.Dtos.Books;

public class BookDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("year")]
    public int PublishedYear { get; init; }

    [JsonPropertyName("isbn")]
    public string? Isbn { get; init; }

    [JsonPropertyName("author")]
    public AuthorSimpleDto Author { get; init; } = new();

    [JsonPropertyName("copies")]
    public List<BookCopyDto> Copies { get; init; } = new();
}

public class BookCreateDto
{
    [Required]
    [StringLength(200)]
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [Range(0, 2200)]
    [JsonPropertyName("year")]
    public int PublishedYear { get; set; }

    [StringLength(32)]
    [JsonPropertyName("isbn")]
    public string? Isbn { get; set; }

    [Required]
    [JsonPropertyName("authorId")]
    public int AuthorId { get; set; }
}

public class BookUpdateDto : BookCreateDto
{
}
