using System;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public decimal Cost { get; set; }

        public int NumberOfDays { get; set; }

        [Required]
        public SubscriptionType SubscriptionType { get; set; }
    }
}