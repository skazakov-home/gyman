using System;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Gyman.BusinessLogicLayer;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Views.Services;
using Gyman.PresentationLayer.Wrappers;
using Prism.Commands;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class MemberDetailViewModel : DetailViewModelBase, IMemberDetailViewModel
    {
        private readonly IMemberRepository memberRepository;
        private MemberWrapper member;

        public MemberDetailViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            IMemberRepository memberRepository)
            : base(eventAggregator, dialogMessageService)
        {
            this.memberRepository = memberRepository;
        }

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
            var result = dialogMessageService.ShowOkCancelDialog(
                $"Do you really want to delete the member {Member.Name}?",
                "Question");

            if (result == DialogMessageResult.OK)
            {
                memberRepository.Remove(Member.Model);
                await memberRepository.SaveAsync();
                RaiseDetailViewDeletedEvent(Member.Id);
            }
        }

        protected async Task SaveWithOptimisticConcurrencyAsync(
            Func<Task> saveFunc, Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException e)
            {
                var databaseValues = e.Entries.Single().GetDatabaseValues();

                if (databaseValues == null)
                {
                    dialogMessageService.ShowInfoDialog(
                        "The entity has been deleted by another user.");
                    RaiseDetailViewDeletedEvent(Id);

                    return;
                }

                var result = dialogMessageService.ShowOkCancelDialog(
                    "The entity has been changed in the mean time by someone else. " +
                    "Click OK to save you changes anyway, click Cancel to reload " +
                    "the entity from the database.", "Question");

                if (result == DialogMessageResult.OK)
                {
                    var entry = e.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    await saveFunc();
                }
                else
                {
                    await e.Entries.Single().ReloadAsync();
                    await LoadAsync(Id);
                }
            }

            afterSaveAction();
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

        private void OnMemberPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (HasChanges)
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
            }
        }

        private void SetTitle()
        {
            Title = $"{Member.Name} {Member.Surname}";
        }
    }
}
