using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoldAlt.Models
{
    public class AltCoin
    {
        public int AltCoinID { get; set; }
        public string CoinName { get; set; }
        public string CoinSymbol { get; set; }
        public string APICode { get; set; }
    }
}