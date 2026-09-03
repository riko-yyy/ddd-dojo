import type { BookRepository } from "../books/book-repository.js";
import { BookRepositoryErrors } from "../books/book-repository-errors.js";
import type { Book } from "../../domain/books/book.js";
import type { BookId } from "../../domain/books/book-id.js";
import type { Member } from "../../domain/members/member.js";
import type { MemberId } from "../../domain/members/member-id.js";
import { LoanRecordId } from "../../domain/members/loan-record-id.js";
import type { LocalDate } from "../../domain/shared/local-date.js";
import { Result } from "../../results/result.js";
import type { MemberRepository } from "../members/member-repository.js";
import { MemberRepositoryErrors } from "../members/member-repository-errors.js";

/** ユースケース: 会員が本を借りる。 */
export class BorrowBookUseCase {
  constructor(
    private readonly memberRepository: MemberRepository,
    private readonly bookRepository: BookRepository,
  ) {}

  handle(memberId: MemberId, bookId: BookId, loanDate: LocalDate): Result<LoanRecordId> {
    return this.findMember(memberId).bind((member) =>
      this.findBook(bookId).bind(() =>
        member.borrow(LoanRecordId.newId(), bookId, loanDate).map((loanRecord) => {
          this.memberRepository.save(member);
          return loanRecord.id;
        }),
      ),
    );
  }

  private findMember(memberId: MemberId): Result<Member> {
    const member = this.memberRepository.find(memberId);
    return member !== undefined ? Result.success(member) : Result.failure(MemberRepositoryErrors.notFound(memberId));
  }

  private findBook(bookId: BookId): Result<Book> {
    const book = this.bookRepository.find(bookId);
    return book !== undefined ? Result.success(book) : Result.failure(BookRepositoryErrors.notFound(bookId));
  }
}
