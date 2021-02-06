using System;

namespace Gyman.BusinessLogicLayer
{
    public enum SubscriptionType
    {
        None,
        S,
        M,
        L
    }

    public class Subscription
    {
        public Subscription() { }

        public int Id { get; set; }

        public decimal Cost { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public SubscriptionType SubscriptionType { get; set; }
    }
}