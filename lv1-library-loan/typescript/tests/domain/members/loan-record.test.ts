import { describe, expect, it } from "vitest";
import { BookId } from "../../../src/domain/books/book-id.js";
import { LocalDate } from "../../../src/domain/shared/local-date.js";
import { LoanRecord } from "../../../src/domain/members/loan-record.js";
import { LoanRecordErrors } from "../../../src/domain/members/loan-record-errors.js";
import { LoanRecordId } from "../../../src/domain/members/loan-record-id.js";
import { LoanStatus } from "../../../src/domain/members/loan-status.js";

describe("LoanRecord", () => {
  const sampleBookId = new BookId("B-001");

  it("貸出すると返却期限は貸出日から14日後になる", () => {
    const loanDate = LocalDate.of(2026, 8, 20);

    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, loanDate);

    expect(loanRecord.dueDate.equals(LocalDate.of(2026, 9, 3))).toBe(true);
    expect(loanRecord.status).toBe(LoanStatus.Borrowed);
  });

  it("返却すると貸出中から返却済に遷移する", () => {
    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, LocalDate.of(2026, 8, 20));

    const result = loanRecord.return();

    expect(result.isSuccess).toBe(true);
    expect(loanRecord.status).toBe(LoanStatus.Returned);
  });

  it("返却済の貸出記録を再度返却すると失敗になる", () => {
    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, LocalDate.of(2026, 8, 20));
    loanRecord.return();

    const result = loanRecord.return();

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(LoanRecordErrors.alreadyReturned(loanRecord.id));
  });

  it("返却期限を過ぎて貸出中なら延滞している", () => {
    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, LocalDate.of(2026, 8, 1));

    expect(loanRecord.isOverdue(LocalDate.of(2026, 8, 16))).toBe(true);
  });

  it("返却期限内なら延滞していない", () => {
    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, LocalDate.of(2026, 8, 20));

    expect(loanRecord.isOverdue(LocalDate.of(2026, 8, 20))).toBe(false);
  });

  it("返却期限を過ぎていても返却済なら延滞していない", () => {
    const loanRecord = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, LocalDate.of(2026, 8, 1));
    loanRecord.return();

    expect(loanRecord.isOverdue(LocalDate.of(2026, 8, 16))).toBe(false);
  });

  it("同じIdの貸出記録は状態が違っても同一とみなされる", () => {
    const id = LoanRecordId.newId();
    const loanRecord1 = LoanRecord.loan(id, sampleBookId, LocalDate.of(2026, 8, 1));
    const loanRecord2 = LoanRecord.loan(id, new BookId("B-999"), LocalDate.of(2026, 8, 20));

    expect(loanRecord1.equals(loanRecord2)).toBe(true);
  });

  it("異なるIdの貸出記録は同一とみなされない", () => {
    const loanRecord1 = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, LocalDate.of(2026, 8, 1));
    const loanRecord2 = LoanRecord.loan(LoanRecordId.newId(), sampleBookId, LocalDate.of(2026, 8, 1));

    expect(loanRecord1.equals(loanRecord2)).toBe(false);
  });
});
