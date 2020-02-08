using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Thesis.ViewModels
{
   
        public class LoginViewModel
        {
            [Required(ErrorMessage = "Musisz wprowadzić e-mail")]
            [EmailAddress(ErrorMessage = "Niepoprawny format e-maila")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Musisz wprowadzić hasło")]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [Display(Name = "Zapamiętaj mnie")]
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Musisz wprowadzić e-mail")]
            [EmailAddress(ErrorMessage = "Niepoprawny format e-maila")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Musisz wprowadzić swój nick")]
            public string NickName { get; set; }

            public string IsPersonOnImage { get; set; } = "no";

            [NotMapped]
            public HttpPostedFileBase ImageFile { get; set; }

        [Required(ErrorMessage = "Musisz wprowadzić hasło")]
            [StringLength(30, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = " Hasło ")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdz Hasło ")]
            [Compare("Password", ErrorMessage = "Hasło i potwierdzenie hasła nie pasują do siebie.")]
            public string ConfirmPassword { get; set; }
        }
    
}