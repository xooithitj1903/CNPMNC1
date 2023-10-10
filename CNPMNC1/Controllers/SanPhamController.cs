using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CNPMNC1.Models;
using System.IO;
using System.Data.Entity;

namespace CNPMNC1.Controllers
{
    public class SanPhamController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "uhhaykO6PxwhS9VIaTTPYIPWWGnyBnXuINuzRmPO",
            BasePath = "https://congnghephanmennangcao-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        // GET: SanPham
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("SanPham/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<SanPham>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<SanPham>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SanPham product = new SanPham();
            return View(product);
        }
        [HttpPost]
        public ActionResult Create(SanPham sanPham)
        {
            try
            {
                if (sanPham.UploadImages != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(sanPham.UploadImages.FileName);
                    String exten = Path.GetExtension(sanPham.UploadImages.FileName);
                    filename = filename = exten;
                    sanPham.ImagePro = "~/Content/images/" + filename;
                    sanPham.UploadImages.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), filename));
                }
                AddStudentToFirebase(sanPham);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        private void AddStudentToFirebase(SanPham sanPham)
        {
            client = new FireSharp.FirebaseClient(config);
            var data = sanPham;
            PushResponse response = client.Push("SanPham/", data);
            data.idsanpham = response.Result.name;
            SetResponse setResponse = client.Set("SanPham/" + data.idsanpham, data);
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("SanPham/" + id);
            SanPham data = JsonConvert.DeserializeObject<SanPham>(response.Body);
            return View(data);
        }


        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("SanPham/" + id);
            SanPham data = JsonConvert.DeserializeObject<SanPham>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(SanPham sanpham)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Set("SanPham/" + sanpham.idsanpham, sanpham);
            SanPham data = JsonConvert.DeserializeObject<SanPham>(response.Body);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("SanPham/" + id);
            SanPham data = JsonConvert.DeserializeObject<SanPham>(response.Body);
            return RedirectToAction("Index");
        }
    }
}