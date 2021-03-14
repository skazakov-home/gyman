using System.Windows;
using Gyman.PresentationLayer.ViewModels;
using MahApps.Metro.Controls;

namespace Gyman.PresentationLayer.Views
{
    public partial class MainView : MetroWindow
    {
        private readonly MainViewModel mainViewModel;

        public MainView(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;

            InitializeComponent();
            InitializeDataContext();
            SubscribeToEvents();
        }

        private void InitializeDataContext()
        {
            DataContext = mainViewModel;
        }

        private void SubscribeToEvents()
        {
            Loaded += OnMainViewLoaded;
        }

        private async void OnMainViewLoaded(object sender, RoutedEventArgs e)
        {
            await mainViewModel.LoadAsync();
        }
    }
}
