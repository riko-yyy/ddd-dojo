import { ResultError } from "../../results/result-error.js";
import type { MemberId } from "./member-id.js";

export const MemberErrors = {
  hasOverdueLoan: (memberId: MemberId): ResultError =>
    new ResultError(
      "Member.HasOverdueLoan",
      `会員(ID: ${memberId})は延滞中の貸出記録があるため、新しく本を借りることができません。`,
    ),
};
