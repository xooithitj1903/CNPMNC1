using CNPMNC1.Models;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FireSharp.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity;
using Firebase.Auth;


namespace CNPMNC1.Controllers
{
    public class DangnhapDangkyController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "uhhaykO6PxwhS9VIaTTPYIPWWGnyBnXuINuzRmPO",
            BasePath = "https://congnghephanmennangcao-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        // GET: DangnhapDangky
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(TaiKhoan taikhoan)
        {
            try
            {
                taikhoan.thanhvien = "chưa là thành viên";
                AddStudentToFirebase(taikhoan);
                return RedirectToAction("Dangnhap");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(TaiKhoan taiKhoan)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("TaiKhoan/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<TaiKhoan>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<TaiKhoan>(((JProperty)item).Value.ToString()));
            }
            foreach (var items in list)
            {
                if(items.email == taiKhoan.email && items.pass == taiKhoan.pass)
                {
                    Session["ID"] = items.id;
                    Session["PasswordUser"] = items.pass;
                    Session["Email"] = items.email;
                    return RedirectToAction("Index", "SanPham");
                } else if(items.email == taiKhoan.email && items.pass != taiKhoan.pass)
                {
                    ViewBag.ErrorDangNhap("Sai Mật Khẩu");
                }
                else if (items.email != taiKhoan.email)
                {
                    ViewBag.ErrorDangNhap("Tài Khoản không tồn tại");
                }
            }
            return View();
        }
        private void AddStudentToFirebase(TaiKhoan taikhoan)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = taikhoan;
            PushResponse response = client.Push("TaiKhoan/", data);
            data.id = response.Result.name;
            SetResponse setResponse = client.Set("TaiKhoan/" + data.id, data);
        }
    }
}