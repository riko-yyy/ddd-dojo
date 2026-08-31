import type { Member } from "../../domain/members/member.js";
import type { MemberId } from "../../domain/members/member-id.js";

export interface MemberRepository {
  find(id: MemberId): Member | undefined;
  save(member: Member): void;
}
