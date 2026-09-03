import { describe, expect, it } from "vitest";
import { Result } from "../../src/results/result.js";
import { ResultError } from "../../src/results/result-error.js";

describe("Result", () => {
  it("success()は値を持たない成功結果を作れる", () => {
    const result = Result.success();

    expect(result.isSuccess).toBe(true);
    expect(result.isFailure).toBe(false);
  });

  it("success(value)は値を持つ成功結果を作れる", () => {
    const result = Result.success(42);

    expect(result.isSuccess).toBe(true);
    expect(result.value).toBe(42);
  });

  it("failure()は失敗結果を作れる", () => {
    const error = new ResultError("Some.Error", "何か失敗した");

    const result = Result.failure<number>(error);

    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(error);
  });

  it("失敗結果からvalueを取得しようとすると例外になる", () => {
    const result = Result.failure<number>(new ResultError("Some.Error", "何か失敗した"));

    expect(() => result.value).toThrow();
  });

  it("mapは成功時のみ値を変換する", () => {
    const success = Result.success(2).map((x) => x * 10);
    const failure = Result.failure<number>(new ResultError("E", "e")).map((x) => x * 10);

    expect(success.value).toBe(20);
    expect(failure.isFailure).toBe(true);
  });

  it("bindは成功時のみ次のResultにつなげる(値あり→値あり)", () => {
    const result = Result.success(2).bind((x) => Result.success(x * 10));

    expect(result.value).toBe(20);
  });

  it("bindは成功時のみ次のResultにつなげる(値あり→値なし)", () => {
    let called = false;
    const result = Result.success(2).bind(() => {
      called = true;
      return Result.success();
    });

    expect(called).toBe(true);
    expect(result.isSuccess).toBe(true);
  });

  it("bindは失敗していればErrorを引き継いで伝播する", () => {
    const error = new ResultError("E", "e");
    let called = false;

    const result = Result.failure<number>(error).bind((x) => {
      called = true;
      return Result.success(x * 10);
    });

    expect(called).toBe(false);
    expect(result.isFailure).toBe(true);
    expect(result.error).toEqual(error);
  });
});
