using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.PresentationLayer.Data.Services;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IMemberLookupDataService memberLookupDataService;

        public NavigationViewModel(
            IEventAggregator eventAggregator,
            IMemberLookupDataService memberLookupDataService)
        {
            this.eventAggregator = eventAggregator;
            this.memberLookupDataService = memberLookupDataService;
            Members = new ObservableCollection<NavigationItemViewModel>();
        }

        public ObservableCollection<NavigationItemViewModel> Members { get; }

        public async Task LoadAsync()
        {
            ClearMembers();
            var lookup = await memberLookupDataService.LoadMemberLookupItemsAsync();
            PopulateMembers(lookup);
        }

        private void ClearMembers()
        {
            Members.Clear();
        }

        private void PopulateMembers(IEnumerable<LookupItem> lookup)
        {
            foreach (var item in lookup)
            {
                Members.Add(new NavigationItemViewModel(
                    eventAggregator,
                    item.Id,
                    item.DisplayMember,
                    nameof(MemberDetailViewModel)));
            }
        }
    }
}
