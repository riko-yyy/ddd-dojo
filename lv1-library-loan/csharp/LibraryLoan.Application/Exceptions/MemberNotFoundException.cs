using LibraryLoan.Domain.Members;

namespace LibraryLoan.Application.Exceptions;

public sealed class MemberNotFoundException : Exception
{
    public MemberId MemberId { get; }

    public MemberNotFoundException(MemberId memberId)
        : base($"会員(ID: {memberId})が見つかりません。")
    {
        MemberId = memberId;
    }
}
