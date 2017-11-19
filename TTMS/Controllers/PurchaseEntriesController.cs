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
            ViewBag.Edit = false;
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Invoice", Value = "Invoice" });
            items.Add(new SelectListItem() { Text = "Challan", Value = "Challan" });
            ViewBag.InvoiceChallan = items;

            PurchaseEntryVM purchaseEntryVM = new PurchaseEntryVM();
            purchaseEntryVM._orderDetail = new List<OrderDetail> { new OrderDetail() };
            purchaseEntryVM.purchaseEntry = new PurchaseEntry {
                CGST = 5,
                SGST = 5,
                DiscountAmount =0
            };
            purchaseEntryVM.purchaseOrder = new PurchaseOrder();
            return View(purchaseEntryVM);
        }

        private float calculateTotal(List<OrderDetail> orderDetails)
        {
            float total = 0;
            foreach(var od in orderDetails)
            {
                total = total + (Convert.ToInt32(od.Quantity) * Convert.ToInt32(od.CostPrice));
            }
            return total;
        }
        private double calculateGrandTotal(double? cgst, double? sgst, double? discount, float total)
        {
            double gtotal = 0;
            double gst = (double)cgst + (double)sgst;
            gtotal = total + ((total / 100) * gst);
            if(discount != 0)
             gtotal = total - ((total / 100) * (double)discount);
            return gtotal;
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
            float totalAmt = calculateTotal(purchaseEntryVM._orderDetail);
            purchaseEntry.TotalAmount = calculateGrandTotal(purchaseEntry.CGST, purchaseEntry.SGST, purchaseEntry.DiscountAmount, totalAmt);
            if (purchaseEntry.DueAmount == 0)
                purchaseEntry.Status = true;
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
                    {
                        var orderDetail = db.OrderDetails.Find(od.ID);
                        if (orderDetail != null)
                        {
                            orderDetail.CostPrice = od.CostPrice;
                            orderDetail.PurchaseOrderID = purchaseEntryVM.purchaseOrder.ID;
                            orderDetail.Quantity = od.Quantity;
                            db.OrderDetails.Attach(orderDetail);
                           // db.Entry(orderDetail).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        po.OrderDetails.Add(od);
                    }
                }
                if (po.ID == 0)
                {
                    po.PurchaseEntries.Add(purchaseEntry);
                    db.PurchaseOrders.Add(po);
                }
                else
                {
                    purchaseEntry.PurchaseOrderID = po.ID;
                    db.PurchaseEntries.Add(purchaseEntry);
                }
                db.SaveChanges();
            }
            var purchaseEntries = db.PurchaseEntries.Include(p => p.PurchaseOrder);
            return View("Index", purchaseEntries.ToList());
        }

        // GET: PurchaseEntries/Edit/5
        public ActionResult Edit(int? id, bool IsPurchaseOrder = false )
        {
            ViewBag.Edit = true;
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
           
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Invoice", Value = "Invoice" });
            items.Add(new SelectListItem() { Text = "Challan", Value = "Challan" });
            ViewBag.InvoiceChallan = items;
           

            PurchaseEntryVM purchaseEntryVM = new PurchaseEntryVM();
            if (IsPurchaseOrder)
            {
                ViewBag.PurchaseEntryID = 0;
                purchaseEntryVM.purchaseEntry = new PurchaseEntry{ CGST = 5,
                    SGST = 5,
                    DiscountAmount = 0 };
                purchaseEntryVM.purchaseOrder = db.PurchaseOrders.Where(x => x.ID == id).SingleOrDefault();
                purchaseEntryVM._orderDetail = db.OrderDetails.Where(x => x.PurchaseOrderID == id).ToList();
            }
            else
            {
                ViewBag.PurchaseEntryID = id;
                purchaseEntryVM.purchaseEntry = db.PurchaseEntries.Where(x => x.ID == id).SingleOrDefault();
                purchaseEntryVM.purchaseOrder = db.PurchaseOrders.Where(x => x.ID == purchaseEntryVM.purchaseEntry.PurchaseOrderID).SingleOrDefault();
                purchaseEntryVM._orderDetail = db.OrderDetails.Where(x => x.PurchaseOrderID == purchaseEntryVM.purchaseEntry.PurchaseOrderID).ToList();

                // calculatet Due Amt
                double paidamt = (double)db.PurchaseEntryPayments.Where(x => x.PurchaseEntryID == id).Select(x => (int?)x.PaidAmount ?? 0)
                                                                                                      .DefaultIfEmpty(0).Sum();
                purchaseEntryVM.purchaseEntry.DueAmount = purchaseEntryVM.purchaseEntry.TotalAmount - paidamt;
            }
            ViewBag.NoDue = (purchaseEntryVM.purchaseEntry.DueAmount <= 0) ? true :  false;
            purchaseEntryVM._orderDetail.Insert(0,new OrderDetail { });
            return View("Create",purchaseEntryVM);
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
