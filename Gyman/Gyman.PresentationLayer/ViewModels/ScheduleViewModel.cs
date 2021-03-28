using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using Gyman.BusinessLogicLayer;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Views.Services;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class ScheduleViewModel : TabViewModelBase, IScheduleViewModel
    {
        private readonly ITrainingRepository trainingRepository;
        private DateTime targetDate;

        public ScheduleViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            ITrainingRepository trainingRepository)
            : base(eventAggregator, dialogMessageService)
        {
            this.trainingRepository = trainingRepository;
            this.targetDate = DateTime.Now;

            Trainings = new ObservableCollection<Training>();
            SourceTrainingsCollection = CollectionViewSource.GetDefaultView(Trainings);
            SourceTrainingsCollection.Filter = new Predicate<object>(t =>
            {
                if (!(t is Training training))
                {
                    return false;
                }

                return training.Start.Date == TargetDate.Date;
            });
            Title = "Расписание";
        }

        public ObservableCollection<Training> Trainings { get; }

        public ICollectionView SourceTrainingsCollection { get; }

        public DateTime TargetDate
        {
            get => targetDate;
            set
            {
                targetDate = value;
                OnPropertyChanged();
                SourceTrainingsCollection.Refresh();
            }
        }

        public override async Task LoadAsync(int id)
        {
            Id = id;
            var trainings = await trainingRepository.GetAllAsync();

            foreach (var training in trainings)
            {
                Trainings.Add(training);
            }
        }

        protected override bool CanSave()
        {
            return true;
        }

        protected override void OnDelete()
        {
            throw new NotImplementedException();
        }

        protected override void OnSave()
        {
            throw new NotImplementedException();
        }
    }
}
