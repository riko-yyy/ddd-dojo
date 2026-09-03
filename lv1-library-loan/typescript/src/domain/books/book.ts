import { Entity } from "../../building-blocks/entity.js";
import type { BookId } from "./book-id.js";
import type { Isbn } from "./isbn.js";

export class Book extends Entity<BookId> {
  readonly title: string;
  readonly author: string;
  readonly isbn: Isbn;

  constructor(id: BookId, title: string, author: string, isbn: Isbn) {
    super(id);

    if (title.trim().length === 0) {
      throw new Error("タイトルは空にできません。");
    }
    if (author.trim().length === 0) {
      throw new Error("著者は空にできません。");
    }

    this.title = title;
    this.author = author;
    this.isbn = isbn;
  }
}
