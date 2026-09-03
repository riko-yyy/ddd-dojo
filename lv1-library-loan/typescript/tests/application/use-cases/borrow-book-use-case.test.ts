import { beforeEach, describe, expect, it } from "vitest";
import { InMemoryBookRepository } from "../../../src/application/books/in-memory-book-repository.js";
import { BookRepositoryErrors } from "../../../src/application/books/book-repository-errors.js";
import { InMemoryMemberRepository } from "../../../src/application/members/in-memory-member-repository.js";
import { MemberRepositoryErrors } from "../../../src/application/members/member-repository-errors.js";
import { BorrowBookUseCase } from "../../../src/application/use-cases/borrow-book-use-case.js";
import { Book } from "../../../src/domain/books/book.js";
import { BookId } from "../../../src/domain/books/book-id.js";
import { Isbn } from "../../../src/domain/books/isbn.js";
import { Member } from "../../../src/domain/members/member.js";
import { MemberErrors } from "../../../src/domain/members/member-errors.js";
import { MemberId } from "../../../src/domain/members/member-id.js";
import { LoanRecordId } from "../../../src/domain/members/loan-record-id.js";
import { LocalDate } from "../../../src/domain/shared/local-date.js";

describe("BorrowBookUseCase", () => {
  let memberRepository: InMemoryMemberRepository;
  let bookRepository: InMemoryBookRepository;
  let useCase: BorrowBookUseCase;

  beforeEach(() => {
    memberRepository = new InMemoryMemberRepository();
    bookRepository = new InMemoryBookRepository();
    useCase = new BorrowBookUseCase(memberRepository, bookRepository);
  });

  it("本を借りると貸出記録が会員に追加される", () => {
    const memberId = new MemberId("M-001");
    const bookId = new BookId("B-001");
    memberRepository.save(Member.create(memberId, "田中"));
    bookRepository.save(new Book(bookId, "本1", "著者1", new Isbn("4798157012")));

    const result = useCase.handle(memberId, bookId, LocalDate.of(2026, 8, 20));

    expect(result.isSuccess).toBe(true);
    const member = memberRepository.find(memberId);
    expect(member?.loanRecords).toHaveLength(1);
    expect(member?.loanRecords[0]?.id.equals(result.value)).toBe(true);
  });

  it("存在しない会員だと失敗になる", () => {
    const bookId = new BookId("B-001");
    bookRepository.save(new Book(bookId, "本1", "著者1", new Isbn("4798157012")));
    const memberId = new MemberId("M-999");

    const result = useCase.handle(memberId, bookId, LocalDate.of(2026, 8, 20));

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(MemberRepositoryErrors.notFound(memberId));
  });

  it("存在しない本だと失敗になる", () => {
    const memberId = new MemberId("M-001");
    memberRepository.save(Member.create(memberId, "田中"));
    const bookId = new BookId("B-999");

    const result = useCase.handle(memberId, bookId, LocalDate.of(2026, 8, 20));

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(BookRepositoryErrors.notFound(bookId));
  });

  it("延滞中の会員は借りられない", () => {
    const memberId = new MemberId("M-001");
    const bookId1 = new BookId("B-001");
    const bookId2 = new BookId("B-002");
    const member = Member.create(memberId, "田中");
    member.borrow(LoanRecordId.newId(), bookId1, LocalDate.of(2026, 8, 1));
    memberRepository.save(member);
    bookRepository.save(new Book(bookId1, "本1", "著者1", new Isbn("4798157012")));
    bookRepository.save(new Book(bookId2, "本2", "著者2", new Isbn("9784798157012")));

    const result = useCase.handle(memberId, bookId2, LocalDate.of(2026, 8, 20));

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(MemberErrors.hasOverdueLoan(memberId));
  });
});
