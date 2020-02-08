using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class GamingEvent
    {
        public int GamingEventId { get; set; }
        public int CategoryEventId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser Publisher { get; set; }

        [Required(ErrorMessage = "Proszę dodać tytuł wydarzenia")]
        [StringLength(75, ErrorMessage = "Tytul nie moze miec wiecej niz 150 znakow")]
        [DisplayName("Tytuł")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Proszę dodać opis wydarzenia")]
        [StringLength(1000, ErrorMessage = "Tytul nie moze miec wiecej niz 1000 znakow")]
        [DisplayName("Opis")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        //TODO: Dodać date
        public DateTime DateOfEvent { get; set; }
        public int MaxNumberOfPeople { get; set; }
        public int CurrentNumberOfPeople { get; set; }

        [DisplayName("Główny obrazek")]
        public string MainPicture { get; set; }

        public virtual CategoryEvent CategoryEvent { get; set; }
        public virtual ICollection<ImageEvent> ImageEvent { get; set; }
        public virtual ICollection<CommentEvent> Comments { get; set; }

        [InverseProperty("AttendGamingEvent")]
        public virtual ICollection<ApplicationUser> UsersToAttend { get; set; }
    }
}