using System.Data.Entity;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.DataAccessLayer;

namespace Gyman.PresentationLayer.Data.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription, GymanDbContext>,
        ISubscriptionRepository
    {
        public SubscriptionRepository(GymanDbContext context) : base(context) { }

        public override async Task<Subscription> GetByIdAsync(int subscriptionId)
        {
            return await Context.Subscriptions.SingleAsync(s => s.Id == subscriptionId);
        }
    }
}
