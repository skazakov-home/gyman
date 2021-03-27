using System.ComponentModel.DataAnnotations;

namespace Gyman.BusinessLogicLayer
{
    public class Member
    {
        public Member() { }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле \"Имя\" обязательно для заполнения.")]
        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов.")]
        public string Surname { get; set; }

        [EmailAddress(ErrorMessage = "Недействительный адрес электронной почты.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Недействительный телефон.")]
        [Required(ErrorMessage = "Поле \"Телефон\" обязательно для заполнения.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле \"Возраст\" обязательно для заполнения.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле \"Вес\" обязательно для заполнения.")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Поле \"Рост\" обязательно для заполнения.")]
        public double Height { get; set; }

        public int? SubscriptionId { get; set; }

        public Subscription Subscription { get; set; }
    }
}
