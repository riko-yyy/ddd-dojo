namespace LibraryLoan.Domain.Books;

public sealed record BookId
{
    public string Value { get; }

    public BookId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("本IDは空にできません。", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;
}
