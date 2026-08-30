using LibraryLoan.Domain.Members;

namespace LibraryLoan.Domain.Exceptions;

public sealed class LoanRecordNotFoundException : Exception
{
    public LoanRecordId LoanRecordId { get; }

    public LoanRecordNotFoundException(LoanRecordId loanRecordId)
        : base($"貸出記録(ID: {loanRecordId})が見つかりません。")
    {
        LoanRecordId = loanRecordId;
    }
}
