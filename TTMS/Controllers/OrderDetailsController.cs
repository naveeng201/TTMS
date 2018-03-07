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
    public class OrderDetailsController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetails = db.orderdetails.Include(o => o.product).Include(o => o.purchaseorder);
            return View(orderDetails.ToList());
        }

        // GET: OrderDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orderdetail orderDetail = db.orderdetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
            ViewBag.PurchaseOrderID = new SelectList(db.purchaseorders, "ID", "PurchaseOrderNo");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PurchaseOrderID,ProductID,ProductName,ProductColor,ProductType,Quantity,TypeOfUnits,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] orderdetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.orderdetails.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.products, "ID", "Name", orderDetail.ProductID);
            ViewBag.PurchaseOrderID = new SelectList(db.purchaseorders, "ID", "PurchaseOrderNo", orderDetail.PurchaseOrderID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orderdetail orderDetail = db.orderdetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name", orderDetail.ProductID);
            ViewBag.PurchaseOrderID = new SelectList(db.purchaseorders, "ID", "PurchaseOrderNo", orderDetail.PurchaseOrderID);
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseOrderID,ProductID,ProductName,ProductColor,ProductType,Quantity,TypeOfUnits,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] orderdetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name", orderDetail.ProductID);
            ViewBag.PurchaseOrderID = new SelectList(db.purchaseorders, "ID", "PurchaseOrderNo", orderDetail.PurchaseOrderID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orderdetail orderDetail = db.orderdetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            orderdetail orderDetail = db.orderdetails.Find(id);
            db.orderdetails.Remove(orderDetail);
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
