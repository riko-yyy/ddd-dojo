import { StringValueObject } from "../../building-blocks/string-value-object.js";

export class BookId extends StringValueObject {
  constructor(value: string) {
    if (value.trim().length === 0) {
      throw new Error("本IDは空にできません。");
    }
    super(value);
  }
}
