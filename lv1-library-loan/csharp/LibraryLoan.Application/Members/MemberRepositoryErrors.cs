using LibraryLoan.Domain.Members;
using LibraryLoan.Results;

namespace LibraryLoan.Application.Members;

public static class MemberRepositoryErrors
{
    public static Error NotFound(MemberId memberId) => new(
        "Member.NotFound",
        $"会員(ID: {memberId})が見つかりません。");
}
