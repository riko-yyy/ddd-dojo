import { ResultError } from "./result-error.js";

/**
 * 「処理が成功したか失敗したか」を表現する型。
 *
 * 例外は「呼び出し側が回復できない・想定していない異常」のために温存し、
 * バリデーション違反やビジネスルール違反のような「想定内の業務的な失敗」は
 * この型で表現する、という役割分担を前提にしている(C#版と同じ方針)。
 *
 * C#版(riko-yyy/DesignShowcase)は値を持たない`Result`と値を持つ`Result<T>`を
 * 別クラスに分けていた。これはC#に「値を持たない型」を汎用的に表現する型が
 * ないための工夫で、その副作用として「値あり→値なし」のBindの組み合わせを
 * 別途用意する必要が生じていた。TypeScriptには`void`があるため、
 * `Result<T = void>`という1つのクラスに統合でき、その制約自体がなくなる。
 */
export class Result<T = void> {
  readonly isSuccess: boolean;
  readonly error: ResultError;
  private readonly rawValue: T | undefined;

  private constructor(isSuccess: boolean, value: T | undefined, error: ResultError) {
    // 「成功なのにErrorがある」「失敗なのにErrorがない」という
    // 矛盾した状態を作れないようにコンストラクタで強制する。
    if (isSuccess && !error.equals(ResultError.none)) {
      throw new Error("成功結果はErrorを持てません。");
    }
    if (!isSuccess && error.equals(ResultError.none)) {
      throw new Error("失敗結果はErrorを持つ必要があります。");
    }

    this.isSuccess = isSuccess;
    this.rawValue = value;
    this.error = error;
  }

  get isFailure(): boolean {
    return !this.isSuccess;
  }

  /**
   * 成功時のみアクセス可能。
   * 失敗結果からのアクセスを許してundefinedを返してしまうと、呼び出し側が
   * isSuccessの確認を省略していても気づけないため、あえて例外にしている。
   */
  get value(): T {
    if (!this.isSuccess) {
      throw new Error("失敗したResultからvalueを取得することはできません。先にisSuccessを確認してください。");
    }
    return this.rawValue as T;
  }

  static success(): Result<void>;
  static success<T>(value: T): Result<T>;
  static success<T>(value?: T): Result<T> {
    return new Result<T>(true, value as T, ResultError.none);
  }

  static failure<T = void>(error: ResultError): Result<T> {
    return new Result<T>(false, undefined, error);
  }

  /**
   * 成功していれば値を変換する。失敗していればErrorを引き継いだまま伝播する。
   * 「値の変換」と「失敗の伝播」を呼び出し側が毎回if文で書かずに済むようにする。
   */
  map<U>(mapper: (value: T) => U): Result<U> {
    return this.isSuccess ? Result.success(mapper(this.value)) : Result.failure(this.error);
  }

  /**
   * 成功していれば、値を使って次のResultを返す処理につなげる(flatMap相当)。
   * つなげる先はResult<U>・Result<void>のどちらでもよい(TはUの片方に固定されていないため)。
   */
  bind<U>(binder: (value: T) => Result<U>): Result<U> {
    return this.isSuccess ? binder(this.value) : Result.failure(this.error);
  }
}
