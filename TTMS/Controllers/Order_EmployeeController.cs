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

        // GET: order_employee
        public ActionResult Index()
        {
            var order_Employee = db.order_employee.Include(o => o.employee);
            return View(order_Employee.ToList());
        }

        // GET: order_employee/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_employee order_Employee = db.order_employee.Find(id);
            if (order_Employee == null)
            {
                return HttpNotFound();
            }
            return View(order_Employee);
        }

        // GET: order_employee/Create
        public ActionResult Create()
        {
            var employees = from emp in db.employees
                            where emp.MasterEmp != true
                            select new
                            {
                                ID = emp.ID,
                                Name = emp.FirstName + ", " + emp.LastName
                            };
            ViewBag.EmployeeID = new SelectList(employees, "ID", "Name");
            var orders = from order in db.orders
                         join ordermaster in db.order_master on order.ID equals ordermaster.OrderID
                         select new { ID = order.ID, OrderNo = order.OrderNo };
            ViewBag.OrderID = new SelectList(orders, "ID", "OrderNo");
            return View();
        }

        // POST: order_employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,OrderID,EmployeeID,Date,Field,NoOfWorksPerDay,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] order_employee order_Employee)
        {
            if (ModelState.IsValid)
            {
                db.order_employee.Add(order_Employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.employees, "ID", "FirstName", order_Employee.EmployeeID);
            return View(order_Employee);
        }

        // GET: order_employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_employee order_Employee = db.order_employee.Find(id);
            if (order_Employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.employees, "ID", "FirstName", order_Employee.EmployeeID);
            return View(order_Employee);
        }

        // POST: order_employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,Date,Field,NoOfWorksPerDay,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] order_employee order_Employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_Employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.employees, "ID", "FirstName", order_Employee.EmployeeID);
            return View(order_Employee);
        }

        // GET: order_employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_employee order_Employee = db.order_employee.Find(id);
            if (order_Employee == null)
            {
                return HttpNotFound();
            }
            return View(order_Employee);
        }

        // POST: order_employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order_employee order_Employee = db.order_employee.Find(id);
            db.order_employee.Remove(order_Employee);
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
