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
    public class PurchaseEntriesController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: PurchaseEntries
        public ActionResult Index()
        {
            var purchaseEntries = db.PurchaseEntries.Include(p => p.PurchaseOrder);
            return View(purchaseEntries.ToList());
        }

        // GET: PurchaseEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseEntry purchaseEntry = db.PurchaseEntries.Find(id);
            if (purchaseEntry == null)
            {
                return HttpNotFound();
            }
            return View(purchaseEntry);
        }

        [HttpGet]
        public ActionResult GetPurchaseEntryNo()
        {
            var result = db.PurchaseOrders.Select(x => x.PurchaseOrderNo).DefaultIfEmpty().Max();
            int iNumber = 0;
            int.TryParse(result == null ? "0" : result.ToString(), out iNumber);
            iNumber = iNumber + 1;
            return Json(iNumber, JsonRequestBehavior.AllowGet);
        }
        // GET: PurchaseEntries/Create
        public ActionResult Create()
        {
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Invoice", Value = "Invoice" });
            items.Add(new SelectListItem() { Text = "Challan", Value = "Challan" });
            ViewBag.InvoiceChallan = items;

            PurchaseEntryVM purchaseEntryVM = new PurchaseEntryVM();
            purchaseEntryVM._orderDetail = new List<OrderDetail> { new OrderDetail() };
            purchaseEntryVM.purchaseEntry = new PurchaseEntry();
            purchaseEntryVM.purchaseOrder = new PurchaseOrder();
            return View(purchaseEntryVM);
        }


        // POST: PurchaseEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseEntryVM purchaseEntryVM)
        {
            purchaseEntryVM._orderDetail.RemoveAt(0); // remove first template record from list

            PurchaseEntry purchaseEntry = purchaseEntryVM.purchaseEntry;
            if (ModelState.IsValid)
            {
                PurchaseOrder po = new PurchaseOrder();
                po.SupplierID = purchaseEntryVM.purchaseOrder.SupplierID;
                po.PurchaseOrderNo = purchaseEntryVM.purchaseOrder.PurchaseOrderNo;
                if (purchaseEntryVM.purchaseOrder.ID != 0)
                    po.ID = purchaseEntryVM.purchaseOrder.ID;
                po.IsPurcheseEntry = true; // set this is true for Purchase Entry
                foreach (var od in purchaseEntryVM._orderDetail)
                {
                    if (od.ProductID == 0)
                        continue;
                    if (db.OrderDetails.Where(x => x.PurchaseOrderID == purchaseEntryVM.purchaseOrder.ID && x.ProductID == od.ProductID).ToList().Count > 0)
                        continue;
                    po.OrderDetails.Add(od);
                }
                po.PurchaseEntries.Add(purchaseEntry);
                db.PurchaseOrders.Add(po);
                db.SaveChanges();
            }
            var purchaseEntries = db.PurchaseEntries.Include(p => p.PurchaseOrder);
            return View("Index", purchaseEntries.ToList());
        }

        // GET: PurchaseEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseEntry purchaseEntry = db.PurchaseEntries.Find(id);
            if (purchaseEntry == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseOrderID = new SelectList(db.PurchaseOrders, "ID", "PurchaseOrderNo", purchaseEntry.PurchaseOrderID);
            return View(purchaseEntry);
        }

        // POST: PurchaseEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseOrderID,InvoiceChallan,InvoiceChallanNo,InvoiceDate,CGST,SGST,DiscountAmount,TotalAmount,DueAmount,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] PurchaseEntry purchaseEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PurchaseOrderID = new SelectList(db.PurchaseOrders, "ID", "PurchaseOrderNo", purchaseEntry.PurchaseOrderID);
            return View(purchaseEntry);
        }

        // GET: PurchaseEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseEntry purchaseEntry = db.PurchaseEntries.Find(id);
            if (purchaseEntry == null)
            {
                return HttpNotFound();
            }
            return View(purchaseEntry);
        }

        // POST: PurchaseEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseEntry purchaseEntry = db.PurchaseEntries.Find(id);
            db.PurchaseEntries.Remove(purchaseEntry);
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
