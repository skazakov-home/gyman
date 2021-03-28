using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Data.Services;
using Gyman.PresentationLayer.Views.Services;
using Gyman.PresentationLayer.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class MemberDetailViewModel : TabViewModelBase, IMemberDetailViewModel
    {
        private readonly IMemberRepository memberRepository;
        private readonly ISubscriptionLookupDataService subscriptionLookupDataService;

        private MemberWrapper member;

        public MemberDetailViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            IMemberRepository memberRepository,
            ISubscriptionLookupDataService subscriptionLookupDataService)
            : base(eventAggregator, dialogMessageService)
        {
            this.memberRepository = memberRepository;
            this.subscriptionLookupDataService = subscriptionLookupDataService;

            Subscriptions = new ObservableCollection<LookupItem>();
        }

        public ObservableCollection<LookupItem> Subscriptions { get; }

        public MemberWrapper Member
        {
            get => member;
            set
            {
                member = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int memberId)
        {
            var member = memberId > 0
                ? await memberRepository.GetByIdAsync(memberId)
                : CreateNewMember();

            Id = memberId;
            WrapMember(member);
            await LoadSubscriptionLookupItemsAsync();
        }

        protected override bool CanSave()
        {
            var notNull = Member != null;
            var notErrors = !Member.HasErrors;
            var hasChanges = HasChanges;

            return notNull && notErrors && hasChanges;
        }

        protected override async void OnSave()
        {
            await SaveWithOptimisticConcurrencyAsync(
                memberRepository.SaveAsync,
                () =>
                {
                    HasChanges = memberRepository.HasChanges();
                    Id = Member.Id;
                    RaiseDetailViewSavedEvent(Member.Id, $"{Member.Name} {Member.Surname}");
                });
        }

        protected override async void OnDelete()
        {
            var result = await dialogMessageService.ShowOkCancelDialogAsync(
                $"Do you really want to delete the member {Member.Name}?",
                "Question");

            if (result == DialogMessageResult.OK)
            {
                memberRepository.Remove(Member.Model);
                await memberRepository.SaveAsync();
                RaiseDetailViewDeletedEvent(Member.Id);
            }
        }

        private Member CreateNewMember()
        {
            var member = new Member();
            memberRepository.Add(member);

            return member;
        }

        private void WrapMember(Member member)
        {
            Member = new MemberWrapper(member);
            Member.PropertyChanged += OnMemberPropertyChanged;
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            CallValidation();
            SetTitle();
        }

        private async Task LoadSubscriptionLookupItemsAsync()
        {
            Subscriptions.Clear();
            Subscriptions.Add(new NullLookupItem { DisplayMember = "Нет абонемента" });
            var lookupItems = await subscriptionLookupDataService.LoadSubscriptionLookupItemsAsync();

            foreach (var item in lookupItems)
            {
                Subscriptions.Add(item);
            }
        }

        private void OnMemberPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = memberRepository.HasChanges();
            }

            if (e.PropertyName == nameof(Member.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }

            if (e.PropertyName == nameof(Member.Name) ||
                e.PropertyName == nameof(Member.Surname))
            {
                SetTitle();
            }
        }

        private void CallValidation()
        {
            if (Member.Id == 0)
            {
                Member.Name = "";
                Member.Surname = "";
                Member.Email = "";
                Member.Phone = "";
                Member.Age = 0;
                Member.Weight = 0;
                Member.Height = 0;
            }
        }

        private void SetTitle()
        {
            Title = $"{Member.Name} {Member.Surname}";
        }
    }
}
