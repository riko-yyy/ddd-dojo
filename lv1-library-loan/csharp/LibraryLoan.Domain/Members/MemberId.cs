namespace LibraryLoan.Domain.Members;

public sealed record MemberId
{
    public string Value { get; }

    public MemberId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("会員IDは空にできません。", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;
}
