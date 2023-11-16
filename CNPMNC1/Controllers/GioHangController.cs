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
using static CNPMNC1.Models.Cart;
using static CNPMNC1.Models.CacphuongTT;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using FireSharp;

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
        [HttpGet]
        public ActionResult Create()
        {
            CacphuongTT  product = new CacphuongTT();
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(CacphuongTT thanhtoan)
        {
            try
            {
                AddStudentToFirebase(thanhtoan);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        private void AddStudentToFirebase(CacphuongTT Thanhtoan)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = Thanhtoan;
            PushResponse response = client.Push("ThanhToan/", data);
            data.idtt = response.Result.name;
            SetResponse setResponse = client.Set("ThanhToan/" + data.idtt, data);
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
        public ActionResult DatHang(string tt) 
        {
            Cart cart = Session["Cart"] as Cart;
            DanhSachDonHang danhsach = new DanhSachDonHang();
            danhsach.ngaydathang = DateTime.Now;
            danhsach.phuongthuctt = tt;
            danhsach.IDkhachhang = Session["ID"] as string;
            danhsach.Tinhtrang = "Đang Duyệt";
            Nullable<decimal> tien = 0;
            foreach (var item in cart.Items)
            {
                 tien += (item.sanPham.gia * item.quantity);
            }
            danhsach.tongtien = tien;
            AddStudentToFirebase(danhsach);
            foreach (var item in cart.Items)
            {
                chitietdonhang chitiet = new chitietdonhang();
                chitiet.idsanpham = item.sanPham.idsanpham;
                chitiet.iddanhsachdh = danhsach.ID;
                chitiet.tongtien = (item.sanPham.gia * item.quantity);
                chitiet.soluong = item.quantity;
                chitiet.tensanpham = item.sanPham.tensanpham;
                chitiet.ImagePro = item.sanPham.ImagePro;
                chitiet.namsx = item.sanPham.namsx;
                chitiet.loai_sanpham = item.sanPham.loai_sanpham;
                chitiet.gia = item.sanPham.gia;
                chitiet.mota = item.sanPham.mota;
                chitiet.thuonghieu = item.sanPham.thuonghieu;
                AddStudentToFirebase(chitiet);
            }
            cart.ClearCart();
            return RedirectToAction("Index","SanPham");
        }
        private void AddStudentToFirebase(DanhSachDonHang Thanhtoan)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = Thanhtoan;
            PushResponse response = client.Push("DanhSachDonHang/", data);
            data.ID = response.Result.name;
            SetResponse setResponse = client.Set("DanhSachDonHang/" + data.ID, data);
        }
        private void AddStudentToFirebase(chitietdonhang Thanhtoan)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = Thanhtoan;
            PushResponse response = client.Push("ChiTietDonHang/", data);
            data.id = response.Result.name;
            SetResponse setResponse = client.Set("ChiTietDonHang/" + data.id, data);
        }
        [HttpGet]
        public ActionResult Create2()
        {
            GiamGiaThanhVien product = new GiamGiaThanhVien();
            return View(product);
        }
        [HttpPost]
        public ActionResult Create2(GiamGiaThanhVien giamgiathanhvien)
        {
            try
            {
                AddStudentToFirebase(giamgiathanhvien);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        private void AddStudentToFirebase(GiamGiaThanhVien Thanhtoan)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = Thanhtoan;
            PushResponse response = client.Push("GiamGiaThanhVien/", data);
            data.Id = response.Result.name;
            SetResponse setResponse = client.Set("GiamGiaThanhVien/" + data.Id, data);
        }
    }
}