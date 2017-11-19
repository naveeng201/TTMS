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
    public class PurchaseOrdersController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: PurchaseOrders
        public ActionResult Index()
        {
            //var purchaseOrder = db.PurchaseOrders.Include(p => p.Supplier);
            //return View(purchaseOrder.ToList());
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "OrganizationName");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");

            var poList = db.PurchaseOrders.Where(x=>x.IsPurcheseEntry == false).ToList();
            //PO._orderDetail = new List<OrderDetail>();
            //PO.purchaseOrder = new PurchaseOrder();
            //PO.purchaseOrder.OrderDetails = db.OrderDetails.ToList();
            return View(poList);
        }

        // GET: PurchaseOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "OrganizationName");
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            var purchaseOrder = new PurchaseOrderVM();
            purchaseOrder.purchaseOrder = new PurchaseOrder();
            purchaseOrder._orderDetail = new List<OrderDetail> { new OrderDetail() };
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
                PurchaseOrder po = new PurchaseOrder();
                po.SupplierID = purchaseOrderVM.purchaseOrder.SupplierID;
                po.PurchaseOrderNo = purchaseOrderVM.purchaseOrder.PurchaseOrderNo;
                if (purchaseOrderVM.purchaseOrder.ID != 0)
                    po.ID = purchaseOrderVM.purchaseOrder.ID;
                po.IsPurcheseEntry = false; // set this is false for Purchase Order
                foreach (var od in purchaseOrderVM._orderDetail)
                {
                    if (od.ProductID == 0)
                        continue;
                    if (db.OrderDetails.Where(x => x.PurchaseOrderID == purchaseOrderVM.purchaseOrder.ID && x.ProductID == od.ProductID).ToList().Count > 0)
                        continue;
                    po.OrderDetails.Add(od);
                }
                db.PurchaseOrders.Add(po);
                db.SaveChanges();
                var poList = db.PurchaseOrders.ToList();
                return View("Index", poList);
            }
            return View("Create", purchaseOrderVM);
        }
        [HttpGet]
        public ActionResult GetPurchaseOrderNo()
        {
            var result = db.PurchaseOrders.Select(x => x.PurchaseOrderNo).DefaultIfEmpty().Max();
            int iNumber = 0;
            int.TryParse(result == null ? "0" : result.ToString(), out iNumber);
            iNumber = iNumber + 1;
            return Json(iNumber, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProdectDetails(int id)
        {

            //do the logic for taking the value for textbox
            var result = from c in db.Products
                         where c.ID == id
                         select new {c.Price,c.ShorName };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // POST: PurchaseOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "ID,PurchaseOrderNo,SupplierID,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] */PurchaseOrderVM purchaseOrderVM)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder = purchaseOrderVM.purchaseOrder;
            purchaseOrder.OrderDetails = purchaseOrderVM._orderDetail;
            if (ModelState.IsValid)
            {
                db.PurchaseOrders.Add(purchaseOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }

            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "OrganizationName");
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            var purchaseOrderVM = new PurchaseOrderVM();
            purchaseOrderVM.purchaseOrder = purchaseOrder;
            purchaseOrderVM._orderDetail = db.OrderDetails.Where(x => x.PurchaseOrderID == id).ToList();
            purchaseOrderVM._orderDetail.Insert(0, new OrderDetail { });
            return View("Create",purchaseOrderVM);
        }

        // POST: PurchaseOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "ID,PurchaseOrderNo,SupplierID,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")]*/ PurchaseOrderVM purchaseOrderVM)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            purchaseOrder = purchaseOrderVM.purchaseOrder;
            if (ModelState.IsValid)
            {
                db.Entry(purchaseOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseOrder);
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
