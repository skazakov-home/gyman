using System.Data.Entity;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.DataAccessLayer;

namespace Gyman.PresentationLayer.Data.Repositories
{
    public class TrainerRepository : GenericRepository<Trainer, GymanDbContext>,
        ITrainerRepository
    {
        public TrainerRepository(GymanDbContext context) : base(context) { }

        public override async Task<Trainer> GetByIdAsync(int trainerId)
        {
            return await Context.Trainers.SingleAsync(m => m.Id == trainerId);
        }
    }
}
