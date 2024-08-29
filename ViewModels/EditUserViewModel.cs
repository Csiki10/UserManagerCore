using System.ComponentModel.DataAnnotations;

namespace UserManagerCore.ViewModels
{
    public class EditUserViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; }

        [Required]
        [Display(Name = "Place of Residence")]
        public string PlaceOfResidence { get; set; }

        public IEnumerable<string> PlacesOfBirth { get; set; }
        public IEnumerable<string> PlacesOfResidence { get; set; }
    }
}
