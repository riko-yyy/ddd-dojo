using LibraryLoan.Domain.Books;

namespace LibraryLoan.Application.Books;

public interface IBookRepository
{
    Book? Find(BookId id);

    void Save(Book book);
}
