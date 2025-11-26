namespace LibraryApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PublishedYear { get; set; }
    public string? Isbn { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;
    public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();
}
