/** 貸出ステータス。「貸出中」→「返却済」の一方向にのみ遷移する。 */
export const LoanStatus = {
  Borrowed: "Borrowed",
  Returned: "Returned",
} as const;

export type LoanStatus = (typeof LoanStatus)[keyof typeof LoanStatus];
