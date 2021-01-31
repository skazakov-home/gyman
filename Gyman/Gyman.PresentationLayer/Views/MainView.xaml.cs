using System.Windows;
using Gyman.PresentationLayer.ViewModels;

namespace Gyman.PresentationLayer.Views
{
    public partial class MainView : Window
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

        private void OnMainViewLoaded(object sender, RoutedEventArgs e)
        {
            // TODO: Load ViewModel
        }
    }
}
