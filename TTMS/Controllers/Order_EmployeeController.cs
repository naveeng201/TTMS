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
    public class Order_EmployeeController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: Order_Employee
        public ActionResult Index()
        {
            var order_Employee = db.Order_Employee.Include(o => o.Employee);
            return View(order_Employee.ToList());
        }

        // GET: Order_Employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Employee order_Employee = db.Order_Employee.Find(id);
            if (order_Employee == null)
            {
                return HttpNotFound();
            }
            return View(order_Employee);
        }

        // GET: Order_Employee/Create
        public ActionResult Create()
        {
            var employees = from emp in db.Employees
                            where emp.MasterEmp != true
                            select new
                            {
                                ID = emp.ID,
                                Name = emp.FirstName + ", " + emp.LastName
                            };
            ViewBag.EmployeeID = new SelectList(employees, "ID", "Name");
            var orders = from order in db.Orders
                         join ordermaster in db.Order_Master on order.ID equals ordermaster.OrderID
                         select new { ID = order.ID, OrderNo = order.OrderNo };
            ViewBag.OrderID = new SelectList(orders, "ID", "OrderNo");
            return View();
        }

        // POST: Order_Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,EmployeeID,Date,Field,NoOfWorksPerDay,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Order_Employee order_Employee)
        {
            if (ModelState.IsValid)
            {
                db.Order_Employee.Add(order_Employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName", order_Employee.EmployeeID);
            return View(order_Employee);
        }

        // GET: Order_Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Employee order_Employee = db.Order_Employee.Find(id);
            if (order_Employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName", order_Employee.EmployeeID);
            return View(order_Employee);
        }

        // POST: Order_Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,Date,Field,NoOfWorksPerDay,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Order_Employee order_Employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_Employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName", order_Employee.EmployeeID);
            return View(order_Employee);
        }

        // GET: Order_Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order_Employee order_Employee = db.Order_Employee.Find(id);
            if (order_Employee == null)
            {
                return HttpNotFound();
            }
            return View(order_Employee);
        }

        // POST: Order_Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order_Employee order_Employee = db.Order_Employee.Find(id);
            db.Order_Employee.Remove(order_Employee);
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
