using System.Collections.Generic;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;

namespace Gyman.PresentationLayer.Data.Services
{
    public interface IMemberLookupDataService
    {
        Task<IEnumerable<LookupItem>> LoadMemberLookupItemsAsync();
    }
}