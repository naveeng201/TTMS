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
    public class ProductsInventoriesController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: ProductsInventories
        public ActionResult Index()
        {
            var productsInventories = db.productsinventories.Include(p => p.product);
            return View(productsInventories.ToList());
        }

        // GET: ProductsInventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productsinventory productsInventory = db.productsinventories.Find(id);
            if (productsInventory == null)
            {
                return HttpNotFound();
            }
            return View(productsInventory);
        }

        // GET: ProductsInventories/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
            return View();
        }

        // POST: ProductsInventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,Quantity,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] productsinventory productsInventory)
        {
            if (ModelState.IsValid)
            {
                db.productsinventories.Add(productsInventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.products, "ID", "Name", productsInventory.ProductID);
            return View(productsInventory);
        }

        // GET: ProductsInventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productsinventory productsInventory = db.productsinventories.Find(id);
            if (productsInventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name", productsInventory.ProductID);
            return View(productsInventory);
        }

        // POST: ProductsInventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,Quantity,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] productsinventory productsInventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsInventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name", productsInventory.ProductID);
            return View(productsInventory);
        }

        // GET: ProductsInventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productsinventory productsInventory = db.productsinventories.Find(id);
            if (productsInventory == null)
            {
                return HttpNotFound();
            }
            return View(productsInventory);
        }

        // POST: ProductsInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            productsinventory productsInventory = db.productsinventories.Find(id);
            db.productsinventories.Remove(productsInventory);
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
