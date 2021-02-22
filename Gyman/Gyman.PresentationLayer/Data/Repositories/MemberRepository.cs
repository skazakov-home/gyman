using System.Data.Entity;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.DataAccessLayer;

namespace Gyman.PresentationLayer.Data.Repositories
{
    public class MemberRepository : GenericRepository<Member, GymanDbContext>,
        IMemberRepository
    {
        public MemberRepository(GymanDbContext context) : base(context) { }

        public override async Task<Member> GetByIdAsync(int memberId)
        {
            return await Context.Members.SingleAsync(m => m.Id == memberId);
        }
    }
}
