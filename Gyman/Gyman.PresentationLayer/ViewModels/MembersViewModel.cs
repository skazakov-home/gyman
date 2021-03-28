using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Gyman.PresentationLayer.Data.Repositories;
using Gyman.PresentationLayer.Views.Services;
using Gyman.PresentationLayer.Wrappers;
using Prism.Events;

namespace Gyman.PresentationLayer.ViewModels
{
    public class MembersViewModel : TabViewModelBase, IMembersViewModel
    {
        private readonly IMemberRepository memberRepository;

        public MembersViewModel(
            IEventAggregator eventAggregator,
            IDialogMessageService dialogMessageService,
            IMemberRepository memberRepository)
            : base(eventAggregator, dialogMessageService)
        {
            this.memberRepository = memberRepository;
            Title = "Все клиенты";
            Members = new ObservableCollection<MemberWrapper>();
        }

        public ObservableCollection<MemberWrapper> Members { get; }

        public override async Task LoadAsync(int id)
        {
            Id = id;
            var members = await memberRepository.GetAllAsync();

            foreach (var member in members)
            {
                var wrapper = new MemberWrapper(member);
                Members.Add(wrapper);
            }
        }

        protected override bool CanSave()
        {
            var notNull = Members.All(m => m != null);
            var notErrors = Members.All(m => !m.HasErrors);
            var hasChanges = HasChanges;

            return notNull && notErrors && hasChanges;
        }

        protected override void OnDelete()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSave()
        {
            memberRepository.SaveAsync();
        }
    }
}
