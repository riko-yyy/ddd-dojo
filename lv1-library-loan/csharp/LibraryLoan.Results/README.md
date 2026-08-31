# LibraryLoan.Results

[riko-yyy/DesignShowcase](https://github.com/riko-yyy/DesignShowcase/tree/main/src/Results) (v1.0.0時点) の
`Result` / `Result<T>` / `Error` / `ResultExtensions` をコピーして取り込んだもの。

- 元リポジトリはNuGet未公開のため、ファイルコピーで取り込んでいる。元が更新された場合は手動で反映が必要。
- namespace を `DesignShowcase.Results` → `LibraryLoan.Results` に変更。
- `Result<T>.Bind(Func<T, Result> binder)`(値あり→値なし)を独自に追加している。元のDesignShowcaseは
  「実際に必要になってから追加する」方針でこの組み合わせを意図的に含めていなかったが、
  `ReturnBookUseCase`(会員を検索した値を使って、値を持たない返却処理につなげる)で実際に必要になったため追加した。

## このリポジトリでの適用範囲

- **業務ルール違反**(不変条件違反・NotFound系)は `Result` / `Result<T>` で表現する。
  例: 延滞中の会員が新しく借りようとする、存在しない貸出記録を返却しようとする。
- **値オブジェクトの入力バリデーション**(空文字、ISBN形式不正など)は引き続き `ArgumentException`。
  値オブジェクトは「不正な値を持つインスタンスをそもそも作らせない」というコンストラクタの契約として
  例外を使う方針は変えていない。
