using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTMS.Models;
using Microsoft.AspNet.Identity;

namespace TTMS.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private TTMSEntities db = new TTMSEntities();

        // GET: Orders
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        public ActionResult Add(SaleProduct sp)
        {
            if (Session["cart"] == null)
            {
                List<SaleProduct> li = new List<SaleProduct>();
                li.Add(sp);
                Session["cart"] = li;
                ViewBag.cart = li.Count();
                Session["count"] = 1;
            }
            else
            {
                List<SaleProduct> li = (List<SaleProduct>)Session["cart"];
                li.Add(sp);
                Session["cart"] = li;
                ViewBag.cart = li.Count();
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
            }
            return RedirectToAction("NewOrder", "Orders");
        }

        public ActionResult AddToCart()
        {
            List<SaleProduct> spList = (List <SaleProduct>) Session["cart"];
            return View(spList);
        }

        public ActionResult Cart()
        {
            return View((List<SaleProduct>)Session["cart"]);
        }
        public ActionResult Remove(SaleProduct sp)
        {
            List<SaleProduct> li = (List<SaleProduct>)Session["cart"];
            li.RemoveAll(x => x.ID == sp.ID);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return RedirectToAction("Cart", "Orders");

        }
        

        public ActionResult PlaceOrder()
        {
            OrderVM orderVM = new OrderVM();
            orderVM.customer = new Customer();
            orderVM.customerAddress = new CustomerAddress();
            orderVM.order = new Order();
            var cartProducts = (List<SaleProduct>)Session["cart"];
            if(cartProducts != null)
            foreach(var cProd in cartProducts)
            {
                OrderItem oi = new OrderItem();
                oi.ProductID = cProd.ID;
                oi.Price = cProd.Price;
                oi.Quantity = 1;
            }
            return View(orderVM);
        }

        [HttpGet]
        public JsonResult GetCustomers(string strCust)
        {

        }
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrdersVM objOrder = new OrdersVM();
            objOrder.order = db.Orders.Find(id);
            objOrder.orderItems = db.OrderItems.Where(x => x.OrderID == id).ToList();
            if (objOrder == null)
            {
                return HttpNotFound();
            }
            return View(objOrder);
        }
        public ActionResult NewOrder()
        {
            IEnumerable<SaleProduct> productsList = db.SaleProducts.ToList();
            return View(productsList);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.Edit = false;
            ViewBag.ProductID = new SelectList(db.Products, "ID", "Name");
            OrdersVM objOrders = new OrdersVM();
            objOrders.order = new Order();
            objOrders.orderItems = new List<OrderItem> { new OrderItem() };
            return View(objOrders);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "ID,OrderNo,CustomerOrganizationName,Address,City,State,Country,Zip,ContactPersonName,ContactNo,ProductType,Size,Quantity,DeliveryDate,PricePerUnit,TotalPrice,GrandTotalWithTax,Advance,Remarks,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")]*/ OrdersVM orderVM)
        {
            orderVM.orderItems.RemoveAt(0);

            if (ModelState.IsValid)
            {
                Order objOrder = orderVM.order;
                objOrder.TotalPrice = calculateTotal(orderVM.orderItems);
                objOrder.GrandTotalWithTax = objOrder.TotalPrice;
                foreach (var orderItem in orderVM.orderItems)
                {
                    objOrder.OrderItems.Add(orderItem);
                }
                db.Orders.Add(objOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderVM);
        }
        private float calculateTotal(IEnumerable<OrderItem> orderItems)
        {
            float total = 0;
            foreach (var od in orderItems)
            {
                total = total + (Convert.ToInt32(od.Quantity) * Convert.ToInt32(od.Price));
            }
            return total;
        }
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderNo,CustomerOrganizationName,Address,City,State,Country,Zip,ContactPersonName,ContactNo,ProductType,Size,Quantity,DeliveryDate,PricePerUnit,TotalPrice,GrandTotalWithTax,Advance,Remarks,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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

        private string GenerateOrderNo()
        {
            var userId = User.Identity.GetUserId().ToString();
            var datepart = DateTime.Now.Day.ToString();
            var monthpart = DateTime.Now.Month.ToString();
            Random rand = new Random();
            int randNo = rand.Next(00000, 99999);
            string OrderNo = string.Format("{0}{1}{2}{3}{4}", "A",userId.Substring(userId.Length - 4),  monthpart, datepart, randNo);
            if (db.Orders.Any(x => x.OrderNo == OrderNo))
            {
                GenerateOrderNo();
            }
            return OrderNo;
        }
        [HttpGet]
        public ActionResult GetOrderNo()
        {
            string strOrderNo = GenerateOrderNo();
            return Json(strOrderNo.ToUpper(), JsonRequestBehavior.AllowGet);
        }
    }
}
