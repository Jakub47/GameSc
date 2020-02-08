using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Proszę dodać tytuł")]
        [StringLength(75, ErrorMessage = "Tytul nie moze miec wiecej niz 75 znakow")]
        [DisplayName("Tytuł")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Proszę dodać informacje o grupie")]
        [StringLength(300, ErrorMessage = "Opis nie moze miec wiecej niz 300 znakow")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Informacje")]
        public string Information { get; set; }

        public string Image { get; set; }

        public virtual ICollection<GamingEvent> GamingEventsOfGroup { get; set; }
        public virtual ICollection<ImageGroup> ImageGroup { get; set; }
    }
}