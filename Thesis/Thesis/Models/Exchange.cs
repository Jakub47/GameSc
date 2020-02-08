using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class Exchange
    {
        //Keys
        public int ExchangeId { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }


        public virtual ICollection<Game> ProposeGames { get; set; }

        //Validate
        [StringLength(75, ErrorMessage = "Imię nie może być dłuższe niz 75 znaków")]
        [DisplayName("Imię")]
        public string FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Nazwisko nie może być dłuższe niz 100 znaków")]
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }

        [StringLength(100, ErrorMessage = "Nazwa ulica nie może być dłuższe niz 100 znaków")]
        [DisplayName("Ulica")]
        public string Street { get; set; }

        [StringLength(100, ErrorMessage = "Nazwa miasta nie może być dłuższe niz 100 znaków")]
        [DisplayName("Miasto")]
        public string Town { get; set; }

        [RegularExpression(@"\d{2}-\d{3}", ErrorMessage = "Nie poprawna forma kodu")]
        [DisplayName("Kod pocztowy")]
        public string ZipCode { get; set; }

        [RegularExpression(@"^(?:\d{8}|00\d{10}|\+\d{2}\d{8})$", ErrorMessage = "Nie poprawna forma numeru")]
        [DisplayName("Numer telefonu")]
        public string TelephoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Niepoprawny adres email")]
        [DisplayName("Email")]
        public string Email { get; set; }


        //Not validate
        [StringLength(50, ErrorMessage = " Długość Informacji nie może być dłuższe niz 50 znaków")]
        [DisplayName("Dodatkowe informacje")]
        public string Info { get; set; }

        public DateTime DateOfInsert { get; set; }

        public Accepted Accepted { get; set; }

        //Lists
        public List<PositionOfExchange> PositionOfExchange { get; set; }
    }

    public enum Accepted
    {
        Yes,
        No,
        Thinking
    }
}