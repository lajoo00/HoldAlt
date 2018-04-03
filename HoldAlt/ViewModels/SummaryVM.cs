using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HoldAlt.ViewModels;

namespace HoldAlt.ViewModels
{
    public class SummaryVM
    {
        public List<HoldingVM> Coins { get; set; }
        public List<StockHoldingVM> Stocks { get; set; }
        [Display(Name = "Total Today")]
        public float TotalCValueToday { get; set; }
        public float TotalSValueToday { get; set; }
        public string DJI { get; set; }
        public float DJIC { get; set; }
        public string NAS { get; set; }
        public float NASC { get; set; }

    }
}