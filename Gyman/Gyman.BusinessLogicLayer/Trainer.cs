using System.ComponentModel.DataAnnotations;

namespace Gyman.BusinessLogicLayer
{
    public class Trainer
    {
        public Trainer() { }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле \"Имя\" обязательно для заполнения.")]
        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов.")]
        public string Surname { get; set; }

        [Phone(ErrorMessage = "Недействительный телефон.")]
        public string Phone { get; set; }

        public bool IsBusy { get; set; }
    }
}
