import { describe, expect, it } from "vitest";
import { Book } from "../../../src/domain/books/book.js";
import { BookId } from "../../../src/domain/books/book-id.js";
import { Isbn } from "../../../src/domain/books/isbn.js";

describe("Book", () => {
  const sampleIsbn = new Isbn("4798157012");

  it("正しい情報から本を生成できる", () => {
    const id = new BookId("B-001");

    const book = new Book(id, "本1", "著者1", sampleIsbn);

    expect(book.id.equals(id)).toBe(true);
    expect(book.title).toBe("本1");
    expect(book.author).toBe("著者1");
    expect(book.isbn.equals(sampleIsbn)).toBe(true);
  });

  it.each(["", "   "])("タイトルが空または空白だと例外になる: %j", (title) => {
    expect(() => new Book(new BookId("B-001"), title, "著者1", sampleIsbn)).toThrow();
  });

  it.each(["", "   "])("著者が空または空白だと例外になる: %j", (author) => {
    expect(() => new Book(new BookId("B-001"), "本1", author, sampleIsbn)).toThrow();
  });

  it("同じIdの本は同一とみなされる", () => {
    const id = new BookId("B-001");
    const book1 = new Book(id, "本1", "著者1", sampleIsbn);
    const book2 = new Book(id, "別タイトル", "別著者", new Isbn("9784798157012"));

    expect(book1.equals(book2)).toBe(true);
  });

  it("異なるIdの本は同一とみなされない", () => {
    const book1 = new Book(new BookId("B-001"), "本1", "著者1", sampleIsbn);
    const book2 = new Book(new BookId("B-002"), "本1", "著者1", sampleIsbn);

    expect(book1.equals(book2)).toBe(false);
  });
});
