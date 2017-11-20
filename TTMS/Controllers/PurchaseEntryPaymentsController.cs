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
    public class PurchaseEntryPaymentsController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: PurchaseEntryPayments
        public ActionResult Index()
        {
            var purchaseEntryPayments = db.PurchaseEntryPayments.Include(p => p.PurchaseEntry);
            return View(purchaseEntryPayments.ToList());
        }

        // GET: PurchaseEntryPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseEntryPayment purchaseEntryPayment = db.PurchaseEntryPayments.Find(id);
            if (purchaseEntryPayment == null)
            {
                return HttpNotFound();
            }
            return View(purchaseEntryPayment);
        }

        // GET: PurchaseEntryPayments/Create
        public ActionResult Create(int iPurchaseEntryID)
        {
            ViewBag.PurchaseEntryID = new SelectList(db.PurchaseEntries, "ID", "InvoiceChallan");
            PurchaseEntryPayment objPayment = new PurchaseEntryPayment();
            objPayment.PurchaseEntryID = iPurchaseEntryID;
            objPayment.PaymentDate = DateTime.Now;
            objPayment.PaidAmount = 0;
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Cash", Value = "Cash" });
            items.Add(new SelectListItem() { Text = "Cheque", Value = "Cheque" });
            items.Add(new SelectListItem() { Text = "DD", Value = "DD" });
            items.Add(new SelectListItem() { Text = "Credit Card", Value = "Credit Card" });
            items.Add(new SelectListItem() { Text = "Debit Card", Value = "Debit Card" });
            items.Add(new SelectListItem() { Text = "Online Transfer", Value = "Online Transfer" });
            items.Add(new SelectListItem() { Text = "Online Wallet Payment", Value = "Online Wallet Payment" });
            ViewBag.PaymentMode= items;
            return PartialView("Create", objPayment);
        }

        // POST: PurchaseEntryPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseEntryPayment purchaseEntryPayment)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseEntryPayments.Add(purchaseEntryPayment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PurchaseEntryID = new SelectList(db.PurchaseEntries, "ID", "InvoiceChallan", purchaseEntryPayment.PurchaseEntryID);
            return View(purchaseEntryPayment);
        }

        // GET: PurchaseEntryPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseEntryPayment purchaseEntryPayment = db.PurchaseEntryPayments.Find(id);
            if (purchaseEntryPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseEntryID = new SelectList(db.PurchaseEntries, "ID", "InvoiceChallan", purchaseEntryPayment.PurchaseEntryID);
            return View(purchaseEntryPayment);
        }

        // POST: PurchaseEntryPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseEntryID,PaymentDate,PaymentMode,PaymentRefNo,PaidAmount,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] PurchaseEntryPayment purchaseEntryPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseEntryPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PurchaseEntryID = new SelectList(db.PurchaseEntries, "ID", "InvoiceChallan", purchaseEntryPayment.PurchaseEntryID);
            return View(purchaseEntryPayment);
        }

        // GET: PurchaseEntryPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseEntryPayment purchaseEntryPayment = db.PurchaseEntryPayments.Find(id);
            if (purchaseEntryPayment == null)
            {
                return HttpNotFound();
            }
            return View(purchaseEntryPayment);
        }

        // POST: PurchaseEntryPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseEntryPayment purchaseEntryPayment = db.PurchaseEntryPayments.Find(id);
            db.PurchaseEntryPayments.Remove(purchaseEntryPayment);
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
