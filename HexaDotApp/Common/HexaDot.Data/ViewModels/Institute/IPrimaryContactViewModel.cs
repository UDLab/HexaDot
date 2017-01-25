﻿using System.ComponentModel.DataAnnotations;

namespace HexaDot.Data.ViewModels.Institute
{
    public interface IPrimaryContactViewModel
    {
        [Display(Name = "First Name")]
        string PrimaryContactFirstName { get; set; }

        [Display(Name = "Last Name")]
        string PrimaryContactLastName { get; set; }

        [Display(Name = "Mobile phone Number")]
        [Phone]
        string PrimaryContactPhoneNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        string PrimaryContactEmail { get; set; }
    }
}
