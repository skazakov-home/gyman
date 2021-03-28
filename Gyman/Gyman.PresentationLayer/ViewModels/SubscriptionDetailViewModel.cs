using System.ComponentModel;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Views.Services;
using Gyman.PresentationLayer.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class SubscriptionDetailViewModel : TabViewModelBase, ISubscriptionDetailViewModel
    {
        private readonly ISubscriptionRepository subscriptionRepository;

        private SubscriptionWrapper subscription;

        public SubscriptionDetailViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            ISubscriptionRepository subscriptionRepository)
            : base(eventAggregator, dialogMessageService)
        {
            this.subscriptionRepository = subscriptionRepository;
        }
        
        public SubscriptionWrapper Subscription
        {
            get => subscription;
            set
            {
                subscription = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int subscriptionId)
        {
            var subscription = subscriptionId > 0
                ? await subscriptionRepository.GetByIdAsync(subscriptionId)
                : CreateNewSubscription();

            Id = subscriptionId;
            WrapSubscription(subscription);
            SetTitle();
        }

        protected override bool CanSave()
        {
            var notNull = Subscription != null;
            var notErrors = !Subscription.HasErrors;
            var hasChanges = HasChanges;

            return notNull && notErrors && hasChanges;
        }

        protected override async void OnDelete()
        {
            var result = await dialogMessageService.ShowOkCancelDialogAsync(
                $"Do you really want to delete the trainer {Subscription.SubscriptionType}?",
                "Question");

            if (result == DialogMessageResult.OK)
            {
                subscriptionRepository.Remove(Subscription.Model);
                await subscriptionRepository.SaveAsync();
                RaiseDetailViewDeletedEvent(Subscription.Id);
            }
        }

        protected override async void OnSave()
        {
            await SaveWithOptimisticConcurrencyAsync(
                subscriptionRepository.SaveAsync,
                () =>
                {
                    HasChanges = subscriptionRepository.HasChanges();
                    Id = Subscription.Id;
                    RaiseDetailViewSavedEvent(Subscription.Id,
                        Subscription.SubscriptionType.ToString());
                });
        }

        private Subscription CreateNewSubscription()
        {
            var subscription = new Subscription();
            subscriptionRepository.Add(subscription);

            return subscription;
        }

        private void WrapSubscription(Subscription subscription)
        {
            Subscription = new SubscriptionWrapper(subscription);
            Subscription.PropertyChanged += OnSubscriptionPropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnSubscriptionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = subscriptionRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Subscription.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void SetTitle()
        {
            Title = $"Абонемент \"{Subscription.SubscriptionType}\"";
        }
    }
}
