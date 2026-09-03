import { StringValueObject } from "../../building-blocks/string-value-object.js";

export class MemberId extends StringValueObject {
  constructor(value: string) {
    if (value.trim().length === 0) {
      throw new Error("会員IDは空にできません。");
    }
    super(value);
  }
}
