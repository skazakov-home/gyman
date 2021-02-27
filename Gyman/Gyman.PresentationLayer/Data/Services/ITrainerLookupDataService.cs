using System.Collections.Generic;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;

namespace Gyman.PresentationLayer.Data.Services
{
    public interface ITrainerLookupDataService
    {
        Task<IEnumerable<LookupItem>> LoadTrainerLookupItemsAsync();
    }
}