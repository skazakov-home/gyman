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
    public class TrainerDetailViewModel : DetailViewModelBase, ITrainerDetailViewModel
    {
        private readonly ITrainerRepository trainerRepository;

        private TrainerWrapper trainer;

        public TrainerDetailViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            ITrainerRepository trainerRepository)
            : base(eventAggregator, dialogMessageService)
        {
            this.trainerRepository = trainerRepository;
        }

        public TrainerWrapper Trainer
        {
            get => trainer;
            set
            {
                trainer = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int trainerId)
        {
            var trainer = trainerId > 0
                ? await trainerRepository.GetByIdAsync(trainerId)
                : CreateNewTrainer();

            Id = trainerId;
            WrapTrainer(trainer);
        }

        protected override bool CanSave()
        {
            var notNull = Trainer != null;
            var notErrors = !Trainer.HasErrors;
            var hasChanges = HasChanges;

            return notNull && notErrors && hasChanges;
        }

        protected override async void OnDelete()
        {
            var result = dialogMessageService.ShowOkCancelDialog(
                $"Do you really want to delete the trainer {Trainer.Name}?",
                "Question");

            if (result == DialogMessageResult.OK)
            {
                trainerRepository.Remove(Trainer.Model);
                await trainerRepository.SaveAsync();
                RaiseDetailViewDeletedEvent(Trainer.Id);
            }
        }

        protected override async void OnSave()
        {
            await SaveWithOptimisticConcurrencyAsync(
                trainerRepository.SaveAsync,
                () =>
                {
                    HasChanges = trainerRepository.HasChanges();
                    Id = Trainer.Id;
                    RaiseDetailViewSavedEvent(Trainer.Id, $"{Trainer.Name} {Trainer.Surname}");
                });
        }

        private Trainer CreateNewTrainer()
        {
            var trainer = new Trainer();
            trainerRepository.Add(trainer);

            return trainer;
        }

        private void WrapTrainer(Trainer trainer)
        {
            Trainer = new TrainerWrapper(trainer);
            Trainer.PropertyChanged += OnTrainerPropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            CallValidation();
            SetTitle();
        }

        private void OnTrainerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = trainerRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Trainer.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }

            if (e.PropertyName == nameof(Trainer.Name) ||
                e.PropertyName == nameof(Trainer.Surname))
            {
                SetTitle();
            }
        }

        private void CallValidation()
        {
            if (Trainer.Id == 0)
            {
                Trainer.Name = "";
            }
        }

        private void SetTitle()
        {
            Title = $"{Trainer.Name} {Trainer.Surname}";
        }
    }
}
