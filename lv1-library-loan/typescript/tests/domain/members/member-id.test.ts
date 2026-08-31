import { describe, expect, it } from "vitest";
import { MemberId } from "../../../src/domain/members/member-id.js";

describe("MemberId", () => {
  it("空でない文字列から生成できる", () => {
    const memberId = new MemberId("M-001");

    expect(memberId.value).toBe("M-001");
  });

  it.each(["", "   "])("空または空白文字列だと例外になる: %j", (value) => {
    expect(() => new MemberId(value)).toThrow();
  });

  it("同じ値のMemberIdは等価である", () => {
    expect(new MemberId("M-001").equals(new MemberId("M-001"))).toBe(true);
  });

  it("異なる値のMemberIdは等価でない", () => {
    expect(new MemberId("M-001").equals(new MemberId("M-002"))).toBe(false);
  });
});
