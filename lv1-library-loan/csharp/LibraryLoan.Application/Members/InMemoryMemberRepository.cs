using LibraryLoan.Domain.Members;

namespace LibraryLoan.Application.Members;

public sealed class InMemoryMemberRepository : IMemberRepository
{
    private readonly Dictionary<MemberId, Member> _members = new();

    public Member? Find(MemberId id) => _members.GetValueOrDefault(id);

    public void Save(Member member) => _members[member.Id] = member;
}
