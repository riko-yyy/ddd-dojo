namespace LibraryLoan.BuildingBlocks;

/// <summary>
/// エンティティの基底クラス。
/// DDDにおけるエンティティの性質――「状態が変わっても、識別子(Id)が同じなら同一である」――を
/// 型として表現する。ValueObjectとの対比が、このクラスの存在理由そのもの。
/// </summary>
/// <typeparam name="TId">
/// 識別子の型。int/Guidのようなprimitiveでも、専用のValueObject(例: OrderId)でもよい。
/// 後者にすると「異なる集約のIdを取り違える」というミスをコンパイル時に防げる。
/// </typeparam>
public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
{
    /// <summary>
    /// エンティティの識別子
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// エンティティのコンストラクタ
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected Entity(TId id)
    {
        // Idを持たないエンティティは存在しない、という制約をコンストラクタで強制する。
        // (例外的にDBの自動採番待ちのような状態を許容したい場合は、
        //  TIdをnull許容にするか、専用のFactoryパターンで扱う方が素直)
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    /// <summary>
    /// エンティティの等価性判定
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        // 実行時の型まで一致させる。
        // 例えば Employee : Person と Customer : Person が同じIdを持っていても
        // 別物として扱いたいため、GetType()を使い is/asのポリモーフィックな比較にはしない。
        if (GetType() != other.GetType()) return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// エンティティの等価性判定
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);

    /// <summary>
    /// エンティティのハッシュコード
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    /// <summary>
    /// エンティティの演算子による等価性判定
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    /// <summary>
    /// エンティティの演算子による不等価性判定
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
}
