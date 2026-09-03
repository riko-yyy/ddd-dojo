/**
 * 「stringを1つだけ持つ」値オブジェクトの基底クラス。
 *
 * C#の`record`は`Equals`/`GetHashCode`/`==`を自動生成してくれるため、
 * このリポジトリのC#実装ではVO用の基底クラスを使わず`record`だけで済ませている。
 * TypeScriptには`record`に相当する機能がなく、`equals`/`toString`を
 * VOごとに手書きすると同じボイラープレートが繰り返されるため、この基底クラスを用意している。
 *
 * 複数フィールドを持つVOが必要になった場合は、DesignShowcaseの`ValueObject`
 * (`GetEqualityComponents()`方式)のような、より汎用的な基底クラスをその時点で追加する。
 * 今は単一stringのVOしか存在しないため、先回りして汎用化はしない。
 */
export abstract class StringValueObject {
  readonly value: string;

  protected constructor(value: string) {
    this.value = value;
  }

  equals(other: unknown): boolean {
    if (!(other instanceof StringValueObject)) {
      return false;
    }
    if (this.constructor !== other.constructor) {
      return false;
    }
    return this.value === other.value;
  }

  toString(): string {
    return this.value;
  }
}
