using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Gyman.PresentationLayer.Data.Services;
using Gyman.PresentationLayer.Events;
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

            SubscribeEvents();
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

        private void SubscribeEvents()
        {
            eventAggregator.GetEvent<DetailViewSavedEvent>().Subscribe(OnDetailViewSaved);
            eventAggregator.GetEvent<DetailViewDeletedEvent>().Subscribe(OnDetailViewDeleted);
        }

        private void OnDetailViewSaved(DetailViewSavedEventArgs e)
        {
            switch (e.ViewModelName)
            {
                case nameof(MemberDetailViewModel):
                    OnDetailSaved(Members, e);
                    break;
                case nameof(TrainerDetailViewModel):
                    OnDetailSaved(Trainers, e);
                    break;
            }
        }

        private void OnDetailSaved(ObservableCollection<NavigationItemViewModel> items,
            DetailViewSavedEventArgs e)
        {
            var lookupItem = items.SingleOrDefault(item => item.Id == e.Id);

            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(
                    eventAggregator, e.Id, e.DisplayMember, e.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = e.DisplayMember;
            }
        }

        private void OnDetailViewDeleted(DetailViewDeletedEventArgs e)
        {
            switch (e.ViewModelName)
            {
                case nameof(MemberDetailViewModel):
                    OnDetailDeleted(Members, e);
                    break;
                case nameof(TrainerDetailViewModel):
                    OnDetailDeleted(Trainers, e);
                    break;
            }
        }

        private void OnDetailDeleted(ObservableCollection<NavigationItemViewModel> items,
            DetailViewDeletedEventArgs e)
        {
            var item = items.SingleOrDefault(f => f.Id == e.Id);

            if (item != null)
            {
                items.Remove(item);
            }
        }
    }
}
