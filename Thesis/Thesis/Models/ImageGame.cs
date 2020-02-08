using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class ImageGame
    {
        public int ImageGameId { get; set; }
        public string FilePath { get; set; }

        //FK
        public int GameId { get; set; }

        //navigation property
        public virtual Game Game { get; set; }
    }
}