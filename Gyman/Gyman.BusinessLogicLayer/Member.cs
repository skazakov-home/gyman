using System.ComponentModel.DataAnnotations;

namespace Gyman.BusinessLogicLayer
{
    public class Member
    {
        public Member() { }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Required]
        public string Phone { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double Height { get; set; }

        public int? SubscriptionId { get; set; }

        public Subscription Subscription { get; set; }
    }
}
