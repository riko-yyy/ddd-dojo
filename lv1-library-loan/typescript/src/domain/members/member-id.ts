export class MemberId {
  readonly value: string;

  constructor(value: string) {
    if (value.trim().length === 0) {
      throw new Error("会員IDは空にできません。");
    }
    this.value = value;
  }

  equals(other: MemberId): boolean {
    return this.value === other.value;
  }

  toString(): string {
    return this.value;
  }
}
