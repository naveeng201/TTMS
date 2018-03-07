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
    public class PurchaseOrdersController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: purchaseorders
        public ActionResult Index()
        {
            //var purchaseOrder = db.purchaseorders.Include(p => p.Supplier);
            //return View(purchaseOrder.ToList());
            ViewBag.SupplierID = new SelectList(db.suppliers, "ID", "OrganizationName");
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
            
            var poList = db.purchaseorders.Where(x=>x.IsPurcheseEntry == false).ToList();
            //PO._orderDetail = new List<orderdetail>();
            //PO.purchaseOrder = new purchaseorder();
            //PO.purchaseOrder.orderdetails = db.orderdetails.ToList();
            return View(poList);
        }

        // GET: purchaseorders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseorder purchaseOrder = db.purchaseorders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // GET: purchaseorders/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.suppliers, "ID", "OrganizationName");
            ViewBag.suppliers = db.suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
            var purchaseOrder = new PurchaseOrderVM();
            purchaseOrder.purchaseOrder = new purchaseorder();
            purchaseOrder._orderDetail = new List<orderdetail> { new orderdetail() };
            return View(purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductDetails(string submitAction, PurchaseOrderVM purchaseOrderVM)
        {
            //Remove first template record.
            purchaseOrderVM._orderDetail.RemoveAt(0);

            if (ModelState.IsValid)
            {
                purchaseorder po = new purchaseorder();
                po.SupplierID = purchaseOrderVM.purchaseOrder.SupplierID;
                po.PurchaseOrderNo = purchaseOrderVM.purchaseOrder.PurchaseOrderNo;
                if (purchaseOrderVM.purchaseOrder.ID != 0)
                    po.ID = purchaseOrderVM.purchaseOrder.ID;
                po.IsPurcheseEntry = false; // set this is false for Purchase Order
                foreach (var od in purchaseOrderVM._orderDetail)
                {
                    if (od.ProductID == 0)
                        continue;
                    if (db.orderdetails.Where(x => x.PurchaseOrderID == purchaseOrderVM.purchaseOrder.ID && x.ProductID == od.ProductID).ToList().Count > 0)
                        continue;
                    po.orderdetails.Add(od);
                }
                db.purchaseorders.Add(po);
                db.SaveChanges();
                var poList = db.purchaseorders.ToList();
                return View("Index", poList);
            }
            return View("Create", purchaseOrderVM);
        }
        [HttpGet]
        public ActionResult GetPurchaseOrderNo()
        {
            var result = db.purchaseorders.Select(x => x.PurchaseOrderNo).DefaultIfEmpty().Max();
            int iNumber = 0;
            int.TryParse(result == null ? "0" : result.ToString(), out iNumber);
            iNumber = iNumber + 1;
            return Json(iNumber, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProdectDetails(int id)
        {

            //do the logic for taking the value for textbox
            var result = from c in db.products
                         where c.ID == id
                         select new {c.Price,c.ShorName };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // POST: purchaseorders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "ID,PurchaseOrderNo,SupplierID,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] */PurchaseOrderVM purchaseOrderVM)
        {
            purchaseorder purchaseOrder = new purchaseorder();
            purchaseOrder = purchaseOrderVM.purchaseOrder;
            purchaseOrder.orderdetails = purchaseOrderVM._orderDetail;
            if (ModelState.IsValid)
            {
                db.purchaseorders.Add(purchaseOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(purchaseOrder);
        }

        // GET: purchaseorders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseorder purchaseOrder = db.purchaseorders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }

            ViewBag.SupplierID = new SelectList(db.suppliers, "ID", "OrganizationName");
            ViewBag.suppliers = db.suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
            var purchaseOrderVM = new PurchaseOrderVM();
            purchaseOrderVM.purchaseOrder = purchaseOrder;
            purchaseOrderVM._orderDetail = db.orderdetails.Where(x => x.PurchaseOrderID == id).ToList();
            purchaseOrderVM._orderDetail.Insert(0, new orderdetail { });
            return View("Create",purchaseOrderVM);
        }

        // POST: purchaseorders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "ID,PurchaseOrderNo,SupplierID,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")]*/ PurchaseOrderVM purchaseOrderVM)
        {
            purchaseorder purchaseOrder = new purchaseorder();
            purchaseOrder = purchaseOrderVM.purchaseOrder;
            if (ModelState.IsValid)
            {
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseOrder);
        }

        // GET: purchaseorders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchaseorder purchaseOrder = db.purchaseorders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // POST: purchaseorders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            purchaseorder purchaseOrder = db.purchaseorders.Find(id);
            db.purchaseorders.Remove(purchaseOrder);
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
