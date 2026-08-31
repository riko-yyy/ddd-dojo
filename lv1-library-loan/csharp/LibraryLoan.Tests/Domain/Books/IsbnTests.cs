using LibraryLoan.Domain.Books;

namespace LibraryLoan.Tests.Domain.Books;

public class IsbnTests
{
    [Theory]
    [InlineData("4798157012")]
    [InlineData("479815701X")]
    [InlineData("4-7981-5701-2")]
    [InlineData("9784798157012")]
    [InlineData("978-4-7981-5701-2")]
    public void 正しい形式のISBNは生成できる(string value)
    {
        var isbn = new Isbn(value);

        Assert.Equal(value, isbn.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")]
    [InlineData("not-an-isbn")]
    public void 不正な形式のISBNは例外になる(string value)
    {
        Assert.Throws<ArgumentException>(() => new Isbn(value));
    }

    [Fact]
    public void 同じ値のISBNは等価である()
    {
        Assert.Equal(new Isbn("4798157012"), new Isbn("4798157012"));
    }
}
