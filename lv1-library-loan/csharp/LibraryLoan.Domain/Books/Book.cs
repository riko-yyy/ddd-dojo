namespace LibraryLoan.Domain.Books;

public sealed class Book
{
    public BookId Id { get; }
    public string Title { get; }
    public string Author { get; }
    public Isbn Isbn { get; }

    public Book(BookId id, string title, string author, Isbn isbn)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("タイトルは空にできません。", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("著者は空にできません。", nameof(author));
        }

        Id = id;
        Title = title;
        Author = author;
        Isbn = isbn;
    }
}
