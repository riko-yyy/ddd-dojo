using LibraryLoan.Domain.Books;
using LibraryLoan.Results;

namespace LibraryLoan.Application.Books;

public static class BookRepositoryErrors
{
    public static Error NotFound(BookId bookId) => new(
        "Book.NotFound",
        $"本(ID: {bookId})が見つかりません。");
}
