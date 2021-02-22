using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Gyman.PresentationLayer.Events;
using Gyman.PresentationLayer.Views.Services;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogMessageService dialogMessageService;
        private readonly IIndex<string, IDetailViewModel> detailViewModelCreator;
        private IDetailViewModel selectedDetailViewModel;

        public MainViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator)
        {
            this.eventAggregator = eventAggregator;
            this.dialogMessageService = dialogMessageService;
            NavigationViewModel = navigationViewModel;
            this.detailViewModelCreator = detailViewModelCreator;
            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            SubscribeEvents();
        }

        public INavigationViewModel NavigationViewModel { get; }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }

        public IDetailViewModel SelectedDetailViewModel
        {
            get => selectedDetailViewModel;
            set
            {
                selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        private void SubscribeEvents()
        {
            eventAggregator.GetEvent<DetailViewOpenedEvent>()
                .Subscribe(OnDetailViewOpened);
            eventAggregator.GetEvent<DetailViewClosedEvent>()
                .Subscribe(OnDetailViewClosed);
        }

        private async void OnDetailViewOpened(DetailViewOpenedEventArgs args)
        {
            var viewModelId = args.Id;
            var viewModelName = args.ViewModelName;
            var detailViewModel = GetDetailViewModel(viewModelId, viewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = detailViewModelCreator[viewModelName];

                if (await TryLoadDetailViewModelAsync(viewModelId, detailViewModel))
                {
                    DetailViewModels.Add(detailViewModel);
                }
            }

            SelectedDetailViewModel = detailViewModel;
        }

        private void OnDetailViewClosed(DetailViewClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(
                            vm => vm.Id == id &&
                            vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private IDetailViewModel GetDetailViewModel(int id, string name)
        {
            return DetailViewModels.FirstOrDefault(
                vm => vm.Id == id && vm.GetType().Name == name);
        }

        private async Task<bool> TryLoadDetailViewModelAsync(
            int id, IDetailViewModel detailViewModel)
        {
            try
            {
                await detailViewModel.LoadAsync(id);

                return true;
            }
            catch
            {
                dialogMessageService.ShowInfoDialog(
                        "Could not load the entity, maybe it was deleted in the meantime " +
                        "by another user. The navigation is refreshed for you.");
                await NavigationViewModel.LoadAsync();

                return false;
            }
        }
    }
}
