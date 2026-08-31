import { describe, expect, it } from "vitest";
import { Isbn } from "../../../src/domain/books/isbn.js";

describe("Isbn", () => {
  it.each(["4798157012", "479815701X", "4-7981-5701-2", "9784798157012", "978-4-7981-5701-2"])(
    "正しい形式のISBNは生成できる: %s",
    (value) => {
      const isbn = new Isbn(value);

      expect(isbn.value).toBe(value);
    },
  );

  it.each(["", "   ", "12345", "not-an-isbn"])("不正な形式のISBNは例外になる: %j", (value) => {
    expect(() => new Isbn(value)).toThrow();
  });

  it("同じ値のISBNは等価である", () => {
    expect(new Isbn("4798157012").equals(new Isbn("4798157012"))).toBe(true);
  });
});
