using LibraryLoan.Application.Members;
using LibraryLoan.Domain.Members;
using LibraryLoan.Results;

namespace LibraryLoan.Application.UseCases;

/// <summary>ユースケース: 会員が借りた本を返却する。</summary>
public sealed class ReturnBookUseCase
{
    private readonly IMemberRepository _memberRepository;

    public ReturnBookUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public Result Handle(MemberId memberId, LoanRecordId loanRecordId)
    {
        var member = _memberRepository.Find(memberId);
        if (member is null)
        {
            return Result.Failure(MemberRepositoryErrors.NotFound(memberId));
        }

        var returnResult = member.Return(loanRecordId);
        if (returnResult.IsFailure)
        {
            return returnResult;
        }

        _memberRepository.Save(member);
        return Result.Success();
    }
}
