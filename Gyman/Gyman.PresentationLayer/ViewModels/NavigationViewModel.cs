using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Gyman.PresentationLayer.Data.Services;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IMemberLookupDataService memberLookupDataService;
        private readonly ITrainerLookupDataService trainerLookupDataService;

        public NavigationViewModel(
            IEventAggregator eventAggregator,
            IMemberLookupDataService memberLookupDataService,
            ITrainerLookupDataService trainerLookupDataService)
        {
            this.eventAggregator = eventAggregator;
            this.memberLookupDataService = memberLookupDataService;
            this.trainerLookupDataService = trainerLookupDataService;

            Members = new ObservableCollection<NavigationItemViewModel>();
            Trainers = new ObservableCollection<NavigationItemViewModel>();
        }

        public ObservableCollection<NavigationItemViewModel> Members { get; }
        public ObservableCollection<NavigationItemViewModel> Trainers { get; }

        public async Task LoadAsync()
        {
            ClearItems();
            await LoadMembers();
            await LoadTrainers();
        }

        private void ClearItems()
        {
            Members.Clear();
            Trainers.Clear();
        }

        private async Task LoadMembers()
        {
            var members = await memberLookupDataService.LoadMemberLookupItemsAsync();

            foreach (var member in members)
            {
                Members.Add(new NavigationItemViewModel(
                    eventAggregator,
                    member.Id,
                    member.DisplayMember,
                    nameof(MemberDetailViewModel)));
            }
        }

        private async Task LoadTrainers()
        {
            var trainers = await trainerLookupDataService.LoadTrainerLookupItemsAsync();

            foreach (var trainer in trainers)
            {
                Trainers.Add(new NavigationItemViewModel(
                    eventAggregator,
                    trainer.Id,
                    trainer.DisplayMember,
                    nameof(TrainerDetailViewModel)));
            }
        }
    }
}
