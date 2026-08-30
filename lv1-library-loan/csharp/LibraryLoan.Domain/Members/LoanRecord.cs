using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Exceptions;

namespace LibraryLoan.Domain.Members;

public sealed class LoanRecord
{
    private const int LoanPeriodDays = 14;

    public LoanRecordId Id { get; }
    public BookId BookId { get; }
    public DateOnly LoanDate { get; }
    public DateOnly DueDate { get; }
    public LoanStatus Status { get; private set; }

    private LoanRecord(LoanRecordId id, BookId bookId, DateOnly loanDate, DateOnly dueDate, LoanStatus status)
    {
        Id = id;
        BookId = bookId;
        LoanDate = loanDate;
        DueDate = dueDate;
        Status = status;
    }

    public static LoanRecord Loan(LoanRecordId id, BookId bookId, DateOnly loanDate)
    {
        return new LoanRecord(id, bookId, loanDate, loanDate.AddDays(LoanPeriodDays), LoanStatus.Borrowed);
    }

    public bool IsOverdue(DateOnly asOfDate) => Status == LoanStatus.Borrowed && asOfDate > DueDate;

    public void Return()
    {
        if (Status != LoanStatus.Borrowed)
        {
            throw new InvalidLoanStatusTransitionException(Id, Status, LoanStatus.Returned);
        }

        Status = LoanStatus.Returned;
    }
}
