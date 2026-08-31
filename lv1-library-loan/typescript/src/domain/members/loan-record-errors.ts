import { ResultError } from "../../results/result-error.js";
import type { LoanRecordId } from "./loan-record-id.js";

export const LoanRecordErrors = {
  notFound: (loanRecordId: LoanRecordId): ResultError =>
    new ResultError("LoanRecord.NotFound", `貸出記録(ID: ${loanRecordId})が見つかりません。`),

  alreadyReturned: (loanRecordId: LoanRecordId): ResultError =>
    new ResultError("LoanRecord.AlreadyReturned", `貸出記録(ID: ${loanRecordId})は既に返却済みです。`),
};
