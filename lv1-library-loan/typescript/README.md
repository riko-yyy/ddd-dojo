# lv1-library-loan (TypeScript)

C#実装([../csharp](../csharp))と同じドメインモデルをTypeScriptに移植したもの。
要求文・モデリングは [../docs.md](../docs.md) / [../model.md](../model.md) を参照。

## セットアップ

```bash
npm install
npm run typecheck
npm test
```

## 構成

C#版のプロジェクト分割(BuildingBlocks / Results / Domain / Application / Tests)を
フォルダ分割として踏襲している。

```
src/
  building-blocks/   # Entity, AggregateRoot, StringValueObject
  results/           # Result<T>, ResultError
  domain/
    books/           # Book, BookId, Isbn
    members/         # Member, LoanRecord, MemberId, LoanRecordId, LoanStatus, *Errors
    shared/           # LocalDate(日付のみを表す値オブジェクト)
  application/
    books/           # BookRepository, InMemoryBookRepository, BookRepositoryErrors
    members/         # MemberRepository, InMemoryMemberRepository, MemberRepositoryErrors
    use-cases/       # BorrowBookUseCase, ReturnBookUseCase
tests/               # srcと同じ階層構成でテストを配置(vitest)
```

## C#版との違い(移植時の判断)

[riko-yyy/DesignShowcase](https://github.com/riko-yyy/DesignShowcase)にはTypeScript版のBuildingBlocks/Resultsが
存在しないため、`src/building-blocks`と`src/results`はC#版と同じ設計思想を
TypeScriptらしい書き方で再実装したもの(コピーではない)。主な相違点:

- **`Result` / `Result<T>` → `Result<T = void>`に統合**: C#は値を持たない`Result`と
  値を持つ`Result<T>`を別クラスに分けていたため、「値あり→値なし」のBindを別途
  用意する必要があった([../decisions.md](../decisions.md)参照)。TypeScriptには`void`があるため
  1つのクラスに統合でき、その制約自体がなくなる。
- **`Error` → `ResultError`**: `Error`はJavaScript組み込みのグローバル型と衝突するため改名。
- **`IAggregateRoot` → `AggregateRoot`**: TypeScriptでは慣習的にインターフェースへ`I`接頭辞を付けない。
  また構造的型システムのため、空インターフェースはC#と違いコンパイル時の強制力を持たない
  (ドキュメンテーション目的のみ)。
- **`IMemberRepository`/`IBookRepository` → `MemberRepository`/`BookRepository`**: 同様に`I`接頭辞を外した。
- **`DateOnly` → 自作の`LocalDate`**: TypeScript/JavaScriptには日付のみを表す標準型がなく、
  ネイティブの`Date`は時刻・タイムゾーンを持つため貸出日の計算に使うと事故りやすい。
  UTC0時を内部表現に持つ値オブジェクトとして自作した。
- **VOの基底クラス`StringValueObject`を追加**: C#の`record`は`Equals`/`GetHashCode`/`==`を
  自動生成するため、C#版では値オブジェクト用の基底クラスを使わず`record`だけで済ませている
  (DesignShowcaseの`ValueObject`は不採用)。TypeScriptには`record`に相当する機能がなく、
  `equals`/`toString`をVOごとに手書きすると同じボイラープレートが繰り返されるため、
  「stringを1つだけ持つVO」用の軽量な基底クラスとして追加した(`BookId`/`MemberId`/
  `LoanRecordId`/`Isbn`が継承)。複数フィールドを持つVOが必要になったら、その時点で
  DesignShowcase方式(`GetEqualityComponents()`)のような汎用的な基底クラスを検討する。
