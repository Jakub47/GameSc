using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Helpers.AI.SentimentalAnalisys
{
    public class SentimentalInterpreter
    {
        public bool IsHappy(string sentence)
        {
            var input = new ModelInput() { Sample = sentence };
            // Load model and predict output of sample data
            ModelOutput result = ConsumeModel.Predict(input);
            return result.Prediction;
        }
    }
}