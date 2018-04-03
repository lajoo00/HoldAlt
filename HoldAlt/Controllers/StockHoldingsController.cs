using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoldAlt.Models;
using HoldAlt.ViewModels;
using HoldAlt.Classes;

namespace HoldAlt.Controllers
{
    public class StockHoldingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockHoldings
        public ActionResult Index()
        {
            // Build the StockHoldingVM Model
            float total = 0;
            List<StockHoldingVM> lSH = new List<StockHoldingVM>();
            var stkHoldings = db.StockHoldings.ToList();
            var stkTrades = db.StockTrades.ToList();
            foreach (StockHolding sh in stkHoldings)
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
                string sPrice = "0";
                string sChange = "0";
                string sVol = "0";
                string sAvgVol = "0";
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

                total += shVM.ValueToday;

                lSH.Add(shVM);
            }

            ViewBag.Value = total;
            return View(lSH);
        }

        // GET: StockHoldings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockHolding stockHolding = db.StockHoldings.Find(id);
            if (stockHolding == null)
            {
                return HttpNotFound();
            }
            return View(stockHolding);
        }

        // GET: StockHoldings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockHoldings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockHoldingID,Ticker,NumberShares,StockAccount,User")] StockHolding stockHolding)
        {
            if (ModelState.IsValid)
            {
                db.StockHoldings.Add(stockHolding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stockHolding);
        }

        // GET: StockHoldings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockHolding stockHolding = db.StockHoldings.Find(id);
            if (stockHolding == null)
            {
                return HttpNotFound();
            }
            return View(stockHolding);
        }

        // POST: StockHoldings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockHoldingID,Ticker,NumberShares,StockAccount,User")] StockHolding stockHolding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockHolding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stockHolding);
        }

        // GET: StockHoldings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockHolding stockHolding = db.StockHoldings.Find(id);
            if (stockHolding == null)
            {
                return HttpNotFound();
            }
            return View(stockHolding);
        }

        // POST: StockHoldings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockHolding stockHolding = db.StockHoldings.Find(id);
            db.StockHoldings.Remove(stockHolding);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
