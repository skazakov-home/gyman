using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Views.Services;
using Gyman.PresentationLayer.Wrappers;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class TrainersViewModel : TabViewModelBase, ITrainersViewModel
    {
        private readonly ITrainerRepository trainerRepository;

        public TrainersViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            ITrainerRepository trainerRepository)
            : base(eventAggregator, dialogMessageService)
        {
            this.trainerRepository = trainerRepository;
            Title = "Все тренеры";
            Trainers = new ObservableCollection<TrainerWrapper>();
        }

        public ObservableCollection<TrainerWrapper> Trainers { get; }

        public override async Task LoadAsync(int id)
        {
            Id = id;
            var trainers = await trainerRepository.GetAllAsync();

            foreach (var member in trainers)
            {
                var wrapper = new TrainerWrapper(member);
                Trainers.Add(wrapper);
            }
        }

        protected override bool CanSave()
        {
            var notNull = Trainers.All(m => m != null);
            var notErrors = Trainers.All(m => !m.HasErrors);
            var hasChanges = HasChanges;

            return notNull && notErrors && hasChanges;
        }

        protected override void OnDelete()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSave()
        {
            trainerRepository.SaveAsync();
        }
    }
}
