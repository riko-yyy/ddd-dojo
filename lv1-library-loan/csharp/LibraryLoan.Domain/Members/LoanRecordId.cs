namespace LibraryLoan.Domain.Members;

public sealed record LoanRecordId
{
    public string Value { get; }

    public LoanRecordId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("貸出記録IDは空にできません。", nameof(value));
        }

        Value = value;
    }

    public static LoanRecordId NewId() => new(Guid.NewGuid().ToString());

    public override string ToString() => Value;
}
