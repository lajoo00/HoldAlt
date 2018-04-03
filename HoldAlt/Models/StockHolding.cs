using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoldAlt.Models
{
    public class StockHolding
    {
        public int StockHoldingID { get; set; }
        public string Ticker { get; set; }
        public float NumberShares { get; set; }
        public string StockAccount{ get; set; }
        public string User { get; set; }
    }
}