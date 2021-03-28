using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using Gyman.PresentationLayer.Events;
using Gyman.PresentationLayer.Views.Services;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogMessageService dialogMessageService;
        private readonly IIndex<string, ITabViewModel> tabViewModelCreator;
        private ITabViewModel selectedTabViewModel;
        private int newDetailViewId = 0;

        public MainViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            INavigationViewModel navigationViewModel,
            IIndex<string, ITabViewModel> tabViewModelCreator)
        {
            this.eventAggregator = eventAggregator;
            this.dialogMessageService = dialogMessageService;
            this.tabViewModelCreator = tabViewModelCreator;

            NavigationViewModel = navigationViewModel;
            TabViewModels = new ObservableCollection<ITabViewModel>();

            SubscribeEvents();
            RegisterCommands();
        }

        public INavigationViewModel NavigationViewModel { get; }

        public ObservableCollection<ITabViewModel> TabViewModels { get; }

        public ITabViewModel SelectedTabViewModel
        {
            get => selectedTabViewModel;
            set
            {
                selectedTabViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateNewTabViewCommand { get; private set; }

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
            eventAggregator.GetEvent<DetailViewDeletedEvent>()
                .Subscribe(OnDetailViewDeleted);
        }

        private async void OnDetailViewOpened(DetailViewOpenedEventArgs args)
        {
            var viewModelId = args.Id;
            var viewModelName = args.ViewModelName;
            var tabViewModel = GetTabViewModel(viewModelId, viewModelName);

            if (tabViewModel == null)
            {
                tabViewModel = tabViewModelCreator[viewModelName];

                if (await TryLoadTabViewModelAsync(viewModelId, tabViewModel))
                {
                    TabViewModels.Add(tabViewModel);
                }
            }

            SelectedTabViewModel = tabViewModel;
        }

        private void OnDetailViewClosed(DetailViewClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void OnDetailViewDeleted(DetailViewDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }

        private void RegisterCommands()
        {
            CreateNewTabViewCommand = new DelegateCommand<Type>(OnCreateNewTabView);
        }

        private void OnCreateNewTabView(Type viewModelType)
        {
            OnDetailViewOpened(
                new DetailViewOpenedEventArgs
                {
                    Id = newDetailViewId--,
                    ViewModelName = viewModelType.Name
                });
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = TabViewModels.SingleOrDefault(
                            vm => vm.Id == id &&
                            vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
            {
                TabViewModels.Remove(detailViewModel);
            }
        }

        private ITabViewModel GetTabViewModel(int id, string name)
        {
            return TabViewModels.FirstOrDefault(
                vm => vm.Id == id && vm.GetType().Name == name);
        }

        private async Task<bool> TryLoadTabViewModelAsync(
            int id, ITabViewModel tabViewModel)
        {
            try
            {
                await tabViewModel.LoadAsync(id);

                return true;
            }
            catch
            {
                await dialogMessageService.ShowInfoDialogAsync(
                        "Could not load the entity, maybe it was deleted in the meantime " +
                        "by another user. The navigation is refreshed for you.");
                await NavigationViewModel.LoadAsync();

                return false;
            }
        }
    }
}
