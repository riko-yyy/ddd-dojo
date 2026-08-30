using LibraryLoan.Application.Exceptions;
using LibraryLoan.Application.Members;
using LibraryLoan.Application.UseCases;
using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Exceptions;
using LibraryLoan.Domain.Members;

namespace LibraryLoan.Tests.Application.UseCases;

public class ReturnBookUseCaseTests
{
    private readonly InMemoryMemberRepository _memberRepository = new();
    private readonly ReturnBookUseCase _useCase;

    public ReturnBookUseCaseTests()
    {
        _useCase = new ReturnBookUseCase(_memberRepository);
    }

    [Fact]
    public void 借りている本を返却するとステータスが返却済になる()
    {
        var memberId = new MemberId("M-001");
        var member = Member.Create(memberId, "田中");
        var loanRecord = member.Borrow(LoanRecordId.NewId(), new BookId("B-001"), new DateOnly(2026, 8, 20));
        _memberRepository.Save(member);

        _useCase.Handle(memberId, loanRecord.Id);

        var updated = _memberRepository.Find(memberId)!;
        Assert.Equal(LoanStatus.Returned, updated.LoanRecords[0].Status);
    }

    [Fact]
    public void 存在しない会員だと例外になる()
    {
        Assert.Throws<MemberNotFoundException>(
            () => _useCase.Handle(new MemberId("M-999"), LoanRecordId.NewId()));
    }

    [Fact]
    public void 存在しない貸出記録だと例外になる()
    {
        var memberId = new MemberId("M-001");
        _memberRepository.Save(Member.Create(memberId, "田中"));

        Assert.Throws<LoanRecordNotFoundException>(
            () => _useCase.Handle(memberId, LoanRecordId.NewId()));
    }
}
