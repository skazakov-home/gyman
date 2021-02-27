using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.DataAccessLayer;

namespace Gyman.PresentationLayer.Data.Services
{
    public class LookupDataService :
        IMemberLookupDataService,
        ISubscriptionLookupDataService,
        ITrainerLookupDataService
    {
        private readonly Func<GymanDbContext> contextCreator;

        public LookupDataService(Func<GymanDbContext> contextCreator)
        {
            this.contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> LoadMemberLookupItemsAsync()
        {
            using (var context = contextCreator())
            {
                return await context.Members.AsNoTracking()
                    .Select(member => new LookupItem
                    {
                        Id = member.Id,
                        DisplayMember = member.Name + " " + member.Surname
                    })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> LoadSubscriptionLookupItemsAsync()
        {
            using (var context = contextCreator())
            {
                return await context.Subscriptions.AsNoTracking()
                    .Select(subscription => new LookupItem
                    {
                        Id = subscription.Id,
                        DisplayMember = subscription.SubscriptionType.ToString()
                    })
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> LoadTrainerLookupItemsAsync()
        {
            using (var context = contextCreator())
            {
                return await context.Trainers.AsNoTracking()
                    .Select(trainer => new LookupItem
                    {
                        Id = trainer.Id,
                        DisplayMember = trainer.Name + " " + trainer.Surname
                    })
                    .ToListAsync();
            }
        }
    }
}
