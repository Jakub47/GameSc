using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Helpers.AI.ObjectDetection.YoloParser
{
    public class DimensionsBase
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
}