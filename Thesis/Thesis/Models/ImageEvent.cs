using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class ImageEvent
    {
        public int ImageEventId { get; set; }
        public string FilePath { get; set; }

        //FK
        public int GamingEventId { get; set; }

        //navigation property
        public virtual GamingEvent GamingEvent { get; set; }
    }
}