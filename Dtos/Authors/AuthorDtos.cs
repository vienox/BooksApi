using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryApi.Dtos.Books;

namespace LibraryApi.Dtos.Authors;

public class AuthorDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; init; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; init; } = string.Empty;

    [JsonPropertyName("date_of_birth")]
    public DateTime? DateOfBirth { get; init; }

    [JsonPropertyName("biography")]
    public string? Biography { get; init; }

    [JsonPropertyName("books")]
    public List<BookSummaryDto> Books { get; init; } = new();
}

public class AuthorSimpleDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; init; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; init; } = string.Empty;
}

public class AuthorCreateUpdateDto
{
    [Required]
    [StringLength(100)]
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [JsonPropertyName("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(2000)]
    [JsonPropertyName("biography")]
    public string? Biography { get; set; }
}
