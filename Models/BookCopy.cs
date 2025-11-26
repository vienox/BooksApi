namespace LibraryApi.Models;

public class BookCopy
{
    public int Id { get; set; }
    public string InventoryNumber { get; set; } = string.Empty;
    public string? Condition { get; set; }
    public bool IsAvailable { get; set; } = true;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}
