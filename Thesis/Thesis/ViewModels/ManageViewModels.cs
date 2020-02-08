using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Thesis.Models;

namespace Thesis.ViewModels
{
    public class ManageCredentialsViewModel
    {
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
        public Thesis.Controllers.ManageController.ManageMessageId? Message { get; set; }
        public UserInformation UserInformation { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Obecne hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nowe hasło jest wymagane")]
        [StringLength(100, ErrorMessage = "Hasło {0} musi mieć przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Potwierdzone hasło nie jest takie same jak nowe hasło")]
        public string ConfirmPassword { get; set; }
    }
}