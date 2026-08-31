# lv1-library-loan (C#)

Lv.1「図書館の貸出システム」のC#実装。要求文・モデリングは
[../docs.md](../docs.md) / [../model.md](../model.md) を、実装上の検討ログは
[../decisions.md](../decisions.md) を参照。

## セットアップ

```bash
dotnet test
```

## 構成

```
LibraryLoan.BuildingBlocks/   # Entity<TId>, IAggregateRoot
LibraryLoan.Results/          # Result, Result<T>, Error, ResultExtensions
LibraryLoan.Domain/
  Books/                      # Book, BookId, Isbn
  Members/                    # Member(集約ルート), LoanRecord, MemberId, LoanRecordId,
                               # LoanStatus, MemberErrors, LoanRecordErrors
LibraryLoan.Application/
  Books/                      # IBookRepository, InMemoryBookRepository, BookRepositoryErrors
  Members/                    # IMemberRepository, InMemoryMemberRepository, MemberRepositoryErrors
  UseCases/                   # BorrowBookUseCase, ReturnBookUseCase
LibraryLoan.Tests/            # xUnit。Domain/Applicationと同じ階層構成でテストを配置
```

## 設計のポイント

- **集約**: 会員(`Member`)が集約ルート。`LoanRecord`(貸出記録)は会員集約に属するEntity、
  `Book`(本)は別の独立したEntityで、`LoanRecord`からはID参照のみ持つ。
- **業務ルール違反はResultで表現**: 不変条件違反(延滞中の会員は新規貸出不可、貸出ステータスは
  「貸出中」→「返却済」のみ)やNotFound系の失敗は例外ではなく`Result`/`Result<T>`の失敗として返し、
  ユースケース層は`Bind`/`Map`でチェーンする。値オブジェクトの入力バリデーション(空文字、ISBN形式)は
  対象外で、引き続き`ArgumentException`を使う。判断の経緯は[../decisions.md](../decisions.md)を参照。
- **`Member`の生成**: コンストラクタはprivateにし、`Member.Create(...)`(新規作成)と
  `Member.Reconstruct(...)`(永続化層からの復元、既存の貸出記録を保持)の2つのstaticファクトリメソッドに
  統一している。

## 外部から取り込んでいるもの

`LibraryLoan.BuildingBlocks`と`LibraryLoan.Results`は、
[riko-yyy/DesignShowcase](https://github.com/riko-yyy/DesignShowcase)からファイルコピーで取り込んだもの。
取り込みの経緯・独自追加した点は各プロジェクトのREADMEを参照。

- [LibraryLoan.BuildingBlocks/README.md](LibraryLoan.BuildingBlocks/README.md)
- [LibraryLoan.Results/README.md](LibraryLoan.Results/README.md)

## TypeScript版との違い

[../typescript](../typescript)に同じドメインモデルのTypeScript版がある。
言語機能の違いに起因する設計の相違点は[../typescript/README.md](../typescript/README.md)を参照。
