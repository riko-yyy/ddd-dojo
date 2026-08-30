using LibraryLoan.Domain.Members;

namespace LibraryLoan.Domain.Exceptions;

/// <summary>不変条件違反: 延滞中の貸出記録がある会員は新しく本を借りられない。</summary>
public sealed class MemberHasOverdueLoanException : Exception
{
    public MemberId MemberId { get; }

    public MemberHasOverdueLoanException(MemberId memberId)
        : base($"会員(ID: {memberId})は延滞中の貸出記録があるため、新しく本を借りることができません。")
    {
        MemberId = memberId;
    }
}
