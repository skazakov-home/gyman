using System.ComponentModel.DataAnnotations;

namespace Gyman.BusinessLogicLayer
{
    public class Trainer
    {
        public Trainer() { }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [Phone]
        [Required]
        public string Phone { get; set; }

        public bool IsBusy { get; set; }
    }
}
