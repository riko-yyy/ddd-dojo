using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Members;

namespace LibraryLoan.Tests.Domain.Members;

public class LoanRecordTests
{
    private static readonly BookId SampleBookId = new("B-001");

    [Fact]
    public void 貸出すると返却期限は貸出日から14日後になる()
    {
        var loanDate = new DateOnly(2026, 8, 20);

        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, loanDate);

        Assert.Equal(new DateOnly(2026, 9, 3), loanRecord.DueDate);
        Assert.Equal(LoanStatus.Borrowed, loanRecord.Status);
    }

    [Fact]
    public void 返却すると貸出中から返却済に遷移する()
    {
        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, new DateOnly(2026, 8, 20));

        var result = loanRecord.Return();

        Assert.True(result.IsSuccess);
        Assert.Equal(LoanStatus.Returned, loanRecord.Status);
    }

    [Fact]
    public void 返却済の貸出記録を再度返却すると失敗になる()
    {
        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, new DateOnly(2026, 8, 20));
        loanRecord.Return();

        var result = loanRecord.Return();

        Assert.True(result.IsFailure);
        Assert.Equal(LoanRecordErrors.AlreadyReturned(loanRecord.Id), result.Error);
    }

    [Fact]
    public void 返却期限を過ぎて貸出中なら延滞している()
    {
        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, new DateOnly(2026, 8, 1));

        Assert.True(loanRecord.IsOverdue(new DateOnly(2026, 8, 16)));
    }

    [Fact]
    public void 返却期限内なら延滞していない()
    {
        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, new DateOnly(2026, 8, 20));

        Assert.False(loanRecord.IsOverdue(new DateOnly(2026, 8, 20)));
    }

    [Fact]
    public void 返却期限を過ぎていても返却済なら延滞していない()
    {
        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, new DateOnly(2026, 8, 1));
        loanRecord.Return();

        Assert.False(loanRecord.IsOverdue(new DateOnly(2026, 8, 16)));
    }

    [Fact]
    public void 同じIdの貸出記録は状態が違っても同一とみなされる()
    {
        var id = LoanRecordId.NewId();
        var loanRecord1 = LoanRecord.Loan(id, SampleBookId, new DateOnly(2026, 8, 1));
        var loanRecord2 = LoanRecord.Loan(id, new BookId("B-999"), new DateOnly(2026, 8, 20));

        Assert.Equal(loanRecord1, loanRecord2);
    }

    [Fact]
    public void 異なるIdの貸出記録は同一とみなされない()
    {
        var loanRecord1 = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, new DateOnly(2026, 8, 1));
        var loanRecord2 = LoanRecord.Loan(LoanRecordId.NewId(), SampleBookId, new DateOnly(2026, 8, 1));

        Assert.NotEqual(loanRecord1, loanRecord2);
    }
}
