using LibraryLoan.Application.Books;
using LibraryLoan.Application.Members;
using LibraryLoan.Application.UseCases;
using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Members;

namespace LibraryLoan.Tests.Application.UseCases;

public class BorrowBookUseCaseTests
{
    private readonly InMemoryMemberRepository _memberRepository = new();
    private readonly InMemoryBookRepository _bookRepository = new();
    private readonly BorrowBookUseCase _useCase;

    public BorrowBookUseCaseTests()
    {
        _useCase = new BorrowBookUseCase(_memberRepository, _bookRepository);
    }

    [Fact]
    public void 本を借りると貸出記録が会員に追加される()
    {
        var memberId = new MemberId("M-001");
        var bookId = new BookId("B-001");
        _memberRepository.Save(Member.Create(memberId, "田中"));
        _bookRepository.Save(new Book(bookId, "本1", "著者1", new Isbn("4798157012")));

        var result = _useCase.Handle(memberId, bookId, new DateOnly(2026, 8, 20));

        Assert.True(result.IsSuccess);
        var member = _memberRepository.Find(memberId)!;
        Assert.Single(member.LoanRecords);
        Assert.Equal(result.Value, member.LoanRecords[0].Id);
    }

    [Fact]
    public void 存在しない会員だと失敗になる()
    {
        var bookId = new BookId("B-001");
        _bookRepository.Save(new Book(bookId, "本1", "著者1", new Isbn("4798157012")));
        var memberId = new MemberId("M-999");

        var result = _useCase.Handle(memberId, bookId, new DateOnly(2026, 8, 20));

        Assert.True(result.IsFailure);
        Assert.Equal(MemberRepositoryErrors.NotFound(memberId), result.Error);
    }

    [Fact]
    public void 存在しない本だと失敗になる()
    {
        var memberId = new MemberId("M-001");
        _memberRepository.Save(Member.Create(memberId, "田中"));
        var bookId = new BookId("B-999");

        var result = _useCase.Handle(memberId, bookId, new DateOnly(2026, 8, 20));

        Assert.True(result.IsFailure);
        Assert.Equal(BookRepositoryErrors.NotFound(bookId), result.Error);
    }

    [Fact]
    public void 延滞中の会員は借りられない()
    {
        var memberId = new MemberId("M-001");
        var bookId1 = new BookId("B-001");
        var bookId2 = new BookId("B-002");
        var member = Member.Create(memberId, "田中");
        member.Borrow(LoanRecordId.NewId(), bookId1, new DateOnly(2026, 8, 1));
        _memberRepository.Save(member);
        _bookRepository.Save(new Book(bookId1, "本1", "著者1", new Isbn("4798157012")));
        _bookRepository.Save(new Book(bookId2, "本2", "著者2", new Isbn("9784798157012")));

        var result = _useCase.Handle(memberId, bookId2, new DateOnly(2026, 8, 20));

        Assert.True(result.IsFailure);
        Assert.Equal(MemberErrors.HasOverdueLoan(memberId), result.Error);
    }
}
