using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Models
{
    public class PositionOfExchange
    {
        public int PositionOfExchangeId { get; set; }
        public int ExchangeId { get; set; }
        public int GameId { get; set; }


        public virtual Game Game { get; set; }
        public virtual Exchange Exchange { get; set; }
    }
}