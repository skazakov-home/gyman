using System.ComponentModel;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Views.Services;
using Gyman.PresentationLayer.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class TrainingDetailViewModel : TabViewModelBase, ITrainingDetailViewModel
    {
        private readonly ITrainingRepository trainingRepository;

        private TrainingWrapper training;

        public TrainingDetailViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            ITrainingRepository trainingRepository)
            : base(eventAggregator, dialogMessageService)
        {
            this.trainingRepository = trainingRepository;
        }

        public TrainingWrapper Training
        {
            get => training;
            set
            {
                training = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int trainingId)
        {
            var trainer = trainingId > 0
                ? await trainingRepository.GetByIdAsync(trainingId)
                : CreateNewTraining();

            Id = trainingId;
            WrapTraining(trainer);
        }

        protected override bool CanSave()
        {
            var notNull = Training != null;
            var notErrors = !Training.HasErrors;
            var hasChanges = HasChanges;

            return notNull && notErrors && hasChanges;
        }

        protected override async void OnDelete()
        {
            var result = await dialogMessageService.ShowOkCancelDialogAsync(
                $"Do you really want to delete the training?",
                "Question");

            if (result == DialogMessageResult.OK)
            {
                trainingRepository.Remove(Training.Model);
                await trainingRepository.SaveAsync();
                RaiseDetailViewDeletedEvent(Training.Id);
            }
        }

        protected override async void OnSave()
        {
            await SaveWithOptimisticConcurrencyAsync(
                trainingRepository.SaveAsync,
                () =>
                {
                    HasChanges = trainingRepository.HasChanges();
                    Id = Training.Id;
                    RaiseDetailViewSavedEvent(Training.Id, $"{Training.Id}");
                });
        }

        private Training CreateNewTraining()
        {
            var training = new Training();
            trainingRepository.Add(training);

            return training;
        }

        private void WrapTraining(Training training)
        {
            Training = new TrainingWrapper(training);
            Training.PropertyChanged += OnTrainingPropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            SetTitle();
        }

        private void OnTrainingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = trainingRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Training.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void SetTitle()
        {
            Title = $"Тренировка {Training.Id}";
        }
    }
}
