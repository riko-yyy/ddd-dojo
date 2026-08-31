import type { BookId } from "../../domain/books/book-id.js";
import { ResultError } from "../../results/result-error.js";

export const BookRepositoryErrors = {
  notFound: (bookId: BookId): ResultError => new ResultError("Book.NotFound", `本(ID: ${bookId})が見つかりません。`),
};
