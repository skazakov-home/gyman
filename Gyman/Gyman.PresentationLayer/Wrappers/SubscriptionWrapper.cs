using Gyman.BusinessLogicLayer;

namespace Gyman.PresentationLayer.Wrappers
{
    public class SubscriptionWrapper : ModelWrapper<Subscription>
    {
        public SubscriptionWrapper(Subscription model)
            : base(model)
        { }

        public int Id => Model.Id;

        public decimal Cost
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public int NumberOfDays
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public SubscriptionType SubscriptionType
        {
            get => GetValue<SubscriptionType>();
            set => SetValue(value);
        }
    }
}
