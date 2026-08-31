using System.Text.RegularExpressions;

namespace LibraryLoan.Domain.Books;

public sealed partial record Isbn
{
    public string Value { get; }

    public Isbn(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("ISBNは空にできません。", nameof(value));
        }

        var normalized = value.Replace("-", string.Empty);
        if (!Isbn10Regex().IsMatch(normalized) && !Isbn13Regex().IsMatch(normalized))
        {
            throw new ArgumentException($"ISBNの形式が不正です: {value}", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^\d{9}[\dXx]$")]
    private static partial Regex Isbn10Regex();

    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex Isbn13Regex();
}
