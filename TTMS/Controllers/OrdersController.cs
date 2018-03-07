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

        // GET: orders
        public ActionResult Index()
        {
            return View(db.orders.ToList());
        }

        public ActionResult Add(saleproduct sp)
        {
            if (Session["cart"] == null)
            {
                List<saleproduct> li = new List<saleproduct>();
                li.Add(sp);
                Session["cart"] = li;
                ViewBag.cart = li.Count();
                Session["count"] = 1;
            }
            else
            {
                List<saleproduct> li = (List<saleproduct>)Session["cart"];
                li.Add(sp);
                Session["cart"] = li;
                ViewBag.cart = li.Count();
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
            }
            return RedirectToAction("NewOrder", "Orders");
        }

        public ActionResult AddToCart()
        {
            List<saleproduct> spList = (List <saleproduct>) Session["cart"];
            return View(spList);
        }

        public ActionResult Cart()
        {
            List<CartProduct> cpList = new List<CartProduct>();

            if (Session["cart"] != null)
            {
                var prodList = (List<saleproduct>)Session["cart"];
                var groupedProds = prodList.GroupBy(info => info.ID).Select(g => new {
                    ID = g.Key,
                    Count = g.Count()
                });
                foreach (var line in groupedProds)
                {
                    CartProduct cp = new CartProduct();
                    var prodduct = prodList.Where(x => x.ID == line.ID).FirstOrDefault();
                    cp.productID = prodduct.ID;
                    cp.name = prodduct.Name;
                    cp.imagePath = prodduct.ImagePath;
                    cp.price = (double)prodduct.Price;
                    cp.quantity = line.Count;
                    cpList.Add(cp);
                }
            }
            return View(cpList);
        }
        public ActionResult Remove(CartProduct sp)
        {
            List<saleproduct> li = (List<saleproduct>)Session["cart"];
            li.RemoveAll(x => x.ID == sp.productID);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return RedirectToAction("Cart", "Orders");

        }

        [HttpPost]
        public ActionResult PlaceOrder(List<CartProduct> cpList)
        {
            OrderVM orderVM = new OrderVM();
            orderVM.address = new address();
            orderVM.customer = new customer();
            orderVM.orderItems = new List<orderitem>();
            double total = 0;
            if (cpList != null)
            {
                foreach (var cProd in cpList)
                {
                    orderitem oi = new orderitem();
                    oi.ProductID = cProd.productID;
                    oi.Price = cProd.price;
                    oi.Quantity = cProd.quantity;
                    orderVM.orderItems.Add(oi);
                    if (cProd.price != 0 && cProd.quantity != 0)
                        total = ((double)cProd.price * cProd.quantity) + total;
                }
                orderVM.order = new order();
                orderVM.order.TotalPrice = total;
                orderVM.order.GST = 5;
                orderVM.order.GrandTotalWithTax = total;
                orderVM.order.DeliveryDate = DateTime.Now.AddDays(30);
                orderVM.order.Discount = 0;
                orderVM.order.Advance = 0;
            }
            return View(orderVM);
        }

        public ActionResult NewOrder()
        {
            IEnumerable<saleproduct> productsList = db.saleproducts.ToList();
            return View(productsList);
        }

        // GET: orders/Create
        public ActionResult Create()
        {
            ViewBag.Edit = false;
            ViewBag.ProductID = new SelectList(db.products, "ID", "Name");
            OrdersVM objOrders = new OrdersVM();
            objOrders.order = new order();
            objOrders.orderItems = new List<orderitem> { new orderitem() };
            return View(objOrders);
        }

        // POST: orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderVM orderVM)
        {
            if (ModelState.IsValid)
            {
                var CustAddress = new customeraddress();
                CustAddress.customer = orderVM.customer;
                CustAddress.address = orderVM.address;
                if (orderVM.customer.ID == 0)
                {
                    db.customeraddresses.Add(CustAddress);
                    db.SaveChanges();
                }

                order objOrder = orderVM.order;
                objOrder.orderitems = orderVM.orderItems;
                objOrder.OrderNo = GenerateOrderNo();
                objOrder.AddressID = CustAddress.address.ID;
                objOrder.CustomerID = CustAddress.customer.ID;
                db.orders.Add(objOrder);
                db.SaveChanges();
                // return RedirectToAction("OrderSuccess", orderVM);
                Session["cart"] = null;
                Session["count"] = null;
                return RedirectToAction("Details","Orders", new { id = objOrder.ID, newOrder= true });
            }
            return View(orderVM);
        }

        // GET: orders/Details/5
        public ActionResult Details(int? id, bool newOrder = false)
        {
            ViewBag.NewOrder = newOrder;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderVM objOrder = new OrderVM();
            objOrder.order = db.orders.Find(id);
            objOrder.orderItems = db.orderitems.Where(x => x.OrderID == id).ToList();
            objOrder.address = db.addresses.Find(objOrder.order.AddressID);
            objOrder.customer = db.customers.Find(objOrder.order.CustomerID);
             
            return View("OrderSummary", objOrder);
        }

        // GET: orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderNo,CustomerOrganizationName,Address,City,State,Country,Zip,ContactPersonName,ContactNo,ProductType,Size,Quantity,DeliveryDate,PricePerUnit,TotalPrice,GrandTotalWithTax,Advance,Remarks,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var orderItems = db.orderitems.Where(x => x.OrderID == id).ToList();
            foreach(orderitem oi in orderItems)
            {
                db.orderitems.Remove(oi);
            }
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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
            var result = from c in db.customers
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
            var result = from c in db.customers
                         join ca in db.customeraddresses on c.ID equals ca.CustomerID
                         join a in db.addresses on ca.AddressID equals a.ID
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
            if (db.orders.Any(x => x.OrderNo == OrderNo))
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
