namespace LibraryLoan.Results;

/// <summary>
/// 「処理が成功したか失敗したか」を表現する型。
///
/// 例外は「呼び出し側が回復できない・想定していない異常」のために温存し、
/// バリデーション違反やビジネスルール違反のような「想定内の業務的な失敗」は
/// この型で表現する、という役割分担を前提にしている。
/// </summary>
public class Result
{
    /// <summary>
    /// 成功しているかどうか
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// 失敗しているかどうか
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// エラー
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="error"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private protected Result(bool isSuccess, Error error)
    {
        // 「成功なのにErrorがある」「失敗なのにErrorがない」という
        // 矛盾した状態を作れないようにコンストラクタで強制する。
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("成功結果はErrorを持てません。");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("失敗結果はErrorを持つ必要があります。");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// 成功した結果を生成するファクトリメソッド
    /// </summary>
    /// <returns></returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// 失敗した結果を生成するファクトリメソッド
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// 成功していれば次の処理を実行し、失敗していればその失敗をそのまま伝播する。（Result->Result）
    /// 「途中で失敗したら以降の処理をスキップする」という制御フローを、
    /// if文の連鎖ではなくメソッドチェーンで表現するために用意している。
    /// </summary>
    public Result Bind(Func<Result> next) => IsSuccess ? next() : this;

    /// <summary>
    /// 成功していれば値を伴う処理につなげる。失敗していれば同じErrorを引き継いでResult&lt;T&gt;の失敗として伝播する。（Result->Result&lt;T&gt;）
    /// </summary>
    public Result<T> Bind<T>(Func<Result<T>> next) => IsSuccess ? next() : Result<T>.Failure(Error);
}
