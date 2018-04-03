using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoldAlt.ViewModels
{
    public class TradeVM
    {
        public int TradeID { get; set; }
        [Display(Name = "Coin")]
        public string CoinName { get; set; }
        [Display(Name = "Trade Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TradeDate { get; set; }
        [Display(Name = "Exchange")]
        public string ExchangeName { get; set; }
        [Display(Name = "Coins Traded")]
        public float AmountTraded { get; set; }
        [Display(Name = "Bought")]
        public bool BoughtYes { get; set; }
        [Display(Name = "BTC per Coin")]
        public float PriceBTC { get; set; }
        [Display(Name = "US $ per Coin")]
        public float PriceUSD { get; set; }
        [Display(Name = "Commission Amount")]
        public float CommissionAmount { get; set; }
        [Display(Name = "Commission Units")]
        public string CommissionCoin { get; set; }
    }
}