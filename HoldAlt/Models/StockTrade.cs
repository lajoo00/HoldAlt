using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoldAlt.Models
{
    public class StockTrade
    {
        public int StockTradeID { get; set; }
        [Display(Name = "Ticker")]
        public string Ticker { get; set; }
        [Display(Name = "Trade Date")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TradeDate { get; set; }
        [Display(Name = "Account")]
        public string Account { get; set; }
        [Display(Name = "Shares Traded")]
        public float AmountTraded { get; set; }
        [Display(Name = "Bought")]
        public bool BoughtYes { get; set; }
        [Display(Name = "Share Price")]
        public float Price { get; set; }
        [Display(Name = "Commission")]
        public float Commission { get; set; }
        public string User { get; set; }

    }
}