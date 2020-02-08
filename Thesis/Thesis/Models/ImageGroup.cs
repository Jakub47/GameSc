using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class ImageGroup
    {
        public int ImageGroupId { get; set; }
        public string FilePath { get; set; }

        //FK
        public int GroupId { get; set; }

        //navigation property
        public virtual Group Group { get; set; }
    }
}