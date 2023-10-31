﻿using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPMNC1.Models;
using FireSharp.Config;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity;
using Firebase.Auth;
using FireSharp.Response;

namespace CNPMNC1.Controllers
{
    public class NguoiDungController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "uhhaykO6PxwhS9VIaTTPYIPWWGnyBnXuINuzRmPO",
            BasePath = "https://congnghephanmennangcao-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        // GET: NguoiDung
        public ActionResult Danhsachdonhang()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("DanhSachDonHang/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<DanhSachDonHang>();
            var id = Session["ID"] as string;
            if (data == null)
            {
                return RedirectToAction("Index2", "SanPham");
            }
            foreach (var item in data)
            {
                    list.Add(JsonConvert.DeserializeObject<DanhSachDonHang>(((JProperty)item).Value.ToString()));
            }
            foreach(var item in list)
            {
                if(item.IDkhachhang != id)
                {
                    list.Remove(item);
                }
            }
            return PartialView(list);
        }
        public ActionResult Danhsachdonhang2()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("DanhSachDonHang/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<DanhSachDonHang>();
            var id = Session["ID"] as string;
            if (data == null)
            {
                return RedirectToAction("Index", "SanPham");
            }
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<DanhSachDonHang>(((JProperty)item).Value.ToString()));
            }
            return PartialView(list);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("DanhSachDonHang/" + id);
            DanhSachDonHang data = JsonConvert.DeserializeObject<DanhSachDonHang>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(DanhSachDonHang sanpham)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Set("DanhSachDonHang/" + sanpham.ID, sanpham);
            DanhSachDonHang data = JsonConvert.DeserializeObject<DanhSachDonHang>(response.Body);
            return RedirectToAction("Danhsachdonhang2");
        }
        public ActionResult chitietdonhang(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("ChiTietDonHang/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<chitietdonhang>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<chitietdonhang>(((JProperty)item).Value.ToString()));
            }
            foreach (var item in list)
            {
                if (item.iddanhsachdh != id)
                {
                    list.Remove(item);
                }
            }
            return PartialView(list);
        }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response4 = client.Get("DanhSachDonHang/" + id);
            DanhSachDonHang data4 = JsonConvert.DeserializeObject<DanhSachDonHang>(response4.Body);
            if(data4.Tinhtrang == "Đang Duyệt" || data4.Tinhtrang =="Đã Duyệt")
            {
                FirebaseResponse response5 = client.Delete("DanhSachDonHang/" + id);
                DanhSachDonHang data5 = JsonConvert.DeserializeObject<DanhSachDonHang>(response5.Body);
            }
            return RedirectToAction("Danhsachdonhang");
        }
        [HttpGet]
        public ActionResult Delete2(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response4 = client.Delete("DanhSachDonHang/" + id);
            DanhSachDonHang data4 = JsonConvert.DeserializeObject<DanhSachDonHang>(response4.Body);
            return RedirectToAction("Danhsachdonhang2");
        }
    }
}