using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoldAlt.ViewModels
{
    public class HoldingVM
    {
        public int HoldingVMID { get; set; }
        [Display(Name = "Coin")]
        public string CoinName { get; set; }
        [Display(Name = "Symbol")]
        public string CoinSymbol { get; set; }
        [Display(Name = "API Code")]
        public string APICode { get; set; }

        [Display(Name = "Coins Held")]
        public float CoinAmount { get; set; }

        [Display(Name = "$ per Coin")]
        public float CoinValueUSD { get; set; }
        [Display(Name = "Average Paid")]
        public string AveragePaid { get; set; }
        [Display(Name = "BTC per Coin")]
        public float CoinValueBTC { get; set; }
        [Display(Name = "Cost $")]
        public string CostUSD { get; set; }
        [Display(Name = "Today's Value $")]
        public float PriceToday { get; set; }

        [Display(Name = "% 1 Day")]
        public float Percent1 { get; set; }
        [Display(Name = "% 7 Day")]
        public float Percent7 { get; set; }
        [Display(Name = "Exchange")]
        public string ExchangeName { get; set; }
    }
}