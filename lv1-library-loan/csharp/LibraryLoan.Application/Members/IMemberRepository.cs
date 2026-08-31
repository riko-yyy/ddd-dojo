using LibraryLoan.Domain.Members;

namespace LibraryLoan.Application.Members;

public interface IMemberRepository
{
    Member? Find(MemberId id);

    void Save(Member member);
}
