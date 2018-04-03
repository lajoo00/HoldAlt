using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HoldAlt.ViewModels
{
    public class StockHoldingVM
    {
        public int StockHoldingVMID { get; set; }
        [Display(Name = "Ticker")]
        public string Ticker { get; set; }
        [Display(Name = "Account")]
        public string Account { get; set; }
        [Display(Name = "Shares")]
        public float NumberShares { get; set; }
        [Display(Name = "Cost")]
        public float PricePaid { get; set; }
        [Display(Name = "Commission")]
        public float Commission { get; set; }
        [Display(Name = "Price Today")]
        public float PriceToday { get; set; }
        [Display(Name = "Change")]
        public float Change { get; set; }
        [Display(Name = "Volume")]
        public float Volume { get; set; }
        [Display(Name = "Avg Vol")]
        public float AvgVol { get; set; }
        [Display(Name = "Value Today")]
        public float ValueToday { get; set; }
    }
}