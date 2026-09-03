import type { Member } from "../../domain/members/member.js";
import type { LoanRecordId } from "../../domain/members/loan-record-id.js";
import type { MemberId } from "../../domain/members/member-id.js";
import { Result } from "../../results/result.js";
import type { MemberRepository } from "../members/member-repository.js";
import { MemberRepositoryErrors } from "../members/member-repository-errors.js";

/** ユースケース: 会員が借りた本を返却する。 */
export class ReturnBookUseCase {
  constructor(private readonly memberRepository: MemberRepository) {}

  handle(memberId: MemberId, loanRecordId: LoanRecordId): Result<void> {
    return this.findMember(memberId).bind((member) =>
      member.return(loanRecordId).bind(() => {
        this.memberRepository.save(member);
        return Result.success();
      }),
    );
  }

  private findMember(memberId: MemberId): Result<Member> {
    const member = this.memberRepository.find(memberId);
    return member !== undefined ? Result.success(member) : Result.failure(MemberRepositoryErrors.notFound(memberId));
  }
}
