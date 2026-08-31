using LibraryLoan.Domain.Books;

namespace LibraryLoan.Tests.Domain.Books;

public class BookIdTests
{
    [Fact]
    public void 空でない文字列から生成できる()
    {
        var bookId = new BookId("B-001");

        Assert.Equal("B-001", bookId.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void 空または空白文字列だと例外になる(string value)
    {
        Assert.Throws<ArgumentException>(() => new BookId(value));
    }

    [Fact]
    public void 同じ値のBookIdは等価である()
    {
        Assert.Equal(new BookId("B-001"), new BookId("B-001"));
    }

    [Fact]
    public void 異なる値のBookIdは等価でない()
    {
        Assert.NotEqual(new BookId("B-001"), new BookId("B-002"));
    }
}
