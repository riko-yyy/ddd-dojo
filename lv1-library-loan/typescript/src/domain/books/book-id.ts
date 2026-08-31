export class BookId {
  readonly value: string;

  constructor(value: string) {
    if (value.trim().length === 0) {
      throw new Error("本IDは空にできません。");
    }
    this.value = value;
  }

  equals(other: BookId): boolean {
    return this.value === other.value;
  }

  toString(): string {
    return this.value;
  }
}
