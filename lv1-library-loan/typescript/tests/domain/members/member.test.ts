import { describe, expect, it } from "vitest";
import { BookId } from "../../../src/domain/books/book-id.js";
import { Member } from "../../../src/domain/members/member.js";
import { MemberErrors } from "../../../src/domain/members/member-errors.js";
import { MemberId } from "../../../src/domain/members/member-id.js";
import { LoanRecord } from "../../../src/domain/members/loan-record.js";
import { LoanRecordErrors } from "../../../src/domain/members/loan-record-errors.js";
import { LoanRecordId } from "../../../src/domain/members/loan-record-id.js";
import { LocalDate } from "../../../src/domain/shared/local-date.js";

describe("Member", () => {
  const createMember = (): Member => Member.create(new MemberId("M-001"), "田中");

  it.each(["", "   "])("氏名が空または空白だと例外になる: %j", (name) => {
    expect(() => Member.create(new MemberId("M-001"), name)).toThrow();
  });

  it("新規作成した会員は貸出記録を持たない", () => {
    const member = createMember();

    expect(member.loanRecords).toHaveLength(0);
  });

  it("同じIdの会員は同一とみなされる", () => {
    const id = new MemberId("M-001");
    const member1 = Member.create(id, "田中");
    const member2 = Member.create(id, "別名義");

    expect(member1.equals(member2)).toBe(true);
  });

  it("異なるIdの会員は同一とみなされない", () => {
    const member1 = Member.create(new MemberId("M-001"), "田中");
    const member2 = Member.create(new MemberId("M-002"), "田中");

    expect(member1.equals(member2)).toBe(false);
  });

  it("reconstructで既存の貸出記録を保持した状態で復元できる", () => {
    const id = new MemberId("M-001");
    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), new BookId("B-001"), LocalDate.of(2026, 8, 1));

    const member = Member.reconstruct(id, "田中", [loanRecord]);

    expect(member.id.equals(id)).toBe(true);
    expect(member.name).toBe("田中");
    expect(member.loanRecords).toHaveLength(1);
    expect(member.loanRecords[0]?.equals(loanRecord)).toBe(true);
  });

  it("reconstructで復元した延滞中の貸出記録があると新しく借りられない", () => {
    // 返却期限: 2026-08-15
    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), new BookId("B-001"), LocalDate.of(2026, 8, 1));
    const member = Member.reconstruct(new MemberId("M-001"), "田中", [loanRecord]);

    const result = member.borrow(LoanRecordId.newId(), new BookId("B-002"), LocalDate.of(2026, 8, 20));

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(MemberErrors.hasOverdueLoan(member.id));
  });

  it("延滞中の貸出記録がなければ本を借りられる", () => {
    const member = createMember();

    const result = member.borrow(LoanRecordId.newId(), new BookId("B-001"), LocalDate.of(2026, 8, 20));

    expect(result.isSuccess).toBe(true);
    expect(member.loanRecords).toHaveLength(1);
    expect(result.value.status).toBe("Borrowed");
  });

  it("延滞中の貸出記録が1件でもあると新しく借りられない", () => {
    const member = createMember();
    // 返却期限: 2026-08-15
    member.borrow(LoanRecordId.newId(), new BookId("B-002"), LocalDate.of(2026, 8, 1));

    const 基準日 = LocalDate.of(2026, 8, 20); // 返却期限を過ぎている

    const result = member.borrow(LoanRecordId.newId(), new BookId("B-003"), 基準日);

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(MemberErrors.hasOverdueLoan(member.id));
  });

  it("延滞中の本を返却すればまた借りられる", () => {
    const member = createMember();
    const 延滞中の貸出 = member.borrow(LoanRecordId.newId(), new BookId("B-002"), LocalDate.of(2026, 8, 1)).value;
    member.return(延滞中の貸出.id);

    const result = member.borrow(LoanRecordId.newId(), new BookId("B-003"), LocalDate.of(2026, 8, 20));

    expect(result.isSuccess).toBe(true);
    expect(member.loanRecords).toHaveLength(2);
  });

  it("存在しない貸出記録を返却しようとすると失敗になる", () => {
    const member = createMember();
    const loanRecordId = LoanRecordId.newId();

    const result = member.return(loanRecordId);

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(LoanRecordErrors.notFound(loanRecordId));
  });
});
