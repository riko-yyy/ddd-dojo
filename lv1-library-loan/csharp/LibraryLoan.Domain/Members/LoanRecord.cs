using LibraryLoan.BuildingBlocks;
using LibraryLoan.Domain.Books;
using LibraryLoan.Results;

namespace LibraryLoan.Domain.Members;

public sealed class LoanRecord : Entity<LoanRecordId>
{
    private const int LoanPeriodDays = 14;

    public BookId BookId { get; }
    public DateOnly LoanDate { get; }
    public DateOnly DueDate { get; }
    public LoanStatus Status { get; private set; }

    private LoanRecord(LoanRecordId id, BookId bookId, DateOnly loanDate, DateOnly dueDate, LoanStatus status)
        : base(id)
    {
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

    public Result Return()
    {
        if (Status != LoanStatus.Borrowed)
        {
            return Result.Failure(LoanRecordErrors.AlreadyReturned(Id));
        }

        Status = LoanStatus.Returned;
        return Result.Success();
    }
}
