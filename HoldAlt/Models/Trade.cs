using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoldAlt.Models
{
    public class Trade
    {
        public int TradeID { get; set; }
        public int CoinID { get; set; }
        [Display(Name = "Trade Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TradeDate { get; set; }
        public int ExchangeID { get; set; }
        [Display(Name = "Coins Traded")]
        public float AmountTraded { get; set; }
        [Display(Name = "Bought")]
        public bool BoughtYes { get; set; }
        [Display(Name = "BTC per Coin")]
        public float PriceBTC { get; set; }
        [Display(Name = "US $ per Coin")]
        public float PriceUSD { get; set; }
        [Display(Name = "Commission Amount")]
        public float Commission { get; set; }
        [Display(Name = "Commission Units")]
        public int CommissionCoinID { get; set; }
        public string User { get; set; }

    }
}