import type { Book } from "../../domain/books/book.js";
import type { BookId } from "../../domain/books/book-id.js";

export interface BookRepository {
  find(id: BookId): Book | undefined;
  save(book: Book): void;
}
