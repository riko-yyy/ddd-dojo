using LibraryLoan.Domain.Members;

namespace LibraryLoan.Domain.Exceptions;

/// <summary>不変条件違反: 貸出ステータスは「貸出中」→「返却済」以外に遷移できない。</summary>
public sealed class InvalidLoanStatusTransitionException : Exception
{
    public LoanRecordId LoanRecordId { get; }
    public LoanStatus CurrentStatus { get; }
    public LoanStatus RequestedStatus { get; }

    public InvalidLoanStatusTransitionException(LoanRecordId loanRecordId, LoanStatus currentStatus, LoanStatus requestedStatus)
        : base($"貸出記録(ID: {loanRecordId})のステータスを{currentStatus}から{requestedStatus}へ変更できません。")
    {
        LoanRecordId = loanRecordId;
        CurrentStatus = currentStatus;
        RequestedStatus = requestedStatus;
    }
}
