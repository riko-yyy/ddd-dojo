import { Entity } from "../../building-blocks/entity.js";
import { Result } from "../../results/result.js";
import type { BookId } from "../books/book-id.js";
import type { LocalDate } from "../shared/local-date.js";
import { LoanRecordErrors } from "./loan-record-errors.js";
import type { LoanRecordId } from "./loan-record-id.js";
import { LoanStatus } from "./loan-status.js";

const LOAN_PERIOD_DAYS = 14;

export class LoanRecord extends Entity<LoanRecordId> {
  readonly bookId: BookId;
  readonly loanDate: LocalDate;
  readonly dueDate: LocalDate;
  private loanStatus: LoanStatus;

  private constructor(id: LoanRecordId, bookId: BookId, loanDate: LocalDate, dueDate: LocalDate, status: LoanStatus) {
    super(id);
    this.bookId = bookId;
    this.loanDate = loanDate;
    this.dueDate = dueDate;
    this.loanStatus = status;
  }

  get status(): LoanStatus {
    return this.loanStatus;
  }

  static loan(id: LoanRecordId, bookId: BookId, loanDate: LocalDate): LoanRecord {
    return new LoanRecord(id, bookId, loanDate, loanDate.addDays(LOAN_PERIOD_DAYS), LoanStatus.Borrowed);
  }

  isOverdue(asOfDate: LocalDate): boolean {
    return this.loanStatus === LoanStatus.Borrowed && asOfDate.isAfter(this.dueDate);
  }

  return(): Result<void> {
    if (this.loanStatus !== LoanStatus.Borrowed) {
      return Result.failure(LoanRecordErrors.alreadyReturned(this.id));
    }

    this.loanStatus = LoanStatus.Returned;
    return Result.success();
  }
}
