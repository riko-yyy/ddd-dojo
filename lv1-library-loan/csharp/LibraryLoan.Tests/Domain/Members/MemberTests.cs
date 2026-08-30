using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Exceptions;
using LibraryLoan.Domain.Members;

namespace LibraryLoan.Tests.Domain.Members;

public class MemberTests
{
    private static Member 会員を作る() => Member.Create(new MemberId("M-001"), "田中");

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void 氏名が空または空白だと例外になる(string name)
    {
        Assert.Throws<ArgumentException>(() => Member.Create(new MemberId("M-001"), name));
    }

    [Fact]
    public void 新規作成した会員は貸出記録を持たない()
    {
        var member = 会員を作る();

        Assert.Empty(member.LoanRecords);
    }

    [Fact]
    public void 同じIdの会員は同一とみなされる()
    {
        var id = new MemberId("M-001");
        var member1 = Member.Create(id, "田中");
        var member2 = Member.Create(id, "別名義");

        Assert.Equal(member1, member2);
    }

    [Fact]
    public void 異なるIdの会員は同一とみなされない()
    {
        var member1 = Member.Create(new MemberId("M-001"), "田中");
        var member2 = Member.Create(new MemberId("M-002"), "田中");

        Assert.NotEqual(member1, member2);
    }

    [Fact]
    public void Reconstructで既存の貸出記録を保持した状態で復元できる()
    {
        var id = new MemberId("M-001");
        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), new BookId("B-001"), new DateOnly(2026, 8, 1));

        var member = Member.Reconstruct(id, "田中", [loanRecord]);

        Assert.Equal(id, member.Id);
        Assert.Equal("田中", member.Name);
        Assert.Single(member.LoanRecords);
        Assert.Equal(loanRecord, member.LoanRecords[0]);
    }

    [Fact]
    public void Reconstructで復元した延滞中の貸出記録があると新しく借りられない()
    {
        var loanRecord = LoanRecord.Loan(LoanRecordId.NewId(), new BookId("B-001"), new DateOnly(2026, 8, 1)); // 返却期限: 2026-08-15
        var member = Member.Reconstruct(new MemberId("M-001"), "田中", [loanRecord]);

        Assert.Throws<MemberHasOverdueLoanException>(
            () => member.Borrow(LoanRecordId.NewId(), new BookId("B-002"), new DateOnly(2026, 8, 20)));
    }

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
