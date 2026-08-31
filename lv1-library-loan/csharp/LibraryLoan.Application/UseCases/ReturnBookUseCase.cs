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
        return FindMember(memberId).Bind(member =>
            member.Return(loanRecordId).Bind(() =>
            {
                _memberRepository.Save(member);
                return Result.Success();
            }));
    }

    private Result<Member> FindMember(MemberId memberId) =>
        _memberRepository.Find(memberId) is { } member
            ? member
            : Result<Member>.Failure(MemberRepositoryErrors.NotFound(memberId));
}
