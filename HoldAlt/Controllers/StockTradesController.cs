        using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoldAlt.Models;

namespace HoldAlt.Controllers
{
    public class StockTradesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockTrades
        public ActionResult Index()
        {
            return View(db.StockTrades.ToList());
        }

        // GET: StockTrades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockTrade stockTrade = db.StockTrades.Find(id);
            if (stockTrade == null)
            {
                return HttpNotFound();
            }
            return View(stockTrade);
        }

        // GET: StockTrades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockTrades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockTradeID,Ticker,TradeDate,Account,AmountTraded,BoughtYes,Price,Commission,User")] StockTrade stockTrade)
        {
            if (ModelState.IsValid)
            {
                db.StockTrades.Add(stockTrade);

                // Add to stock holdings
                StockHolding h = new StockHolding();
                h = db.StockHoldings.FirstOrDefault(x => x.Ticker == stockTrade.Ticker && x.StockAccount == stockTrade.Account);
                if (h != null)
                {
                    // Found stock in this Stock Account
                    h.NumberShares += stockTrade.AmountTraded;
                    db.Entry(h).State = EntityState.Modified;
                }
                else
                {
                    // Add new holding
                    h.Ticker = stockTrade.Ticker;
                    h.NumberShares = stockTrade.AmountTraded;
                    h.StockAccount = stockTrade.Account;
                    db.StockHoldings.Add(h);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(stockTrade);
        }

        // GET: StockTrades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockTrade stockTrade = db.StockTrades.Find(id);
            if (stockTrade == null)
            {
                return HttpNotFound();
            }
            return View(stockTrade);
        }

        // POST: StockTrades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockTradeID,Ticker,TradeDate,Account,AmountTraded,BoughtYes,Price,Commission,User")] StockTrade stockTrade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockTrade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stockTrade);
        }

        // GET: StockTrades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockTrade stockTrade = db.StockTrades.Find(id);
            if (stockTrade == null)
            {
                return HttpNotFound();
            }
            return View(stockTrade);
        }

        // POST: StockTrades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockTrade stockTrade = db.StockTrades.Find(id);
            db.StockTrades.Remove(stockTrade);
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
