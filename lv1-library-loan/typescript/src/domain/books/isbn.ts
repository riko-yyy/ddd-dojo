const ISBN_10_PATTERN = /^\d{9}[\dXx]$/;
const ISBN_13_PATTERN = /^\d{13}$/;

export class Isbn {
  readonly value: string;

  constructor(value: string) {
    if (value.trim().length === 0) {
      throw new Error("ISBNは空にできません。");
    }

    const normalized = value.replaceAll("-", "");
    if (!ISBN_10_PATTERN.test(normalized) && !ISBN_13_PATTERN.test(normalized)) {
      throw new Error(`ISBNの形式が不正です: ${value}`);
    }

    this.value = value;
  }

  equals(other: Isbn): boolean {
    return this.value === other.value;
  }

  toString(): string {
    return this.value;
  }
}
