using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoldAlt.Models
{
    public class Holding
    {
        public int HoldingID { get; set; }
        public int CoinID { get; set; }
        public float CoinAmount { get; set; }
        public int ExchangeID { get; set; }
        public string User { get; set; }

    }
}