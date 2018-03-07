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
    public class PurchaseEntriesController : Controller
    {
        private TTMSEntities db = new TTMSEntities();
         
        // GET: purchaseentries
        public ActionResult Index()
        {
            var purchaseEntries = db.purchaseentries.Include(p => p.purchaseorder);
            return View(purchaseEntries.ToList());
        }

        // GET: purchaseentries/Details/5
        public ActionResult Details(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //purchaseentry purchaseEntry = db.purchaseentries.Find(id);
            PurchaseEntryVM purchaseEntryVM = new PurchaseEntryVM();
            ViewBag.PurchaseEntryID = id;
            purchaseEntryVM.purchaseEntry = db.purchaseentries.Where(x => x.ID == id).SingleOrDefault();
            purchaseEntryVM.purchaseOrder = db.purchaseorders.Where(x => x.ID == purchaseEntryVM.purchaseEntry.PurchaseOrderID).SingleOrDefault();
            purchaseEntryVM._orderDetail = db.orderdetails.Where(x => x.PurchaseOrderID == purchaseEntryVM.purchaseEntry.PurchaseOrderID).ToList();

            // calculatet Due Amt
            double paidamt = (double)db.purchaseentrypayments.Where(x => x.PurchaseEntryID == id).Select(x => (int?)x.PaidAmount ?? 0)
                                                                                                  .DefaultIfEmpty(0).Sum();
            purchaseEntryVM.purchaseEntry.DueAmount = purchaseEntryVM.purchaseEntry.TotalAmount - paidamt;

            ViewBag.PaymentHistory = db.purchaseentrypayments.Where(x => x.PurchaseEntryID == id).ToList();
             
            if (purchaseEntryVM == null)
            {
                return HttpNotFound();
            }
            return View(purchaseEntryVM);
        }

        [HttpGet]
        public ActionResult GetPurchaseEntryNo()
        {
            var result = db.purchaseorders.Select(x => x.PurchaseOrderNo).DefaultIfEmpty().Max();
            int iNumber = 0;
            int.TryParse(result == null ? "0" : result.ToString(), out iNumber);
            iNumber = iNumber + 1;
            return Json(iNumber, JsonRequestBehavior.AllowGet);
        }
        // GET: purchaseentries/Create
        public ActionResult Create()
        {
            ViewBag.Edit = false;
            ViewBag.Suppliers = db.suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Invoice", Value = "Invoice" });
            items.Add(new SelectListItem() { Text = "Challan", Value = "Challan" });
            ViewBag.InvoiceChallan = items;

            PurchaseEntryVM purchaseEntryVM = new PurchaseEntryVM();
            purchaseEntryVM._orderDetail = new List<orderdetail> { new orderdetail() };
            purchaseEntryVM.purchaseEntry = new purchaseentry {
                CGST = 5,
                SGST = 5,
                DiscountAmount =0
            };
            purchaseEntryVM.purchaseOrder = new purchaseorder();
            return View(purchaseEntryVM);
        }

        private float calculateTotal(List<orderdetail> orderDetails)
        {
            float total = 0;
            foreach(var od in orderDetails)
            {
                total = total + (Convert.ToInt32(od.ReceivedQuantity) * Convert.ToInt32(od.CostPrice));
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

        // POST: purchaseentries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseEntryVM purchaseEntryVM)
        {
            purchaseEntryVM._orderDetail.RemoveAt(0); // remove first template record from list

            purchaseentry purchaseEntry = purchaseEntryVM.purchaseEntry;
            float totalAmt = calculateTotal(purchaseEntryVM._orderDetail);
            purchaseEntry.TotalAmount = calculateGrandTotal(purchaseEntry.CGST, purchaseEntry.SGST, purchaseEntry.DiscountAmount, totalAmt);
            if (purchaseEntry.DueAmount == 0)
                purchaseEntry.Status = true;
            if (ModelState.IsValid)
            {
                purchaseorder po = new purchaseorder();
                po.SupplierID = purchaseEntryVM.purchaseOrder.SupplierID;
                po.PurchaseOrderNo = purchaseEntryVM.purchaseOrder.PurchaseOrderNo;
                if (purchaseEntryVM.purchaseOrder.ID != 0)
                    po.ID = purchaseEntryVM.purchaseOrder.ID;
                po.IsPurcheseEntry = true; // set this is true for Purchase Entry
                foreach (var od in purchaseEntryVM._orderDetail)
                {
                    if (od.ProductID == 0)
                        continue;
                    if (db.orderdetails.Where(x => x.PurchaseOrderID == purchaseEntryVM.purchaseOrder.ID && x.ProductID == od.ProductID).ToList().Count > 0)
                    {
                        var orderDetail = db.orderdetails.Find(od.ID);
                        if (orderDetail != null)
                        {
                            orderDetail.CostPrice = od.CostPrice;
                            orderDetail.PurchaseOrderID = purchaseEntryVM.purchaseOrder.ID;
                            orderDetail.Quantity = od.Quantity;
                            orderDetail.ReceivedQuantity = od.ReceivedQuantity;
                            db.orderdetails.Attach(orderDetail);
                            db.Entry(orderDetail).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    else
                    { 
                        po.orderdetails.Add(od);
                    }

                    // Update inventory table with quantity
                    var objPI = db.productsinventories.Where(x => x.ProductID == od.ProductID).SingleOrDefault();
                    if (objPI != null)
                    {
                        objPI.Quantity = objPI.Quantity + (int)od.ReceivedQuantity;
                        db.productsinventories.Attach(objPI);
                        db.Entry(objPI).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        productsinventory PI = new productsinventory();
                        PI.ProductID = od.ProductID;
                        PI.Quantity = (int)od.ReceivedQuantity;
                        db.productsinventories.Add(PI);
                    }

                }
                if (po.ID == 0)
                {
                    po.purchaseentries.Add(purchaseEntry);
                    db.purchaseorders.Add(po);
                }
                else
                {
                    purchaseEntry.PurchaseOrderID = po.ID;
                    db.purchaseentries.Add(purchaseEntry);
                }
                db.SaveChanges();

                // Once save is success update Products Inventory table

            }
            var purchaseEntries = db.purchaseentries.Include(p => p.purchaseorder);
            return View("Index", purchaseEntries.ToList());
        }

        // GET: purchaseentries/Edit/5
        public ActionResult Edit(int? id, bool IsPurchaseOrder = false )
        {
            ViewBag.Edit = true;
            ViewBag.Suppliers = db.suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
           
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Invoice", Value = "Invoice" });
            items.Add(new SelectListItem() { Text = "Delivery Challan", Value = "Delivery Challan" });
            ViewBag.InvoiceChallan = items;
           

            PurchaseEntryVM purchaseEntryVM = new PurchaseEntryVM();
            if (IsPurchaseOrder)
            {
                ViewBag.PurchaseEntryID = 0;
                purchaseEntryVM.purchaseEntry = new purchaseentry{ CGST = 5,
                    SGST = 5,
                    DiscountAmount = 0 };
                purchaseEntryVM.purchaseOrder = db.purchaseorders.Where(x => x.ID == id).SingleOrDefault();
                purchaseEntryVM._orderDetail = db.orderdetails.Where(x => x.PurchaseOrderID == id).ToList();
            }
            else
            {
                ViewBag.PurchaseEntryID = id;
                purchaseEntryVM.purchaseEntry = db.purchaseentries.Where(x => x.ID == id).SingleOrDefault();
                purchaseEntryVM.purchaseOrder = db.purchaseorders.Where(x => x.ID == purchaseEntryVM.purchaseEntry.PurchaseOrderID).SingleOrDefault();
                purchaseEntryVM._orderDetail = db.orderdetails.Where(x => x.PurchaseOrderID == purchaseEntryVM.purchaseEntry.PurchaseOrderID).ToList();

                // calculatet Due Amt
                double paidamt = (double)db.purchaseentrypayments.Where(x => x.PurchaseEntryID == id).Select(x => (int?)x.PaidAmount ?? 0)
                                                                                                      .DefaultIfEmpty(0).Sum();
                purchaseEntryVM.purchaseEntry.DueAmount = purchaseEntryVM.purchaseEntry.TotalAmount - paidamt;
            }
            ViewBag.NoDue = (purchaseEntryVM.purchaseEntry.DueAmount <= 0) ? true :  false;
            purchaseEntryVM._orderDetail.Insert(0,new orderdetail { });
            return View("Create",purchaseEntryVM);
        }

        // POST: purchaseentries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PurchaseOrderID,InvoiceChallan,InvoiceChallanNo,InvoiceDate,CGST,SGST,DiscountAmount,TotalAmount,DueAmount,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] purchaseentry purchaseEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaseEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PurchaseOrderID = new SelectList(db.purchaseorders, "ID", "PurchaseOrderNo", purchaseEntry.PurchaseOrderID);
            return View(purchaseEntry);
        }

        // GET: purchaseentries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseentry purchaseEntry = db.purchaseentries.Find(id);
            if (purchaseEntry == null)
            {
                return HttpNotFound();
            }
            return View(purchaseEntry);
        }

        // POST: purchaseentries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            purchaseentry purchaseEntry = db.purchaseentries.Find(id);
            db.purchaseentries.Remove(purchaseEntry);
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
