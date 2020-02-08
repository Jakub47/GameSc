using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public int CategoryGameId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        [Required(ErrorMessage = "Proszę dodać tytuł")]
        [StringLength(150, ErrorMessage = "Tytul nie moze miec wiecej niz 75 znakow")]
        [DisplayName("Tytuł")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Proszę dodać Wydawce")]
        [StringLength(150, ErrorMessage = "Nazwa w polu Wydawca nie moze miec wiecej niz 75 znakow")]
        [DisplayName("Wydawca")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić opis gry")]
        [StringLength(1000, ErrorMessage = "Opis nie może być dłuższy niż 500 znaków")]
        [DisplayName("Opis Gry")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Proszę wprowadzić opis skrócony gry")]
        [StringLength(150, ErrorMessage = "Opis skrócony nie może być dłuższy niż 50 znaków")]
        [DisplayName("Opis skrócony gry")]
        public string ShortDescription { get; set; }


        public bool Hidden { get; set; }

        [DisplayName("Główny obrazek")]
        public string MainPicture { get; set; }

        [DisplayName("Gry do wymiany")]
        public string GamesForExchange { get; set; }

        public virtual CategoryGame Category { get; set; }
        public virtual ICollection<ImageGame> ImageGames { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        [NotMapped]
        public bool IsInObserver { get; set; }
    }
}