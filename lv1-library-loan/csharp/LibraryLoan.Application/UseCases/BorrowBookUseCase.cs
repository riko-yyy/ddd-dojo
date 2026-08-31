using LibraryLoan.Application.Books;
using LibraryLoan.Application.Members;
using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Members;
using LibraryLoan.Results;

namespace LibraryLoan.Application.UseCases;

/// <summary>ユースケース: 会員が本を借りる。</summary>
public sealed class BorrowBookUseCase
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBookRepository _bookRepository;

    public BorrowBookUseCase(IMemberRepository memberRepository, IBookRepository bookRepository)
    {
        _memberRepository = memberRepository;
        _bookRepository = bookRepository;
    }

    public Result<LoanRecordId> Handle(MemberId memberId, BookId bookId, DateOnly loanDate)
    {
        var member = _memberRepository.Find(memberId);
        if (member is null)
        {
            return Result<LoanRecordId>.Failure(MemberRepositoryErrors.NotFound(memberId));
        }

        if (_bookRepository.Find(bookId) is null)
        {
            return Result<LoanRecordId>.Failure(BookRepositoryErrors.NotFound(bookId));
        }

        var borrowResult = member.Borrow(LoanRecordId.NewId(), bookId, loanDate);
        if (borrowResult.IsFailure)
        {
            return Result<LoanRecordId>.Failure(borrowResult.Error);
        }

        _memberRepository.Save(member);
        return borrowResult.Value.Id;
    }
}
