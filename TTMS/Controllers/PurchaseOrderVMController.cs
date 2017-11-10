using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTMS.Models;

namespace TTMS.Controllers
{
    public class PurchaseOrderVMController : Controller
    {
        private TTMSEntities db = new TTMSEntities();
        // GET: PurchaseOrderVM
        public ActionResult Index()
        {
            PurcheseOrderVM POVM = new PurcheseOrderVM();
            POVM.purchaseOrder = db.PurchaseOrders.FirstOrDefault();
            POVM._orderDetail = db.OrderDetails.SingleOrDefault();
            return View();
        }

        public IEnumerable<OrderDetail> GetOrderDetail()
        {
            var od = db.OrderDetails.ToList();
            return od;
        }
        // GET: PurchaseOrderVM/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PurchaseOrderVM/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchaseOrderVM/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PurchaseOrderVM/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PurchaseOrderVM/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PurchaseOrderVM/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PurchaseOrderVM/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
