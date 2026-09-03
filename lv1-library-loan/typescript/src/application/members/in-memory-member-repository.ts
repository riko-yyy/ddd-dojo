import type { Member } from "../../domain/members/member.js";
import type { MemberId } from "../../domain/members/member-id.js";
import type { MemberRepository } from "./member-repository.js";

export class InMemoryMemberRepository implements MemberRepository {
  private readonly members = new Map<string, Member>();

  find(id: MemberId): Member | undefined {
    return this.members.get(id.value);
  }

  save(member: Member): void {
    this.members.set(member.id.value, member);
  }
}
