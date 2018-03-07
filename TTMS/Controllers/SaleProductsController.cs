using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            var saleProducts = db.saleproducts.Include(s => s.brandsmaster).Include(s => s.productcategory).Include(s => s.unit);
            return View(saleProducts.ToList());
        }

        // GET: SaleProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            saleproduct saleProduct = db.saleproducts.Find(id);
            if (saleProduct == null)
            {
                return HttpNotFound();
            }
            return View(saleProduct);
        }

        // GET: SaleProducts/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.brandsmasters, "ID", "BrandName");
            ViewBag.ProductCategoryID = new SelectList(db.productcategories, "ID", "Name");
            ViewBag.UnitID = new SelectList(db.units, "ID", "Name");
            return View();
        }

        // POST: SaleProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(saleproduct saleProduct)
        {
            if (ModelState.IsValid)
            {
                var file = Request.Files["ImageUpload"];
                if (file != null && file.ContentLength > 0)
                {
                    var uploadDir = "~/assets/ProductImages";
                    var imagePath = Path.Combine(Server.MapPath(uploadDir), file.FileName);
                    var imageUrl = Path.Combine(uploadDir, file.FileName);
                    file.SaveAs(imagePath);
                    saleProduct.ImagePath = imageUrl;
                }

                db.saleproducts.Add(saleProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.brandsmasters, "ID", "BrandName", saleProduct.BrandID);
            ViewBag.ProductCategoryID = new SelectList(db.productcategories, "ID", "Name", saleProduct.ProductCategoryID);
            ViewBag.UnitID = new SelectList(db.units, "ID", "Name", saleProduct.UnitID);
            return View(saleProduct);
        }

        // GET: SaleProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            saleproduct saleProduct = db.saleproducts.Find(id);
            if (saleProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.brandsmasters, "ID", "BrandName", saleProduct.BrandID);
            ViewBag.ProductCategoryID = new SelectList(db.productcategories, "ID", "Name", saleProduct.ProductCategoryID);
            ViewBag.UnitID = new SelectList(db.units, "ID", "Name", saleProduct.UnitID);
            return View(saleProduct);
        }

        // POST: SaleProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ShortName,Type,Size,Color,Price,Description,ProductCategoryID,BrandID,UnitID,Status,ImagePath,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] saleproduct saleProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(saleProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.brandsmasters, "ID", "BrandName", saleProduct.BrandID);
            ViewBag.ProductCategoryID = new SelectList(db.productcategories, "ID", "Name", saleProduct.ProductCategoryID);
            ViewBag.UnitID = new SelectList(db.units, "ID", "Name", saleProduct.UnitID);
            return View(saleProduct);
        }

        // GET: SaleProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            saleproduct saleProduct = db.saleproducts.Find(id);
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
            saleproduct saleProduct = db.saleproducts.Find(id);
            db.saleproducts.Remove(saleProduct);
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
