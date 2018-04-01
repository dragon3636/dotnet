using Model.DAO;
using OnlineShow.Common;
using OnlineShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Model.EF;
using System.IO;
using System.Configuration;
using Common;

namespace OnlineShow.Controllers
{
    public class CartController : Controller
    {

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CarItem>();
            if (cart!=null)
            {
                list = (List<CarItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(long productId, int quantity)
        {
            var cart = Session[CommonConstants.CartSession];
            var product = new ProductDao().ViewDetail(productId);
            if (cart != null)
            {
                var list = (List<CarItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity+=quantity;
                        }
                    }

                }
                else
                {
                    // tạo mới  đối tượng cart item
                    var item = new CarItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                // gán  vào  sesion
                Session[CommonConstants.CartSession] = list;
            }
            else
            {
                // tạo mới  đối tượng cart item 
                var item = new CarItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CarItem>();
                list.Add(item);
                // gán  vào  sesion
                Session[CommonConstants.CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CarItem>>(cartModel);
            var sessionCart = (List<CarItem>)Session[CommonConstants.CartSession];
            foreach (var item in sessionCart)
            {
                var itemJson = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (itemJson!=null)
                {
                    item.Quantity = itemJson.Quantity;
                }
            }
            Session[CommonConstants.CartSession] = sessionCart;
            return Json( new {
            status = true});
        }
        public JsonResult DeleteAll(string cartModel)
        {
           
            Session[CommonConstants.CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CarItem>)Session[CommonConstants.CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CarItem>();
            if (cart != null)
            {
                list = (List<CarItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment( string name,string mobile,string address,string email)
        {
            //  tạo đối tượng Oder
            var oder = new Order();
            oder.ShipName = name;
            oder.ShipMobie = mobile;
            oder.ShipAddress = address;
            oder.ShipEmail = email;
            oder.CreateDate = DateTime.Now;
            try
            {
                var oderid = new OderDao().InsertOder(oder);
                var cart = (List<CarItem>)Session[CommonConstants.CartSession];
                var oderDetail = new OderDetail();
                decimal total =0 ;
                foreach (var item in cart)
                {
                    var product = item.Product;
                    oderDetail.ProductID = product.ID;
                    oderDetail.OderID = oderid;
                    oderDetail.Price = product.Price;
                    oderDetail.Quantity = item.Quantity;
                    var result = new OderDetailDao().Insert(oderDetail);
                    total += (item.Product.Price.GetValueOrDefault(0)* item.Quantity);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/newOrder.html"));
                content = content.Replace("{{CustomerName}}", name);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                // Gữi  email xác  nhận  đã đặt mua  đến khách hàng 
                new MailHelper().SendMail(email, "Đơn  đặt hàng của bạn trên OnlineShop", content);
                // Gữi email  thông báo  tới  quản trị  có đơn hàng 
                new MailHelper().SendMail(toEmail, "Đơn  đặt hàng mới của khách hàng", content);

            }
            catch (Exception )
            {
                // ghi log
                return Redirect("/loi-thanh-toan");
            }
          
            return  Redirect("/hoan-thanh");
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}