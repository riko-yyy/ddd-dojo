import type { Book } from "../../domain/books/book.js";
import type { BookId } from "../../domain/books/book-id.js";
import type { BookRepository } from "./book-repository.js";

export class InMemoryBookRepository implements BookRepository {
  private readonly books = new Map<string, Book>();

  find(id: BookId): Book | undefined {
    return this.books.get(id.value);
  }

  save(book: Book): void {
    this.books.set(book.id.value, book);
  }
}
