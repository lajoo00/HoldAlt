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
    public class HoldingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Holdings
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Build VM List
            var holdings = db.Holdings.ToList();
            var trades = db.Trades.ToList();
            List<HoldingVM> hVMList = new List<HoldingVM>();
            foreach (Holding h in holdings)
            {
                AltCoin a = db.AltCoins.FirstOrDefault(x => x.AltCoinID == h.CoinID);
                Exchange e = db.Exchanges.FirstOrDefault(y => y.ExchangeID == h.ExchangeID);
                HoldingVM hv = new HoldingVM();
                hv.HoldingVMID = h.HoldingID;
                hv.CoinName = a.CoinName;
                hv.CoinSymbol = a.CoinSymbol;
                hv.CoinAmount = h.CoinAmount;
                hv.ExchangeName = e.ExchangeName;
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

                hv.CoinValueBTC = Convert.ToSingle(Globals.CoinToUSDtoday(a.APICode)) / Convert.ToSingle(Globals.CoinToUSDtoday("bitcoin"));
                hv.PriceToday = hv.CoinAmount * hv.CoinValueUSD;
                hv.Percent1 = Globals.PctChange(a.APICode, 1);
                hv.Percent7 = Globals.PctChange(a.APICode, 7);
                hVMList.Add(hv);

            }
            return View(hVMList);
        }

        // GET: Holdings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holding holding = db.Holdings.Find(id);
            if (holding == null)
            {
                return HttpNotFound();
            }
            return View(holding);
        }

        // GET: Holdings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Holdings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HoldingID,CoinID,CoinAmount,ExchangeID")] Holding holding)
        {
            if (ModelState.IsValid)
            {
                db.Holdings.Add(holding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(holding);
        }

        // GET: Holdings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holding holding = db.Holdings.Find(id);
            if (holding == null)
            {
                return HttpNotFound();
            }
            return View(holding);
        }

        // POST: Holdings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HoldingID,CoinID,CoinAmount,ExchangeID")] Holding holding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(holding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(holding);
        }

        // GET: Holdings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Holding holding = db.Holdings.Find(id);
            if (holding == null)
            {
                return HttpNotFound();
            }
            return View(holding);
        }

        // POST: Holdings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Holding holding = db.Holdings.Find(id);
            db.Holdings.Remove(holding);
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
