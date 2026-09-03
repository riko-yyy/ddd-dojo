import { describe, expect, it } from "vitest";
import { LocalDate } from "../../../src/domain/shared/local-date.js";

describe("LocalDate", () => {
  it("of()から生成しISO形式で文字列化できる", () => {
    const date = LocalDate.of(2026, 8, 20);

    expect(date.toISODate()).toBe("2026-08-20");
  });

  it("不正な日付(存在しない日)は例外になる", () => {
    expect(() => LocalDate.of(2026, 2, 30)).toThrow();
  });

  it("fromISODateで文字列から生成できる", () => {
    const date = LocalDate.fromISODate("2026-08-20");

    expect(date.year).toBe(2026);
    expect(date.month).toBe(8);
    expect(date.day).toBe(20);
  });

  it("不正な形式の文字列は例外になる", () => {
    expect(() => LocalDate.fromISODate("2026/08/20")).toThrow();
  });

  it("addDaysで日数を加算できる(月をまたぐ)", () => {
    const date = LocalDate.of(2026, 8, 20).addDays(14);

    expect(date.toISODate()).toBe("2026-09-03");
  });

  it("isAfterで前後を比較できる", () => {
    const earlier = LocalDate.of(2026, 8, 20);
    const later = LocalDate.of(2026, 8, 21);

    expect(later.isAfter(earlier)).toBe(true);
    expect(earlier.isAfter(later)).toBe(false);
    expect(earlier.isAfter(earlier)).toBe(false);
  });

  it("同じ日付は等価である", () => {
    expect(LocalDate.of(2026, 8, 20).equals(LocalDate.of(2026, 8, 20))).toBe(true);
  });
});
