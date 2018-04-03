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

namespace HoldAlt.Controllers
{
    public class TradesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trades
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            List<Trade> tList = new List<Trade>();
            List<TradeVM> tVMList = new List<TradeVM>();
            tList = db.Trades.ToList();
            foreach (Trade tr in tList)
            {
                TradeVM trVM = new TradeVM();
                AltCoin a = db.AltCoins.FirstOrDefault(x => x.AltCoinID == tr.CoinID);
                Exchange e = db.Exchanges.FirstOrDefault(y => y.ExchangeID == tr.ExchangeID);

                trVM.TradeID = tr.TradeID;
                trVM.CoinName = a.CoinName;
                trVM.TradeDate = tr.TradeDate;
                trVM.ExchangeName = e.ExchangeName;
                trVM.AmountTraded = tr.AmountTraded;
                trVM.BoughtYes = tr.BoughtYes;
                trVM.PriceBTC = tr.PriceBTC;
                trVM.PriceUSD = tr.PriceUSD;
                trVM.CommissionAmount = tr.Commission;
                a = db.AltCoins.FirstOrDefault(x => x.AltCoinID == tr.CommissionCoinID);
                trVM.CommissionCoin = a.CoinName;

                tVMList.Add(trVM);
            }

            return View(tVMList);
        }

        // GET: Trades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        // GET: Trades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TradeID,CoinID,TradeDate,ExchangeID,AmountTraded,BoughtYes,PriceBTC,PriceUSD,Commission,CommissionCoinID")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Trades.Add(trade);

                // Add to Holdings
                Holding h = new Holding();
                h = db.Holdings.FirstOrDefault(x => x.CoinID == trade.CoinID && x.ExchangeID == trade.ExchangeID);
                if (h != null)
                {
                    // Found coin at same Exchange
                    h.CoinAmount += trade.AmountTraded;
                    db.Entry(h).State = EntityState.Modified;
                }
                else
                {
                    // Add new holding 
                    h.CoinID = trade.CoinID;
                    h.CoinAmount = trade.AmountTraded;
                    h.ExchangeID = trade.ExchangeID;
                    db.Holdings.Add(h);
                }


                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(trade);
        }

        // GET: Trades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        // POST: Trades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TradeID,CoinID,TradeDate,ExchangeID,AmountTraded,BoughtYes,PriceBTC,PriceUSD,Commission,CommissionCoinID")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trade);
        }

        // GET: Trades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        // POST: Trades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trade trade = db.Trades.Find(id);
            db.Trades.Remove(trade);
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
