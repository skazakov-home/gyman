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

        [Required(ErrorMessage = "Поле \"Стоимость\" обязательно для заполнения.")]
        public decimal Cost { get; set; }

        public int NumberOfDays { get; set; }

        [Required]
        public SubscriptionType SubscriptionType { get; set; }
    }
}