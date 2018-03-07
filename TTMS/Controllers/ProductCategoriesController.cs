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
    public class ProductCategoriesController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: productcategories
        public ActionResult Index()
        {
            return View(db.productcategories.ToList());
        }

        // GET: productcategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productcategory productCategory = db.productcategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: productcategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: productcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] productcategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.productcategories.Add(productCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: productcategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productcategory productCategory = db.productcategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: productcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] productcategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategory);
        }

        // GET: productcategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productcategory productCategory = db.productcategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: productcategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            productcategory productCategory = db.productcategories.Find(id);
            db.productcategories.Remove(productCategory);
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
