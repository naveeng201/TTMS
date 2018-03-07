using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTMS.Models;

namespace TTMS.Controllers
{
    [Authorize]
    public class BrandsMastersController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: BrandsMasters
        public ActionResult Index()
        {
            return View(db.brandsmasters.ToList());
        }

        // GET: BrandsMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brandsmaster brandsMaster = db.brandsmasters.Find(id);
            if (brandsMaster == null)
            {
                return HttpNotFound();
            }
            return View(brandsMaster);
        }

        // GET: BrandsMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BrandName,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] brandsmaster brandsMaster)
        {
            if (ModelState.IsValid)
            {
                db.brandsmasters.Add(brandsMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brandsMaster);
        }

        // GET: BrandsMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brandsmaster brandsMaster = db.brandsmasters.Find(id);
            if (brandsMaster == null)
            {
                return HttpNotFound();
            }
            return View(brandsMaster);
        }

        // POST: BrandsMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BrandName,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] brandsmaster brandsMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brandsMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brandsMaster);
        }

        // GET: BrandsMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            brandsmaster brandsMaster = db.brandsmasters.Find(id);
            if (brandsMaster == null)
            {
                return HttpNotFound();
            }
            return View(brandsMaster);
        }

        // POST: BrandsMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            brandsmaster brandsMaster = db.brandsmasters.Find(id);
            db.brandsmasters.Remove(brandsMaster);
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
