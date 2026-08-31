using LibraryLoan.Domain.Members;

namespace LibraryLoan.Tests.Domain.Members;

public class MemberIdTests
{
    [Fact]
    public void 空でない文字列から生成できる()
    {
        var memberId = new MemberId("M-001");

        Assert.Equal("M-001", memberId.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void 空または空白文字列だと例外になる(string value)
    {
        Assert.Throws<ArgumentException>(() => new MemberId(value));
    }

    [Fact]
    public void 同じ値のMemberIdは等価である()
    {
        Assert.Equal(new MemberId("M-001"), new MemberId("M-001"));
    }

    [Fact]
    public void 異なる値のMemberIdは等価でない()
    {
        Assert.NotEqual(new MemberId("M-001"), new MemberId("M-002"));
    }
}
