using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Helpers.AI.SentimentalAnalisys
{
    public class ModelInput
    {
        [ColumnName("sample"), LoadColumn(0)]
        public string Sample { get; set; }


        [ColumnName("label"), LoadColumn(1)]
        public bool Label { get; set; }
    }
}