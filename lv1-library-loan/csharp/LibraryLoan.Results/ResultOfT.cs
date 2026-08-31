namespace LibraryLoan.Results;

/// <summary>
/// 値を伴う処理結果。
/// </summary>
/// <typeparam name="T">成功時に得られる値の型。</typeparam>
public sealed class Result<T> : Result
{
    private readonly T? _value;

    /// <summary>
    /// 成功時のみアクセス可能。
    /// 失敗結果からのアクセスを許してnullを返してしまうと、呼び出し側が
    /// IsSuccessの確認を省略していても気づけないため、あえて例外にしている。
    /// </summary>
    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "失敗したResultからValueを取得することはできません。先にIsSuccessを確認してください。");

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="value"></param>
    /// <param name="error"></param>
    private Result(bool isSuccess, T? value, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// 成功した結果を生成するファクトリメソッド
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T> Success(T value) => new(true, value, Error.None);

    /// <summary>
    /// 失敗した結果を生成するファクトリメソッド
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result<T> Failure(Error error) => new(false, default, error);

    /// <summary>
    /// 成功していれば値を変換する。失敗していればErrorを引き継いだまま伝播する。
    /// 「値の変換」と「失敗の伝播」を呼び出し側が毎回if文で書かずに済むようにする。
    /// </summary>
    public Result<TNew> Map<TNew>(Func<T, TNew> mapper) =>
        IsSuccess ? Result<TNew>.Success(mapper(Value)) : Result<TNew>.Failure(Error);

    /// <summary>
    /// 成功していれば、値を使って次のResultを返す処理につなげる(flatMap相当)。
    /// 変換先自体がResultを返す関数(バリデーションなど失敗しうる処理)を
    /// つなげたい場合はMapではなくこちらを使う。
    /// </summary>
    public Result<TNew> Bind<TNew>(Func<T, Result<TNew>> binder) =>
        IsSuccess ? binder(Value) : Result<TNew>.Failure(Error);

    /// <summary>
    /// T -> Result&lt;T&gt;への暗黙変換。
    /// 「成功した値をそのまま返したいだけ」の場面で、毎回Result&lt;T&gt;.Success(value)と
    /// 書かせないための糖衣。失敗側はErrorしか作れないので暗黙変換は用意していない
    /// (どんな型からもErrorに暗黙変換できてしまうと事故のもとになるため)。
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);
}
