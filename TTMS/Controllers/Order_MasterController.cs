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
    public class Order_MasterController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: Order_Master
        public ActionResult Index()
        {
            var order_Master = db.Order_Master.Include(o => o.Order);
            return View(order_Master.ToList());
        }

        // GET: Order_Master/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Master order_Master = db.Order_Master.Find(id);
            if (order_Master == null)
            {
                return HttpNotFound();
            }
            return View(order_Master);
        }

        // GET: Order_Master/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "OrderNo");
            var employees = from emp in db.Employees
                            where emp.MasterEmp == true
                            select new {
                                ID = emp.ID,
                                Name = emp.FirstName + ", " + emp.LastName
                            };
            ViewBag.EmployeeID = new SelectList(employees, "ID", "Name");
            return View();
        }

        // POST: Order_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,EmployeeID,Material,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Order_Master order_Master)
        {
            if (ModelState.IsValid)
            {
                db.Order_Master.Add(order_Master);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "ID", "OrderNo", order_Master.OrderID);
            return View(order_Master);
        }

        // GET: Order_Master/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Master order_Master = db.Order_Master.Find(id);
            if (order_Master == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "OrderNo", order_Master.OrderID);
            return View(order_Master);
        }

        // POST: Order_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,EmployeeID,Material,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Order_Master order_Master)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_Master).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "ID", "OrderNo", order_Master.OrderID);
            return View(order_Master);
        }

        // GET: Order_Master/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Master order_Master = db.Order_Master.Find(id);
            if (order_Master == null)
            {
                return HttpNotFound();
            }
            return View(order_Master);
        }

        // POST: Order_Master/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order_Master order_Master = db.Order_Master.Find(id);
            db.Order_Master.Remove(order_Master);
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
