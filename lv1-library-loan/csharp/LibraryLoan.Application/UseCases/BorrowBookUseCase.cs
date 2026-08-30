using LibraryLoan.Application.Books;
using LibraryLoan.Application.Exceptions;
using LibraryLoan.Application.Members;
using LibraryLoan.Domain.Books;
using LibraryLoan.Domain.Members;

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

    public LoanRecordId Handle(MemberId memberId, BookId bookId, DateOnly loanDate)
    {
        var member = _memberRepository.Find(memberId) ?? throw new MemberNotFoundException(memberId);
        _ = _bookRepository.Find(bookId) ?? throw new BookNotFoundException(bookId);

        var loanRecord = member.Borrow(LoanRecordId.NewId(), bookId, loanDate);
        _memberRepository.Save(member);

        return loanRecord.Id;
    }
}
