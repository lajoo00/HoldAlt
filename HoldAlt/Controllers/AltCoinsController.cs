using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HoldAlt.Models;
using System.IO;
using HoldAlt.Classes;

namespace HoldAlt.Controllers
{
    public class AltCoinsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AltCoins
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (db.AltCoins.ToList().Count == 0)
            {
                // Get the AltCoins
                var strValue = string.Empty;
                var strRequest = (HttpWebRequest)WebRequest.Create("https://api.coinmarketcap.com/v1/ticker/");
                strRequest.Timeout = 5000;
                strRequest.Method = "GET";
                strRequest.ContentType = "text/plain";
                using (var _webResponse = (HttpWebResponse)strRequest.GetResponse())
                {
                    var _webResponseStatus = _webResponse.StatusCode;
                    var _stream = _webResponse.GetResponseStream();
                    using (var _streamReader = new StreamReader(_stream))
                    {
                        strValue = _streamReader.ReadToEnd();
                    }
                }

                // Parse the AltCoins
                if (strValue != string.Empty)
                {
                    //MessageBox.Show(_plainText);
                    // Parse return string for coin name and symbol
                    int index = 0;
                    int index2;
                    int coinIndex = -1;
                    while (true)
                    {
                        index = strValue.IndexOf("name", index);
                        if (index <= 0)
                            break;
                        if (index > 0)
                        {
                            // Get coin name
                            index += 8;
                            index2 = strValue.IndexOf("\"", index);
                            Globals.strCoinName[++coinIndex] = strValue.Substring(index, index2 - index);
                            index = index2;

                            //MessageBox.Show(strCoinName[coinIndex-1]);

                        }
                    }
                    Array.Sort(Globals.strCoinName);

                    coinIndex = -1;
                    for (int i = 0; i < Globals.strCoinName.Length; ++i)
                    {
                        index = strValue.IndexOf(Globals.strCoinName[i]);
                        // Get coin symbol
                        index = strValue.IndexOf("symbol", index) + 10;
                        index2 = strValue.IndexOf("\"", index);
                        Globals.strCoinSymbol[++coinIndex] = strValue.Substring(index, index2 - index);
                        string strName = Globals.strCoinName[coinIndex];
                        string strSymbol = Globals.strCoinSymbol[coinIndex];
                    }
                    coinIndex = -1;
                    for (int i = 0; i < Globals.strCoinName.Length; ++i)
                    {
                        index = strValue.IndexOf(Globals.strCoinName[i]) - 100;
                        if (index < 0)
                            index = 0;
                        // Get coin symbol
                        index = strValue.IndexOf("id", index) + 6;
                        index2 = strValue.IndexOf("\"", index);
                        Globals.strAPICode[++coinIndex] = strValue.Substring(index, index2 - index);
                        string strName = Globals.strCoinName[coinIndex];
                        string strSymbol = Globals.strCoinSymbol[coinIndex];
                        string strAPICode = Globals.strAPICode[coinIndex];
                    }
                }

                // Insert the records
                for (int i = 0; i < Globals.strCoinName.Count(); ++i)
                {
                    AltCoin ac = new AltCoin()
                    {
                        CoinName = Globals.strCoinName[i],
                        CoinSymbol = Globals.strCoinSymbol[i],
                        APICode = Globals.strAPICode[i]
                    };
                    db.AltCoins.Add(ac);
                }
                AltCoin acUS = new AltCoin()
                {
                    CoinName = "US Dollar",
                    CoinSymbol = "US$",
                    APICode = "NA"
                };
                db.AltCoins.Add(acUS);
                db.SaveChanges();

            }
            return View(db.AltCoins.ToList());
        }

        // GET: AltCoins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AltCoin altCoin = db.AltCoins.Find(id);
            if (altCoin == null)
            {
                return HttpNotFound();
            }
            return View(altCoin);
        }

        // GET: AltCoins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AltCoins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AltCoinID,CoinName,CoinSymbol")] AltCoin altCoin)
        {
            if (ModelState.IsValid)
            {
                db.AltCoins.Add(altCoin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(altCoin);
        }

        // GET: AltCoins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AltCoin altCoin = db.AltCoins.Find(id);
            if (altCoin == null)
            {
                return HttpNotFound();
            }
            return View(altCoin);
        }

        // POST: AltCoins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AltCoinID,CoinName,CoinSymbol")] AltCoin altCoin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(altCoin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(altCoin);
        }

        // GET: AltCoins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AltCoin altCoin = db.AltCoins.Find(id);
            if (altCoin == null)
            {
                return HttpNotFound();
            }
            return View(altCoin);
        }

        // POST: AltCoins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AltCoin altCoin = db.AltCoins.Find(id);
            db.AltCoins.Remove(altCoin);
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
