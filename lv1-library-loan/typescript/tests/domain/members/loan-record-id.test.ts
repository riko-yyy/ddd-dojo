import { describe, expect, it } from "vitest";
import { LoanRecordId } from "../../../src/domain/members/loan-record-id.js";

describe("LoanRecordId", () => {
  it("空でない文字列から生成できる", () => {
    const loanRecordId = new LoanRecordId("L-101");

    expect(loanRecordId.value).toBe("L-101");
  });

  it.each(["", "   "])("空または空白文字列だと例外になる: %j", (value) => {
    expect(() => new LoanRecordId(value)).toThrow();
  });

  it("同じ値のLoanRecordIdは等価である", () => {
    expect(new LoanRecordId("L-101").equals(new LoanRecordId("L-101"))).toBe(true);
  });

  it("異なる値のLoanRecordIdは等価でない", () => {
    expect(new LoanRecordId("L-101").equals(new LoanRecordId("L-102"))).toBe(false);
  });

  it("newIdを呼ぶたびに異なる値が生成される", () => {
    const id1 = LoanRecordId.newId();
    const id2 = LoanRecordId.newId();

    expect(id1.equals(id2)).toBe(false);
  });
});
