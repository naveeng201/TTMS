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
            var orders = from order in db.Orders
                         where !(from om in db.Order_Master select om.OrderID).Contains(order.ID)
                         select order;
            ViewBag.OrderID = new SelectList(orders, "ID", "OrderNo");
            var employees = from emp in db.Employees
                            where emp.MasterEmp == true
                            select new {
                                ID = emp.ID,
                                Name = emp.FirstName + ", " + emp.LastName
                            };
            ViewBag.EmployeeID = new SelectList(employees, "ID", "Name");
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            List<SelectListItem> items = new List<SelectListItem> {
                new SelectListItem{Text = statusEnum.NewOrder.ToString(), Value = "5" },
                new SelectListItem{Text = statusEnum.InProgress.ToString(), Value="6"},
                new SelectListItem{Text=statusEnum.Completed.ToString(),Value="4"}
            };
            ViewBag.Status = new SelectList(items, "Value", "Text");

            Order_MasterVM orderMasterVM = new Order_MasterVM();
            orderMasterVM.order_Master_products = new List<OrderMasterProducts>
            {
                new OrderMasterProducts()
            };
            return View(orderMasterVM);
        }

        // POST: Order_Master/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order_MasterVM order_MasterVM)
        {
            order_MasterVM.order_Master_products.RemoveAt(0);
            Order_Master OM = new Order_Master();
            OM.EmployeeID = order_MasterVM.EmployeeID;
            OM.OrderID = order_MasterVM.OrderID;
            OM.Status =Convert.ToInt32(order_MasterVM.Status);
            if (ModelState.IsValid)
            {
                foreach(var x in order_MasterVM.order_Master_products)
                {
                    Order_Master_Items obj = new Order_Master_Items();
                    obj.ProductID = x.ProductID;
                    obj.Quantity = x.Quantity;
                    obj.RemainingQuantity = x.RemainingQuantity;
                    OM.Order_Master_Items.Add(obj);

                    // update inventery
                    var objPI = db.ProductsInventories.Where(p => p.ProductID == x.ProductID).SingleOrDefault();
                    if (objPI != null)
                    {
                        objPI.Quantity = objPI.Quantity - (int)x.Quantity;
                        db.ProductsInventories.Attach(objPI);
                        db.Entry(objPI).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("One or more product does not exist or it does not have required quantity.");
                    }
                }
                db.Order_Master.Add(OM);

                

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "ID", "OrderNo", order_MasterVM.OrderID);
            return View(order_MasterVM);
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

            Order_MasterVM orderMasterVM = new Order_MasterVM();
            orderMasterVM.order_Master_products = new List<OrderMasterProducts>();
            orderMasterVM.OrderNo = order_Master.Order.OrderNo;
            orderMasterVM.EmployeeName = order_Master.Employee.FirstName +" "+ order_Master.Employee.LastName;
            orderMasterVM.Status = order_Master.Status.ToString();

            List<SelectListItem> items = new List<SelectListItem> {
                new SelectListItem{Text = statusEnum.NewOrder.ToString(), Value = "5" },
                new SelectListItem{Text = statusEnum.InProgress.ToString(), Value="6"},
                new SelectListItem{Text=statusEnum.Completed.ToString(),Value="4"}
            };
            ViewBag.Status = new SelectList(items, "Value", "Text", orderMasterVM.Status);

            foreach (var omi in db.Order_Master_Items.Where(x => x.OrderMasterID == order_Master.ID).ToList())
            {
                OrderMasterProducts objOMP = new OrderMasterProducts();
                objOMP.ID = omi.ID;
                objOMP.Name = omi.Product.Name;
                objOMP.ProductID = omi.ProductID;
                objOMP.Quantity = omi.Quantity;
                objOMP.RemainingQuantity = 0;
                orderMasterVM.order_Master_products.Add(objOMP);
            }

            return View(orderMasterVM);
        }

        // POST: Order_Master/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order_MasterVM order_MasterVM)
        {
            if (ModelState.IsValid)
            {
                foreach(var omp in order_MasterVM.order_Master_products)
                {
                    var orderMasterItems = db.Order_Master_Items.Where(x => x.ID == omp.ID).FirstOrDefault();
                    orderMasterItems.RemainingQuantity = omp.RemainingQuantity;
                    db.Entry(orderMasterItems).State = EntityState.Modified;
                    db.SaveChanges();

                    var objPI = db.ProductsInventories.Where(p => p.ProductID == omp.ProductID).SingleOrDefault();
                    if (objPI != null)
                    {
                        objPI.Quantity = objPI.Quantity + (int)omp.RemainingQuantity;
                        db.ProductsInventories.Attach(objPI);
                        db.Entry(objPI).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("One or more product does not exist or it does not have required quantity.");
                    }
                }
                var order_Master = db.Order_Master.Where(x => x.ID == order_MasterVM.ID).FirstOrDefault();
                order_Master.Status = Convert.ToInt32(order_MasterVM.Status);
                db.Entry(order_Master).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order_MasterVM);
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
        [HttpGet]
        public ActionResult GetProdectDetails(int id)
        {

            //do the logic for taking the value for textbox
            var result = from  pi in db.ProductsInventories
                         join c in db.Products on pi.ProductID equals c.ID
                         where c.ID == id
                         select new { c.Price, c.ShorName,pi.Quantity };
            if(result.ToList().Count <= 0)
            {
                throw new HttpException("No products");
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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
