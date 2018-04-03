using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoldAlt.ViewModels;
using HoldAlt.Models;
using HoldAlt.Classes;
using System.Net;

namespace HoldAlt.Controllers
{
    public class SummaryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Summary
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            SummaryVM sVM = new SummaryVM();
            sVM.TotalCValueToday = 0;
            sVM.TotalSValueToday = 0;

            sVM.Coins = new List<HoldingVM>();
            sVM.Stocks = new List<StockHoldingVM>();

            // Build the Coins model
            List<HoldingVM> cVM = new List<HoldingVM>();
            // Build VM List
            var holdings = db.Holdings.ToList();
            var trades = db.Trades.ToList();
            //var altCoins = db.AltCoins.ToList();          // Don't need because of globals list
            //var exch = db.Exchanges.ToList();             // Don't need to get Exchanges table since not displayed in Summary
            foreach (Holding h in holdings)
            {
                AltCoin a = Globals.lAltCoins.FirstOrDefault(x => x.AltCoinID == h.CoinID);
               // Exchange e = exch.FirstOrDefault(y => y.ExchangeID == h.ExchangeID);
                HoldingVM hv = new HoldingVM();
                hv.HoldingVMID = h.HoldingID;
                hv.CoinName = a.CoinName;
                hv.CoinSymbol = a.CoinSymbol;
                hv.CoinAmount = h.CoinAmount;
                //hv.ExchangeName = e.ExchangeName;
                hv.CoinValueUSD = Convert.ToSingle(Globals.CoinToUSDtoday(a.APICode));
                // Calculate Average Paid
                float coinTraded = 0, amtPaid = 0, coinT;
                float myCostUSD = 0;
                foreach (Trade t in trades)
                {
                    if (t.CoinID == h.CoinID)
                    {
                        coinT = t.AmountTraded - t.Commission;
                        coinTraded += coinT;
                        amtPaid += t.PriceUSD * coinT;
                        myCostUSD += t.AmountTraded * t.PriceUSD;
                    }
                }
                if (coinTraded > 0)
                    hv.AveragePaid = (amtPaid / coinTraded).ToString();
                else
                    hv.AveragePaid = "";
                if (myCostUSD == 0)
                    hv.CostUSD = "";
                else
                    hv.CostUSD = myCostUSD.ToString();

                /*
                hv.CoinValueBTC = Convert.ToSingle(Globals.CoinToUSDtoday(a.APICode)) / Convert.ToSingle(Globals.CoinToUSDtoday("bitcoin"));
                hv.Percent1 = Globals.PctChange(a.APICode, 1);
                hv.Percent7 = Globals.PctChange(a.APICode, 7);
                */
                hv.PriceToday = hv.CoinAmount * hv.CoinValueUSD;
                sVM.TotalCValueToday += hv.PriceToday;

                sVM.Coins.Add(hv);

            }


            // Build the Stocks model
            List<StockHoldingVM> tVM = new List<StockHoldingVM>();
            string sPrice = "0";
            string sChange = "0";
            string sVol = "0";
            string sAvgVol = "0";

            var stocks = db.StockHoldings.ToList();
            var stkTrades = db.StockTrades.ToList(); 
            foreach (StockHolding sh in stocks)
            {
                StockHoldingVM shVM = new StockHoldingVM();
                shVM.Ticker = sh.Ticker;
                shVM.NumberShares = sh.NumberShares;
                shVM.Account = sh.StockAccount;

                // Calculate average of price paid including commission
                List<StockTrade> lST = new List<StockTrade>();
                lST = stkTrades.Where(x => x.Ticker == sh.Ticker).ToList();
                float cost = 0;
                float nShares = 0;
                foreach (StockTrade st in lST)
                {
                    cost += st.Price * st.AmountTraded;// + st.Commission*2;
                    nShares += st.AmountTraded;
                }
                shVM.PricePaid = cost / nShares;

                shVM.Commission = 10;

                // Get price of stock today using Yahoo
                using (WebClient web = new WebClient())
                {
                    string strURL = string.Format("https://finance.yahoo.com/quote/" + sh.Ticker + "?p=" + sh.Ticker);
                    try
                    {
                        string data = web.DownloadString(strURL);
                        int iStart, iEnd;
                        iStart = data.IndexOf("<!-- react-text: 36 -->") + "<!-- react-text: 36 -->".Length;
                        iEnd = data.IndexOf("<!", iStart);
                        sPrice = data.Substring(iStart, iEnd - iStart);
                        iStart = data.IndexOf("quote-market-notice") - 100;
                        iEnd = data.IndexOf("<!-- react-text: 39 -->", iStart) + "<!-- react-text: 39 -->".Length;
                        if (iEnd < 1000)
                            iEnd = data.IndexOf("<!-- react-text: 38 -->", iStart) + "<!-- react-text: 38 -->".Length;
                        iStart = iEnd;
                        iEnd = data.IndexOf(" (", iStart);
                        sChange = data.Substring(iStart, iEnd - iStart);

                        iStart = data.IndexOf("<!-- react-text: 74 -->") + "<!-- react-text: 74 -->".Length;
                        iEnd = data.IndexOf("<!", iStart);
                        sVol = data.Substring(iStart, iEnd - iStart);
                        iStart = data.IndexOf("<!-- react-text: 80 -->") + "<!-- react-text: 80 -->".Length;
                        iEnd = data.IndexOf("<!", iStart);
                        sAvgVol = data.Substring(iStart, iEnd - iStart);

                    }
                    catch
                    {

                    }
                }

                if (Globals.IsNumeric(sPrice))
                    shVM.PriceToday = Convert.ToSingle(sPrice);
                else
                    shVM.PriceToday = 0;
                if (Globals.IsNumeric(sChange))
                    shVM.Change = Convert.ToSingle(sChange);
                else
                    shVM.Change = 0;
                shVM.ValueToday = nShares * shVM.PriceToday;

                if (Globals.IsNumeric(sVol))
                    shVM.Volume = Convert.ToSingle(sVol);
                else
                    shVM.Volume = 0;

                if (Globals.IsNumeric(sAvgVol))
                    shVM.AvgVol = Convert.ToSingle(sAvgVol);
                else
                    shVM.AvgVol = 0;

                sVM.TotalSValueToday += shVM.ValueToday;

                sVM.Stocks.Add(shVM);
            }


            // DJI and NASDAQ
            using (WebClient web = new WebClient())
            {
                string strURL = string.Format("https://finance.yahoo.com/quote/^DJI?p=^DJI");
                try
                {
                    string data = web.DownloadString(strURL);
                    int iStart, iEnd;
                    iStart = data.IndexOf("<!-- react-text: 36 -->") + "<!-- react-text: 36 -->".Length;
                    iEnd = data.IndexOf("<!", iStart);
                    sPrice = data.Substring(iStart, iEnd - iStart);
                    iStart = data.IndexOf("quote-market-notice") - 100;
                    iEnd = data.IndexOf("<!-- react-text: 39 -->", iStart) + "<!-- react-text: 39 -->".Length;
                    if (iEnd < 1000)
                        iEnd = data.IndexOf("<!-- react-text: 38 -->", iStart) + "<!-- react-text: 38 -->".Length;
                    iStart = iEnd;
                    iEnd = data.IndexOf(" (", iStart);
                    sChange = data.Substring(iStart, iEnd - iStart);
                    sVM.DJI = sPrice;
                    sVM.DJIC = Convert.ToSingle(sChange);

                    strURL = string.Format("https://finance.yahoo.com/quote/^IXIC?p=^IXIC");
                    data = web.DownloadString(strURL);
                    iStart = data.IndexOf("<!-- react-text: 36 -->") + "<!-- react-text: 36 -->".Length;
                    iEnd = data.IndexOf("<!", iStart);
                    sPrice = data.Substring(iStart, iEnd - iStart);
                    iStart = data.IndexOf("quote-market-notice") - 100;
                    iEnd = data.IndexOf("<!-- react-text: 39 -->", iStart) + "<!-- react-text: 39 -->".Length;
                    if (iEnd < 1000)
                        iEnd = data.IndexOf("<!-- react-text: 38 -->", iStart) + "<!-- react-text: 38 -->".Length;
                    iStart = iEnd;
                    iEnd = data.IndexOf(" (", iStart);
                    sChange = data.Substring(iStart, iEnd - iStart);
                    sVM.NAS = sPrice;
                    sVM.NASC = Convert.ToSingle(sChange);
                }
                catch
                {
                    sVM.DJI = "Error";
                    sVM.DJIC = -1;
                    sVM.NAS = "Error";
                    sVM.NASC = -1;
                }

            }
            return View(sVM);
        }
    }
}