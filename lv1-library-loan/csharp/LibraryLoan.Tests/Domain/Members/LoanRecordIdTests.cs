using LibraryLoan.Domain.Members;

namespace LibraryLoan.Tests.Domain.Members;

public class LoanRecordIdTests
{
    [Fact]
    public void 空でない文字列から生成できる()
    {
        var loanRecordId = new LoanRecordId("L-101");

        Assert.Equal("L-101", loanRecordId.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void 空または空白文字列だと例外になる(string value)
    {
        Assert.Throws<ArgumentException>(() => new LoanRecordId(value));
    }

    [Fact]
    public void 同じ値のLoanRecordIdは等価である()
    {
        Assert.Equal(new LoanRecordId("L-101"), new LoanRecordId("L-101"));
    }

    [Fact]
    public void 異なる値のLoanRecordIdは等価でない()
    {
        Assert.NotEqual(new LoanRecordId("L-101"), new LoanRecordId("L-102"));
    }

    [Fact]
    public void NewIdを呼ぶたびに異なる値が生成される()
    {
        var id1 = LoanRecordId.NewId();
        var id2 = LoanRecordId.NewId();

        Assert.NotEqual(id1, id2);
    }
}
