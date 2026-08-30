using LibraryLoan.Application.Exceptions;
using LibraryLoan.Application.Members;
using LibraryLoan.Domain.Members;

namespace LibraryLoan.Application.UseCases;

/// <summary>ユースケース: 会員が借りた本を返却する。</summary>
public sealed class ReturnBookUseCase
{
    private readonly IMemberRepository _memberRepository;

    public ReturnBookUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public void Handle(MemberId memberId, LoanRecordId loanRecordId)
    {
        var member = _memberRepository.Find(memberId) ?? throw new MemberNotFoundException(memberId);
        member.Return(loanRecordId);
        _memberRepository.Save(member);
    }
}
