import { StringValueObject } from "../../building-blocks/string-value-object.js";

export class LoanRecordId extends StringValueObject {
  constructor(value: string) {
    if (value.trim().length === 0) {
      throw new Error("貸出記録IDは空にできません。");
    }
    super(value);
  }

  static newId(): LoanRecordId {
    return new LoanRecordId(crypto.randomUUID());
  }
}
