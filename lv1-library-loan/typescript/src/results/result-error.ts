/**
 * 失敗の理由を表す軽量な値。
 *
 * C#版(riko-yyy/DesignShowcaseのError)と同じ設計だが、`Error`はJavaScript組み込みの
 * グローバル型と衝突するため`ResultError`という名前にしている。
 *
 * 文字列だけで表現すると「呼び出し側がこのエラーをどう判定すればいいか」が曖昧になるため、
 * 機械的に判定可能な`code`と、人間向けの`message`を分けて持たせている。
 */
export class ResultError {
  readonly code: string;
  readonly message: string;

  constructor(code: string, message: string) {
    this.code = code;
    this.message = message;
  }

  /** 「エラーなし」を表す値。Result.success()の内部でのみ使う。 */
  static readonly none = new ResultError("", "");

  equals(other: ResultError): boolean {
    return this.code === other.code && this.message === other.message;
  }

  toString(): string {
    return this.code.length === 0 ? "(none)" : `${this.code}: ${this.message}`;
  }
}
