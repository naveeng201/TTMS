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
    public class SaleProductsController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: SaleProducts
        public ActionResult Index()
        {
            var saleProducts = db.SaleProducts.Include(s => s.BrandsMaster).Include(s => s.ProductCategory).Include(s => s.Unit);
            return View(saleProducts.ToList());
        }

        // GET: SaleProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleProduct saleProduct = db.SaleProducts.Find(id);
            if (saleProduct == null)
            {
                return HttpNotFound();
            }
            return View(saleProduct);
        }

        // GET: SaleProducts/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.BrandsMasters, "ID", "BrandName");
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name");
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name");
            return View();
        }

        // POST: SaleProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ShortName,Type,Size,Color,Price,Description,ProductCategoryID,BrandID,UnitID,Status,ImagePath,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SaleProduct saleProduct)
        {
            if (ModelState.IsValid)
            {
                db.SaleProducts.Add(saleProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.BrandsMasters, "ID", "BrandName", saleProduct.BrandID);
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", saleProduct.ProductCategoryID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", saleProduct.UnitID);
            return View(saleProduct);
        }

        // GET: SaleProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleProduct saleProduct = db.SaleProducts.Find(id);
            if (saleProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.BrandsMasters, "ID", "BrandName", saleProduct.BrandID);
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", saleProduct.ProductCategoryID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", saleProduct.UnitID);
            return View(saleProduct);
        }

        // POST: SaleProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ShortName,Type,Size,Color,Price,Description,ProductCategoryID,BrandID,UnitID,Status,ImagePath,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SaleProduct saleProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.BrandsMasters, "ID", "BrandName", saleProduct.BrandID);
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ID", "Name", saleProduct.ProductCategoryID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", saleProduct.UnitID);
            return View(saleProduct);
        }

        // GET: SaleProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleProduct saleProduct = db.SaleProducts.Find(id);
            if (saleProduct == null)
            {
                return HttpNotFound();
            }
            return View(saleProduct);
        }

        // POST: SaleProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SaleProduct saleProduct = db.SaleProducts.Find(id);
            db.SaleProducts.Remove(saleProduct);
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
