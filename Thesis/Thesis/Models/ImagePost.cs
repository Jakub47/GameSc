using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class ImagePost
    {
        public int ImagePostId { get; set; }
        public string FilePath { get; set; }

        //FK
        public int PostId { get; set; }

        //navigation property
        public virtual Post Post { get; set; }
    }
}