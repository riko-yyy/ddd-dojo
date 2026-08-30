using LibraryLoan.Domain.Books;

namespace LibraryLoan.Application.Books;

public sealed class InMemoryBookRepository : IBookRepository
{
    private readonly Dictionary<BookId, Book> _books = new();

    public Book? Find(BookId id) => _books.GetValueOrDefault(id);

    public void Save(Book book) => _books[book.Id] = book;
}
