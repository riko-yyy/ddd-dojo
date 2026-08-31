/**
 * エンティティの基底クラス。
 * 「状態が変わっても、識別子(id)が同じなら同一である」という性質を型として表現する。
 *
 * TypeScriptには`==`演算子のオーバーロードがないため、同一性の比較は
 * `equals()`メソッドで行う(`===`は参照比較になってしまう)。
 */
export abstract class Entity<TId> {
  readonly id: TId;

  protected constructor(id: TId) {
    if (id === null || id === undefined) {
      throw new Error("エンティティのidはnull/undefinedにできません。");
    }
    this.id = id;
  }

  equals(other: unknown): boolean {
    if (!(other instanceof Entity)) {
      return false;
    }
    if (this === other) {
      return true;
    }
    // 実行時の型まで一致させる。継承関係にある別々のエンティティが
    // 同じidを持っていても別物として扱うため、instanceofではなくconstructorを比較する。
    if (this.constructor !== other.constructor) {
      return false;
    }
    return idEquals(this.id, (other as Entity<TId>).id);
  }
}

function idEquals(a: unknown, b: unknown): boolean {
  if (isEquatable(a)) {
    return a.equals(b);
  }
  return a === b;
}

function isEquatable(value: unknown): value is { equals: (other: unknown) => boolean } {
  return (
    typeof value === "object" &&
    value !== null &&
    "equals" in value &&
    typeof (value as { equals: unknown }).equals === "function"
  );
}
