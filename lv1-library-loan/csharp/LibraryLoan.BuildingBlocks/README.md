# LibraryLoan.BuildingBlocks

[riko-yyy/DesignShowcase](https://github.com/riko-yyy/DesignShowcase/tree/main/src/BuildingBlocks) (v1.0.0時点) の
`Entity<TId>` / `IAggregateRoot` をコピーして取り込んだもの。

- `ValueObject` は取り込んでいない。このリポジトリでは値オブジェクトを `record` で表現する方針のため。
- 元リポジトリはNuGet未公開のため、ファイルコピーで取り込んでいる。元が更新された場合は手動で反映が必要。
- 変更点は namespace を `DesignShowcase.BuildingBlocks` → `LibraryLoan.BuildingBlocks` にした点のみ。
