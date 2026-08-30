using LibraryLoan.Domain.Books;

namespace LibraryLoan.Tests.Domain.Books;

public class BookTests
{
    private static readonly Isbn SampleIsbn = new("4798157012");

    [Fact]
    public void 正しい情報から本を生成できる()
    {
        var id = new BookId("B-001");

        var book = new Book(id, "本1", "著者1", SampleIsbn);

        Assert.Equal(id, book.Id);
        Assert.Equal("本1", book.Title);
        Assert.Equal("著者1", book.Author);
        Assert.Equal(SampleIsbn, book.Isbn);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void タイトルが空または空白だと例外になる(string title)
    {
        Assert.Throws<ArgumentException>(() => new Book(new BookId("B-001"), title, "著者1", SampleIsbn));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void 著者が空または空白だと例外になる(string author)
    {
        Assert.Throws<ArgumentException>(() => new Book(new BookId("B-001"), "本1", author, SampleIsbn));
    }

    [Fact]
    public void 同じIdの本は同一とみなされる()
    {
        var id = new BookId("B-001");
        var book1 = new Book(id, "本1", "著者1", SampleIsbn);
        var book2 = new Book(id, "別タイトル", "別著者", new Isbn("9784798157012"));

        Assert.Equal(book1, book2);
    }

    [Fact]
    public void 異なるIdの本は同一とみなされない()
    {
        var book1 = new Book(new BookId("B-001"), "本1", "著者1", SampleIsbn);
        var book2 = new Book(new BookId("B-002"), "本1", "著者1", SampleIsbn);

        Assert.NotEqual(book1, book2);
    }
}
