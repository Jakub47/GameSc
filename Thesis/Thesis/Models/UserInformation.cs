using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class UserInformation
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Adress { get; set; }

        public string Town { get; set; }

        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Błędny format kodu pocztowego.")]
        public string PostCode { get; set; }

        [RegularExpression(@"(\+\d{2})*[\d\s-]+", ErrorMessage = "Błędny format numeru telefonu.")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Błędny format adresu e-mail.")]
        public string Email { get; set; }

        public UserInformation()
        {

        }

        public UserInformation(string email)
        {
            Email = email;
        }
    }
}