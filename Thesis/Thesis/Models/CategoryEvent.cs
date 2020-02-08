using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class CategoryEvent
    {
        public int CategoryEventId { get; set; }

        [Required(ErrorMessage = "Wprowadz nazwe kategorii")]
        [StringLength(100, ErrorMessage = "Nazwa nie moze miec wiecej niz 50 znakow")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wprowadz opis kategorii")]
        [StringLength(150, ErrorMessage = "opis nie moze miec wiecej niz 50 znakow")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Wprowadz obraz dla kategorii")]
        public string Picture { get; set; }

        [Required(ErrorMessage = "Wprowadz ikone dla kategorii")]
        public string Icon { get; set; }

        public virtual ICollection<GamingEvent> GamingEvents { get; set; }
    }
}