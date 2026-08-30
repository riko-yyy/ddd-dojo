using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Exceptions;
using LibraryLoan.Domain.Members;

namespace LibraryLoan.Tests.Domain.Members;

public class MemberTests
{
    private static Member 会員を作る() => new(new MemberId("M-001"), "田中");

    [Fact]
    public void 延滞中の貸出記録がなければ本を借りられる()
    {
        var member = 会員を作る();

        var loanRecord = member.Borrow(LoanRecordId.NewId(), new BookId("B-001"), new DateOnly(2026, 8, 20));

        Assert.Single(member.LoanRecords);
        Assert.Equal(LoanStatus.Borrowed, loanRecord.Status);
    }

    [Fact]
    public void 延滞中の貸出記録が1件でもあると新しく借りられない()
    {
        var member = 会員を作る();
        member.Borrow(LoanRecordId.NewId(), new BookId("B-002"), new DateOnly(2026, 8, 1)); // 返却期限: 2026-08-15

        var 基準日 = new DateOnly(2026, 8, 20); // 返却期限を過ぎている

        Assert.Throws<MemberHasOverdueLoanException>(
            () => member.Borrow(LoanRecordId.NewId(), new BookId("B-003"), 基準日));
    }

    [Fact]
    public void 延滞中の本を返却すればまた借りられる()
    {
        var member = 会員を作る();
        var 延滞中の貸出 = member.Borrow(LoanRecordId.NewId(), new BookId("B-002"), new DateOnly(2026, 8, 1));
        member.Return(延滞中の貸出.Id);

        member.Borrow(LoanRecordId.NewId(), new BookId("B-003"), new DateOnly(2026, 8, 20));

        Assert.Equal(2, member.LoanRecords.Count);
    }

    [Fact]
    public void 存在しない貸出記録を返却しようとすると例外になる()
    {
        var member = 会員を作る();

        Assert.Throws<LoanRecordNotFoundException>(() => member.Return(LoanRecordId.NewId()));
    }
}
