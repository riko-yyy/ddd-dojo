using LibraryLoan.Results;

namespace LibraryLoan.Domain.Members;

public static class MemberErrors
{
    public static Error HasOverdueLoan(MemberId memberId) => new(
        "Member.HasOverdueLoan",
        $"会員(ID: {memberId})は延滞中の貸出記録があるため、新しく本を借りることができません。");
}
