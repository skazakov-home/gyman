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

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public SubscriptionType SubscriptionType { get; set; }
    }
}