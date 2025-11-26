using System.Text.Json.Serialization;

namespace LibraryApi.Dtos.Books;

public class BookSummaryDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("title")]
    public string Title { get; init; } = string.Empty;
}
