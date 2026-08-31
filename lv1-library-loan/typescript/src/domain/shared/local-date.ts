/**
 * 時刻・タイムゾーンを持たない「日付のみ」を表す値オブジェクト。
 * C#の`DateOnly`に相当する。
 *
 * ネイティブの`Date`はタイムゾーンと時刻情報を持つため、「14日後」のような
 * カレンダー計算にそのまま使うとローカルタイムゾーンの影響で日付がずれる事故が
 * 起きやすい。内部表現を常にUTC0時のタイムスタンプに固定することでそれを避けている。
 */
export class LocalDate {
  private readonly utcMillis: number;

  private constructor(utcMillis: number) {
    this.utcMillis = utcMillis;
  }

  static of(year: number, month: number, day: number): LocalDate {
    const utcMillis = Date.UTC(year, month - 1, day);
    const date = new LocalDate(utcMillis);

    // Date.UTCは月末を超えた日付(例: 2月30日)を翌月に繰り上げてしまうため、
    // 往復させて一致するかどうかで不正な日付を検出する。
    if (date.year !== year || date.month !== month || date.day !== day) {
      throw new Error(`日付が不正です: ${year}-${month}-${day}`);
    }

    return date;
  }

  static fromISODate(isoDate: string): LocalDate {
    const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(isoDate);
    if (match === null) {
      throw new Error(`日付の形式が不正です: ${isoDate}`);
    }

    const [, year, month, day] = match as unknown as [string, string, string, string];
    return LocalDate.of(Number(year), Number(month), Number(day));
  }

  get year(): number {
    return new Date(this.utcMillis).getUTCFullYear();
  }

  get month(): number {
    return new Date(this.utcMillis).getUTCMonth() + 1;
  }

  get day(): number {
    return new Date(this.utcMillis).getUTCDate();
  }

  addDays(days: number): LocalDate {
    const millisPerDay = 24 * 60 * 60 * 1000;
    return new LocalDate(this.utcMillis + days * millisPerDay);
  }

  isAfter(other: LocalDate): boolean {
    return this.utcMillis > other.utcMillis;
  }

  equals(other: LocalDate): boolean {
    return this.utcMillis === other.utcMillis;
  }

  toISODate(): string {
    const year = this.year.toString().padStart(4, "0");
    const month = this.month.toString().padStart(2, "0");
    const day = this.day.toString().padStart(2, "0");
    return `${year}-${month}-${day}`;
  }

  toString(): string {
    return this.toISODate();
  }
}
