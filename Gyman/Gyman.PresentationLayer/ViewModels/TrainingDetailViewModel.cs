using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Data.Services;
using Gyman.PresentationLayer.Views.Services;
using Gyman.PresentationLayer.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class TrainingDetailViewModel : TabViewModelBase, ITrainingDetailViewModel
    {
        private readonly ITrainingRepository trainingRepository;
        private readonly IMemberLookupDataService memberLookupDataService;
        private readonly ITrainerLookupDataService trainerLookupDataService;
        private TrainingWrapper training;

        public TrainingDetailViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            ITrainingRepository trainingRepository,
            IMemberLookupDataService memberLookupDataService,
            ITrainerLookupDataService trainerLookupDataService)
            : base(eventAggregator, dialogMessageService)
        {
            this.trainingRepository = trainingRepository;
            this.memberLookupDataService = memberLookupDataService;
            this.trainerLookupDataService = trainerLookupDataService;
            Members = new ObservableCollection<LookupItem>();
            Trainers = new ObservableCollection<LookupItem>();
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

        public ObservableCollection<LookupItem> Members { get; }

        public ObservableCollection<LookupItem> Trainers { get; }

        public override async Task LoadAsync(int trainingId)
        {
            var training = trainingId > 0
                ? await trainingRepository.GetByIdAsync(trainingId)
                : CreateNewTraining();

            Id = trainingId;
            WrapTraining(training);
            await LoadMemberLookupItemsAsync();
            await LoadTrainerLookupItemsAsync();
        }

        private async Task LoadMemberLookupItemsAsync()
        {
            Members.Clear();
            Members.Add(new NullLookupItem { DisplayMember = "Нет клиента" });
            var lookupItems = await memberLookupDataService.LoadMemberLookupItemsAsync();

            foreach (var item in lookupItems)
            {
                Members.Add(item);
            }
        }

        private async Task LoadTrainerLookupItemsAsync()
        {
            Trainers.Clear();
            Trainers.Add(new NullLookupItem { DisplayMember = "Нет тренера" });
            var lookupItems = await trainerLookupDataService.LoadTrainerLookupItemsAsync();

            foreach (var item in lookupItems)
            {
                Trainers.Add(item);
            }
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
            await trainingRepository.SaveAsync();
            HasChanges = trainingRepository.HasChanges();
            Id = Training.Id;
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
            Title = $"Тренировка";
        }
    }
}
