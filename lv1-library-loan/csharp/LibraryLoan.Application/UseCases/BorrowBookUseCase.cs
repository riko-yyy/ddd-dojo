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
        return FindMember(memberId).Bind(member =>
            FindBook(bookId).Bind(_ =>
                member.Borrow(LoanRecordId.NewId(), bookId, loanDate).Map(loanRecord =>
                {
                    _memberRepository.Save(member);
                    return loanRecord.Id;
                })));
    }

    private Result<Member> FindMember(MemberId memberId) =>
        _memberRepository.Find(memberId) is { } member
            ? member
            : Result<Member>.Failure(MemberRepositoryErrors.NotFound(memberId));

    private Result<Book> FindBook(BookId bookId) =>
        _bookRepository.Find(bookId) is { } book
            ? book
            : Result<Book>.Failure(BookRepositoryErrors.NotFound(bookId));
}
