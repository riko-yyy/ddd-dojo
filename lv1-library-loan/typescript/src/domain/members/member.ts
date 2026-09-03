import type { AggregateRoot } from "../../building-blocks/aggregate-root.js";
import { Entity } from "../../building-blocks/entity.js";
import { Result } from "../../results/result.js";
import type { BookId } from "../books/book-id.js";
import type { LocalDate } from "../shared/local-date.js";
import { LoanRecord } from "./loan-record.js";
import { LoanRecordErrors } from "./loan-record-errors.js";
import type { LoanRecordId } from "./loan-record-id.js";
import { MemberErrors } from "./member-errors.js";
import type { MemberId } from "./member-id.js";

/** 会員集約のルート。 */
export class Member extends Entity<MemberId> implements AggregateRoot {
  readonly name: string;
  private readonly loanRecordList: LoanRecord[];

  private constructor(id: MemberId, name: string, loanRecords: LoanRecord[]) {
    super(id);

    if (name.trim().length === 0) {
      throw new Error("氏名は空にできません。");
    }

    this.name = name;
    this.loanRecordList = loanRecords;
  }

  get loanRecords(): readonly LoanRecord[] {
    return this.loanRecordList;
  }

  /** 新規に会員を作成する。貸出記録は0件から始まる。 */
  static create(id: MemberId, name: string): Member {
    return new Member(id, name, []);
  }

  /** 永続化層から会員を再構築する。既存の貸出記録を保持した状態で復元するために使う。 */
  static reconstruct(id: MemberId, name: string, loanRecords: readonly LoanRecord[]): Member {
    return new Member(id, name, [...loanRecords]);
  }

  /** 本を借りる。延滞中の貸出記録が1件でもある場合は借りられない。 */
  borrow(loanRecordId: LoanRecordId, bookId: BookId, loanDate: LocalDate): Result<LoanRecord> {
    if (this.loanRecordList.some((loanRecord) => loanRecord.isOverdue(loanDate))) {
      return Result.failure(MemberErrors.hasOverdueLoan(this.id));
    }

    const loanRecord = LoanRecord.loan(loanRecordId, bookId, loanDate);
    this.loanRecordList.push(loanRecord);
    return Result.success(loanRecord);
  }

  /** 借りた本を返却する。 */
  return(loanRecordId: LoanRecordId): Result<void> {
    const loanRecord = this.loanRecordList.find((record) => record.id.equals(loanRecordId));
    if (loanRecord === undefined) {
      return Result.failure(LoanRecordErrors.notFound(loanRecordId));
    }

    return loanRecord.return();
  }
}
