using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppHotelManagement.Models;

namespace WebAppHotelManagement.Controllers
{
    public class MonthlyRsController : Controller
    {
        private HotelDBEntities db = new HotelDBEntities();

        // GET: MonthlyR
        public ActionResult Index()
        {
            return View(db.MonthlyRs.ToList());
        }

        // GET: MonthlyRs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyR monthlyR = db.MonthlyRs.Find(id);
            if (monthlyR == null)
            {
                return HttpNotFound();
            }
            return View(monthlyR);
        }

        // GET: MonthlyRs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MonthlyRs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,MonthS,MonthE,Total")] MonthlyR monthlyR)
        {
            if (ModelState.IsValid)
            {
                db.MonthlyRs.Add(monthlyR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(monthlyR);
        }

        // GET: MonthlyRs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyR monthlyR = db.MonthlyRs.Find(id);
            if (monthlyR == null)
            {
                return HttpNotFound();
            }
            return View(monthlyR);
        }

        // POST: MonthlyRs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,MonthS,MonthE,Total")] MonthlyR monthlyR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(monthlyR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(monthlyR);
        }

        // GET: MonthlyRs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MonthlyR monthlyR = db.MonthlyRs.Find(id);
            if (monthlyR == null)
            {
                return HttpNotFound();
            }
            return View(monthlyR);
        }

        // POST: MonthlyRs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MonthlyR monthlyR = db.MonthlyRs.Find(id);
            db.MonthlyRs.Remove(monthlyR);
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
