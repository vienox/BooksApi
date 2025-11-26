using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<BookCopy> BookCopies => Set<BookCopy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.Biography)
                .HasMaxLength(2000);

            entity.HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.Description)
                .HasMaxLength(2000);

            entity.Property(b => b.Isbn)
                .HasMaxLength(32);

            entity.Property(b => b.PublishedYear)
                .HasDefaultValue(0);

            entity.HasMany(b => b.Copies)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BookCopy>(entity =>
        {
            entity.Property(c => c.InventoryNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(c => c.Condition)
                .HasMaxLength(250);
        });
    }
}
