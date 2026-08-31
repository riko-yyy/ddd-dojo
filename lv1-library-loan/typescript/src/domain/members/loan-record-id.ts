export class LoanRecordId {
  readonly value: string;

  constructor(value: string) {
    if (value.trim().length === 0) {
      throw new Error("貸出記録IDは空にできません。");
    }
    this.value = value;
  }

  static newId(): LoanRecordId {
    return new LoanRecordId(crypto.randomUUID());
  }

  equals(other: LoanRecordId): boolean {
    return this.value === other.value;
  }

  toString(): string {
    return this.value;
  }
}
