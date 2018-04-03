﻿using System;
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
    public class ExchangesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Exchanges
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Exchanges.ToList());
        }

        // GET: Exchanges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exchange exchange = db.Exchanges.Find(id);
            if (exchange == null)
            {
                return HttpNotFound();
            }
            return View(exchange);
        }

        // GET: Exchanges/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exchanges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExchangeID,ExchangeName,URL")] Exchange exchange)
        {
            if (ModelState.IsValid)
            {
                db.Exchanges.Add(exchange);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exchange);
        }

        // GET: Exchanges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exchange exchange = db.Exchanges.Find(id);
            if (exchange == null)
            {
                return HttpNotFound();
            }
            return View(exchange);
        }

        // POST: Exchanges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExchangeID,ExchangeName,URL")] Exchange exchange)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exchange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exchange);
        }

        // GET: Exchanges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exchange exchange = db.Exchanges.Find(id);
            if (exchange == null)
            {
                return HttpNotFound();
            }
            return View(exchange);
        }

        // POST: Exchanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exchange exchange = db.Exchanges.Find(id);
            db.Exchanges.Remove(exchange);
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
