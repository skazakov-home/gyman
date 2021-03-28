using System.Data.Entity;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.DataAccessLayer;

namespace Gyman.PresentationLayer.Data.Repositories
{
    public class TrainingRepository : GenericRepository<Training, GymanDbContext>,
        ITrainingRepository
    {
        public TrainingRepository(GymanDbContext context) : base(context) { }

        public override async Task<Training> GetByIdAsync(int trainingId)
        {
            return await Context.Trainings.SingleAsync(m => m.Id == trainingId);
        }
    }
}
