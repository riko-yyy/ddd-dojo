namespace LibraryLoan.Results;

/// <summary>
/// 失敗の理由を表す軽量な値。
/// 文字列だけで表現すると「呼び出し側がこのエラーをどう判定すればいいか」が
/// 曖昧になるため、機械的に判定可能な<see cref="Code"/>と、
/// 人間向けの<see cref="Message"/>を分けて持たせている。
///
/// BuildingBlocksのValueObjectと同じく、構造的等価性を持つ値であるため
/// 自前でEquals/GetHashCodeを実装している(recordは使わない)。
/// </summary>
public readonly struct Error : IEquatable<Error>
{
    /// <summary>
    /// エラー種別を表す識別子(例: "Order.AlreadyShipped")。
    /// 呼び出し側がエラー種別で分岐したい場合はこちらを使う想定。
    /// </summary>
    public string Code { get; }

    /// <summary>人間が読むためのメッセージ。ログやUI表示に使う想定。</summary>
    public string Message { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public Error(string code, string message)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// 「エラーなし」を表す値。Result.Success()の内部でのみ使う。
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// 等価性を判定する
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Error other) => Code == other.Code && Message == other.Message;

    /// <summary>
    /// 等価性を判定する
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is Error other && Equals(other);

    /// <summary>
    /// ハッシュコードを生成する
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => HashCode.Combine(Code, Message);

    /// <summary>
    /// 演算子で等価性を判定する
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Error left, Error right) => left.Equals(right);

    /// <summary>
    /// 演算子で等価性を判定する
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Error left, Error right) => !(left == right);

    /// <summary>
    /// 文字列に変換する
    /// </summary>
    /// <returns></returns>
    public override string ToString() => Code.Length == 0 ? "(none)" : $"{Code}: {Message}";
}
