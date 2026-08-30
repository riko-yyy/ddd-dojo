namespace LibraryLoan.Domain.Members;

/// <summary>貸出ステータス。「貸出中」→「返却済」の一方向にのみ遷移する。</summary>
public enum LoanStatus
{
    Borrowed,
    Returned,
}
