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
        // FindMemberはResult<Member>を返すが、Return()は値を持たないResultを返すため、
        // Result<T>のBindでは繋げない(「値あり→値なし」の組み合わせはこのライブラリが
        // 意図的に提供していない、LibraryLoan.Results/README.md参照)。
        // ここだけはガード節で会員を取り出し、以降をBindでつなぐ。
        var memberResult = FindMember(memberId);
        if (memberResult.IsFailure)
        {
            return Result.Failure(memberResult.Error);
        }

        var member = memberResult.Value;
        return member.Return(loanRecordId).Bind(() =>
        {
            _memberRepository.Save(member);
            return Result.Success();
        });
    }

    private Result<Member> FindMember(MemberId memberId) =>
        _memberRepository.Find(memberId) is { } member
            ? member
            : Result<Member>.Failure(MemberRepositoryErrors.NotFound(memberId));
}
