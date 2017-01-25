using System.ComponentModel.DataAnnotations;

namespace HexaDot.Data.ViewModels.Institute
{
    public class InstituteEditViewModel : IPrimaryContactViewModel
    {        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Logo URL")]
        public string LogoUrl { get; set; }

        [Display(Name = "Website URL")]
        public string WebUrl { get; set; }     

        [Display(Name = "First Name")]
        public string PrimaryContactFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string PrimaryContactLastName { get; set; }

        [Display(Name = "Mobile phone Number")]
        [Phone]
        public string PrimaryContactPhoneNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string PrimaryContactEmail { get; set; }

        [Display(Name = "Full Description")]
        public string Description { get; set; }

        [Display(Name = "Summary Description")]
        [MaxLength(250)]
        public string Summary { get; set; }

    }
}
