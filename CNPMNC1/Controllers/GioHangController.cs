using CNPMNC1.Models;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FireSharp.Config;
using static CNPMNC1.Models.CartItem;

namespace CNPMNC1.Controllers
{
    public class GioHangController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "uhhaykO6PxwhS9VIaTTPYIPWWGnyBnXuINuzRmPO",
            BasePath = "https://congnghephanmennangcao-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        // GET: GiỏHang
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return View("EmptyCart");
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("SanPham/" + id);
            SanPham data = JsonConvert.DeserializeObject<SanPham>(response.Body);
            if (data != null)
            {
                GetCart().Add_Product_Cart(data);
            }
            return RedirectToAction("ShowCart", "GioHang");
        }
        public ActionResult update_cart_quantity(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            string idpro = (form["ID"]);
            int quantity = int.Parse(form["carquantity"]);
            cart.Update_quantity(idpro, quantity);
            return RedirectToAction("ShowCart", "GioHang");
        }
        public ActionResult RemoveCart(string id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_cartitem(id);
            return RedirectToAction("ShowCart", "GioHang");
        }
        public PartialViewResult BagCart()
        {
            int total_quantity = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
                total_quantity = cart.Total_quantity();
            ViewBag.QuantityCart = total_quantity;
            return PartialView("BagCart");
        }
    }
}