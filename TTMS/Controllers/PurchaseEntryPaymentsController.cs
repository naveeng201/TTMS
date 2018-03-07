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
            var purchaseEntryPayments = db.purchaseentrypayments.Include(p => p.purchaseentry);
            return View(purchaseEntryPayments.ToList());
        }

        // GET: PurchaseEntryPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseentrypayment purchaseEntryPayment = db.purchaseentrypayments.Find(id);
            if (purchaseEntryPayment == null)
            {
                return HttpNotFound();
            }
            return View(purchaseEntryPayment);
        }

        // GET: PurchaseEntryPayments/Create
        public ActionResult Create(int iPurchaseEntryID)
        {
            ViewBag.PurchaseEntryID = new SelectList(db.purchaseentries, "ID", "InvoiceChallan");
            purchaseentrypayment objPayment = new purchaseentrypayment();
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
        public ActionResult Create(purchaseentrypayment purchaseEntryPayment)
        {
            if (ModelState.IsValid)
            {
                db.purchaseentrypayments.Add(purchaseEntryPayment);
                db.SaveChanges();
                return RedirectToAction("Index", "PurchaseEntries");
            }

            ViewBag.PurchaseEntryID = new SelectList(db.purchaseentries, "ID", "InvoiceChallan", purchaseEntryPayment.PurchaseEntryID);
            return View(purchaseEntryPayment);
        }

        // GET: PurchaseEntryPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseentrypayment purchaseEntryPayment = db.purchaseentrypayments.Find(id);
            if (purchaseEntryPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseEntryID = new SelectList(db.purchaseentries, "ID", "InvoiceChallan", purchaseEntryPayment.PurchaseEntryID);
            return View(purchaseEntryPayment);
        }

        // POST: PurchaseEntryPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseEntryID,PaymentDate,PaymentMode,PaymentRefNo,PaidAmount,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] purchaseentrypayment purchaseEntryPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseEntryPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PurchaseEntryID = new SelectList(db.purchaseentries, "ID", "InvoiceChallan", purchaseEntryPayment.PurchaseEntryID);
            return View(purchaseEntryPayment);
        }

        // GET: PurchaseEntryPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseentrypayment purchaseEntryPayment = db.purchaseentrypayments.Find(id);
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
            purchaseentrypayment purchaseEntryPayment = db.purchaseentrypayments.Find(id);
            db.purchaseentrypayments.Remove(purchaseEntryPayment);
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
