import { beforeEach, describe, expect, it } from "vitest";
import { InMemoryMemberRepository } from "../../../src/application/members/in-memory-member-repository.js";
import { MemberRepositoryErrors } from "../../../src/application/members/member-repository-errors.js";
import { ReturnBookUseCase } from "../../../src/application/use-cases/return-book-use-case.js";
import { BookId } from "../../../src/domain/books/book-id.js";
import { Member } from "../../../src/domain/members/member.js";
import { MemberId } from "../../../src/domain/members/member-id.js";
import { LoanRecordErrors } from "../../../src/domain/members/loan-record-errors.js";
import { LoanRecordId } from "../../../src/domain/members/loan-record-id.js";
import { LocalDate } from "../../../src/domain/shared/local-date.js";

describe("ReturnBookUseCase", () => {
  let memberRepository: InMemoryMemberRepository;
  let useCase: ReturnBookUseCase;

  beforeEach(() => {
    memberRepository = new InMemoryMemberRepository();
    useCase = new ReturnBookUseCase(memberRepository);
  });

  it("借りている本を返却するとステータスが返却済になる", () => {
    const memberId = new MemberId("M-001");
    const member = Member.create(memberId, "田中");
    const loanRecord = member.borrow(LoanRecordId.newId(), new BookId("B-001"), LocalDate.of(2026, 8, 20)).value;
    memberRepository.save(member);

    const result = useCase.handle(memberId, loanRecord.id);

    expect(result.isSuccess).toBe(true);
    const updated = memberRepository.find(memberId);
    expect(updated?.loanRecords[0]?.status).toBe("Returned");
  });

  it("存在しない会員だと失敗になる", () => {
    const memberId = new MemberId("M-999");

    const result = useCase.handle(memberId, LoanRecordId.newId());

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(MemberRepositoryErrors.notFound(memberId));
  });

  it("存在しない貸出記録だと失敗になる", () => {
    const memberId = new MemberId("M-001");
    memberRepository.save(Member.create(memberId, "田中"));
    const loanRecordId = LoanRecordId.newId();

    const result = useCase.handle(memberId, loanRecordId);

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(LoanRecordErrors.notFound(loanRecordId));
  });
});
