using LibraryLoan.Results;

namespace LibraryLoan.Domain.Members;

public static class LoanRecordErrors
{
    public static Error NotFound(LoanRecordId loanRecordId) => new(
        "LoanRecord.NotFound",
        $"貸出記録(ID: {loanRecordId})が見つかりません。");

    public static Error AlreadyReturned(LoanRecordId loanRecordId) => new(
        "LoanRecord.AlreadyReturned",
        $"貸出記録(ID: {loanRecordId})は既に返却済みです。");
}
