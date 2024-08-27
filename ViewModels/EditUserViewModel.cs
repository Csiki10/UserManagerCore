using System.ComponentModel.DataAnnotations;

namespace UserManagerCore.ViewModels
{
    public class EditUserViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string PlaceOfBirth { get; set; }

        [Required]
        public string PlaceOfResidence { get; set; }
    }
}
