﻿using System;
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
    public class DeliveriesController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: Deliveries
        public ActionResult Index()
        {
            var deliveries = db.deliveries.Include(d => d.order);
            return View(deliveries.ToList());
        }

        // GET: Deliveries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            delivery delivery = db.deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // GET: Deliveries/Create
        public ActionResult Create()
        {
            var employees = from emp in db.employees
                            where emp.MasterEmp != true
                            select new
                            {
                                ID = emp.ID,
                                Name = emp.FirstName + ", " + emp.LastName
                            };
            var orders = from o in db.orders
                         join om in db.order_master on o.ID equals om.OrderID
                         where om.Status == 4
                         select new { ID = o.ID, OrderNo = o.OrderNo };
            ViewBag.EmployeeID = new SelectList(employees, "ID", "Name");
            ViewBag.OrderID = new SelectList(orders, "ID", "OrderNo");
            return View();
        }

        // POST: Deliveries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,EmployeeID,DeliveryDate,ChallanNo,DeliveredProducts,BalanceOrder,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.deliveries.Add(delivery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.orders, "ID", "OrderNo", delivery.OrderID);
            return View(delivery);
        }

        // GET: Deliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            delivery delivery = db.deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.orders, "ID", "OrderNo", delivery.OrderID);
            return View(delivery);
        }

        // POST: Deliveries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,EmployeeID,DeliveryDate,ChallanNo,DeliveredProducts,BalanceOrder,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] delivery delivery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.orders, "ID", "OrderNo", delivery.OrderID);
            return View(delivery);
        }

        // GET: Deliveries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            delivery delivery = db.deliveries.Find(id);
            if (delivery == null)
            {
                return HttpNotFound();
            }
            return View(delivery);
        }

        // POST: Deliveries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            delivery delivery = db.deliveries.Find(id);
            db.deliveries.Remove(delivery);
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
