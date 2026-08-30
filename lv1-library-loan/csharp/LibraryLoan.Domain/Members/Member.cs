using LibraryLoan.BuildingBlocks;
using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Exceptions;

namespace LibraryLoan.Domain.Members;

/// <summary>会員集約のルート。</summary>
public sealed class Member : Entity<MemberId>, IAggregateRoot
{
    private readonly List<LoanRecord> _loanRecords = new();

    public string Name { get; }
    public IReadOnlyList<LoanRecord> LoanRecords => _loanRecords;

    public Member(MemberId id, string name)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("氏名は空にできません。", nameof(name));
        }

        Name = name;
    }

    /// <summary>
    /// 本を借りる。延滞中の貸出記録が1件でもある場合は借りられない。
    /// </summary>
    public LoanRecord Borrow(LoanRecordId loanRecordId, BookId bookId, DateOnly loanDate)
    {
        if (_loanRecords.Any(r => r.IsOverdue(loanDate)))
        {
            throw new MemberHasOverdueLoanException(Id);
        }

        var loanRecord = LoanRecord.Loan(loanRecordId, bookId, loanDate);
        _loanRecords.Add(loanRecord);
        return loanRecord;
    }

    /// <summary>借りた本を返却する。</summary>
    public void Return(LoanRecordId loanRecordId)
    {
        var loanRecord = _loanRecords.FirstOrDefault(r => r.Id == loanRecordId)
            ?? throw new LoanRecordNotFoundException(loanRecordId);

        loanRecord.Return();
    }
}
