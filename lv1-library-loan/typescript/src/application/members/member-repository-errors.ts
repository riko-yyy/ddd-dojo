import type { MemberId } from "../../domain/members/member-id.js";
import { ResultError } from "../../results/result-error.js";

export const MemberRepositoryErrors = {
  notFound: (memberId: MemberId): ResultError =>
    new ResultError("Member.NotFound", `会員(ID: ${memberId})が見つかりません。`),
};
