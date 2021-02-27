using System.Collections.Generic;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;

namespace Gyman.PresentationLayer.Data.Services
{
    public interface ISubscriptionLookupDataService
    {
        Task<IEnumerable<LookupItem>> LoadSubscriptionLookupItemsAsync();
    }
}