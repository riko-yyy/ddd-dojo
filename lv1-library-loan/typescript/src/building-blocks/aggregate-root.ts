/**
 * 集約ルートであることを示すマーカーインターフェース。
 *
 * C#版(riko-yyy/DesignShowcaseのIAggregateRoot)と異なり、TypeScriptの構造的型システムでは
 * 空のインターフェースはコンパイル時の強制力を持たない(どんなオブジェクトも構造的に
 * 合致してしまうため、`implements AggregateRoot`を書き忘れても検出できない)。
 * それでも「このクラスは集約ルートである」という設計意図をコードから読み取れるようにする、
 * というドキュメンテーション目的でC#版と同じ形を採用している。
 */
export interface AggregateRoot {}
