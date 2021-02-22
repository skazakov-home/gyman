using System.Windows.Input;
using Gyman.PresentationLayer.Events;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private string displayMember;

        public NavigationItemViewModel(
            IEventAggregator eventAggregator,
            int id,
            string displayMember,
            string detailViewModelName)
        {
            this.eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            DetailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailView);
        }

        public int Id { get; }

        public string DisplayMember
        {
            get => displayMember;
            set
            {
                displayMember = value;
                OnPropertyChanged();
            }
        }

        public string DetailViewModelName { get; }

        public ICommand OpenDetailViewCommand { get; }

        private void OnOpenDetailView()
        {
            eventAggregator.GetEvent<DetailViewOpenedEvent>().Publish(
                new DetailViewOpenedEventArgs
                {
                    Id = Id,
                    ViewModelName = DetailViewModelName
                });
        }
    }
}
