import { describe, expect, it } from "vitest";
import { BookId } from "../../../src/domain/books/book-id.js";

describe("BookId", () => {
  it("空でない文字列から生成できる", () => {
    const bookId = new BookId("B-001");

    expect(bookId.value).toBe("B-001");
  });

  it.each(["", "   "])("空または空白文字列だと例外になる: %j", (value) => {
    expect(() => new BookId(value)).toThrow();
  });

  it("同じ値のBookIdは等価である", () => {
    expect(new BookId("B-001").equals(new BookId("B-001"))).toBe(true);
  });

  it("異なる値のBookIdは等価でない", () => {
    expect(new BookId("B-001").equals(new BookId("B-002"))).toBe(false);
  });
});
