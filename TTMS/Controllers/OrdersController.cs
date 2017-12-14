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
            List<CartProduct> cpList = new List<CartProduct>();

            if (Session["cart"] != null)
            {
                var prodList = (List<SaleProduct>)Session["cart"];
                var groupedProds = prodList.GroupBy(info => info.ID).Select(g => new {
                    ID = g.Key,
                    Count = g.Count()
                });
                foreach (var line in groupedProds)
                {
                    CartProduct cp = new CartProduct();
                    cp.prodduct = prodList.Where(x => x.ID == line.ID).FirstOrDefault();
                    cp.quantity = line.Count;
                    cpList.Add(cp);
                }
            }
            return View(cpList);
        }
        public ActionResult Remove(SaleProduct sp)
        {
            List<SaleProduct> li = (List<SaleProduct>)Session["cart"];
            li.RemoveAll(x => x.ID == sp.ID);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return RedirectToAction("Cart", "Orders");

        }

        [HttpPost]
        public ActionResult PlaceOrder(List<CartProduct> cpList)
        {
            OrderVM orderVM = new OrderVM();
            orderVM.orderItems = new List<OrderItem>();
            var cartProducts = (List<SaleProduct>)Session["cart"];
            double total = 0;
            if(cartProducts != null)
            foreach(var cProd in cartProducts)
            {
                OrderItem oi = new OrderItem();
                oi.ProductID = cProd.ID;
                oi.Price = cProd.Price;
                oi.Quantity = 1;
                orderVM.orderItems.Add(oi);
               if(cProd.Price != null && cProd.Price != 0)
                total = total + (double)cProd.Price;
            }
            orderVM.order = new Order();
            orderVM.order.TotalPrice = total;
            orderVM.order.GST = 5;
            orderVM.order.GrandTotalWithTax = total;
            orderVM.order.DeliveryDate = DateTime.Now.AddDays(30);
            orderVM.order.Discount = 0;
            orderVM.order.Advance = 0;
            return View(orderVM);
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
        public ActionResult Create(/*[Bind(Include = "ID,OrderNo,CustomerOrganizationName,Address,City,State,Country,Zip,ContactPersonName,ContactNo,ProductType,Size,Quantity,DeliveryDate,PricePerUnit,TotalPrice,GrandTotalWithTax,Advance,Remarks,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")]*/ OrderVM orderVM)
        {
            if (ModelState.IsValid)
            {
                var CustAddress = new CustomerAddress();
                CustAddress.Customer = orderVM.customer;
                CustAddress.Address = orderVM.address;
                if (orderVM.customer.ID == 0)
                {
                    db.CustomerAddresses.Add(CustAddress);
                    db.SaveChanges();
                }

                Order objOrder = orderVM.order;
                objOrder.OrderItems = orderVM.orderItems;
                objOrder.OrderNo = GenerateOrderNo();
                objOrder.AddressID = CustAddress.Address.ID;
                objOrder.CustomerID = CustAddress.Customer.ID;
                db.Orders.Add(objOrder);
                db.SaveChanges();
                // return RedirectToAction("OrderSuccess", orderVM);
                Session["cart"] = null;
                ViewBag.NewOrder = true;
            }
            return View("OrderSummary", orderVM);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderVM objOrder = new OrderVM();
            objOrder.order = db.Orders.Find(id);
            objOrder.orderItems = db.OrderItems.Where(x => x.OrderID == id).ToList();
            objOrder.address = db.Addresses.Find(objOrder.order.AddressID);
            objOrder.customer = db.Customers.Find(objOrder.order.CustomerID);
             
            return View("OrderSummary", objOrder);
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
            var orderItems = db.OrderItems.Where(x => x.OrderID == id).ToList();
            foreach(OrderItem oi in orderItems)
            {
                db.OrderItems.Remove(oi);
            }
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

        [HttpGet]
        public JsonResult GetCustomers(string strCust)
        {
            var result = from c in db.Customers
                         where c.CustomerOrganizationName.Contains(strCust)
                         select new
                         {
                             Name = c.CustomerOrganizationName,
                             Value = c.ID
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCustomersAddress(int id)
        {
            var result = from c in db.Customers
                         join ca in db.CustomerAddresses on c.ID equals ca.CustomerID
                         join a in db.Addresses on ca.AddressID equals a.ID
                         where c.ID == id
                         select new
                         {
                             cpName = c.ContactPersonName,
                             cNo = c.ContactNo,
                             acNo = c.AlternateContactNo,
                             ID = a.ID,
                             Name = a.Name,
                             Address1 = a.Address1,
                             Address2 = a.Address2,
                             City = a.City,
                             State = a.State,
                             Country = a.Country,
                             Zip = a.Zip,
                             AddressType = a.AddressType
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
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
