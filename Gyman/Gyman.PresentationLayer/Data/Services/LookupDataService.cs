using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.DataAccessLayer;

namespace Gyman.PresentationLayer.Data.Services
{
    public class LookupDataService : IMemberLookupDataService
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
    }
}
