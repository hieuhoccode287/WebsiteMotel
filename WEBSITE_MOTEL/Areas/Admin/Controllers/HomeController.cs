using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {   
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
            if (tk != null)
            {
                Session["Admin"] = tk;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Ten Dang Nhap Hoac Mat Khau Khong Dung";
            }
            return View();
        }
    }
}