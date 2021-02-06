using System;
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

        public DateTime Start
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime End
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public SubscriptionType SubscriptionType
        {
            get => GetValue<SubscriptionType>();
            set => SetValue(value);
        }
    }
}
