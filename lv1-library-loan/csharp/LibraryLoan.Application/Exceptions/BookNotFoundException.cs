using LibraryLoan.Domain.Books;

namespace LibraryLoan.Application.Exceptions;

public sealed class BookNotFoundException : Exception
{
    public BookId BookId { get; }

    public BookNotFoundException(BookId bookId)
        : base($"本(ID: {bookId})が見つかりません。")
    {
        BookId = bookId;
    }
}
