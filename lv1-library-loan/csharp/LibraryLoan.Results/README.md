# LibraryLoan.Results

[riko-yyy/DesignShowcase](https://github.com/riko-yyy/DesignShowcase/tree/main/src/Results) (v1.0.0時点) の
`Result` / `Result<T>` / `Error` / `ResultExtensions` をコピーして取り込んだもの。

- 元リポジトリはNuGet未公開のため、ファイルコピーで取り込んでいる。元が更新された場合は手動で反映が必要。
- 変更点は namespace を `DesignShowcase.Results` → `LibraryLoan.Results` にした点のみ。

## このリポジトリでの適用範囲

- **業務ルール違反**(不変条件違反・NotFound系)は `Result` / `Result<T>` で表現する。
  例: 延滞中の会員が新しく借りようとする、存在しない貸出記録を返却しようとする。
- **値オブジェクトの入力バリデーション**(空文字、ISBN形式不正など)は引き続き `ArgumentException`。
  値オブジェクトは「不正な値を持つインスタンスをそもそも作らせない」というコンストラクタの契約として
  例外を使う方針は変えていない。
