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
            //Phòng đang được đăng tin
            var pendingRooms = data.PHONGTROs.Where(pt => pt.TrangThai == 1).ToList();
            int pendingRoomCount = pendingRooms.Count();
            ViewBag.PendingRoomCount = pendingRoomCount;

            //Phòng chờ duyệt
            var approvalRooms = data.PHONGTROs.Where(pt => pt.TrangThai == 0).ToList();
            int approvalRoomCount = approvalRooms.Count();
            ViewBag.ApprovalRoomCount = approvalRoomCount;

            //Phòng hết hạn đăng
            var expiredRooms = data.PHONGTROs.Where(pt => pt.TrangThai == 2).ToList();
            int expiredRoomCount = expiredRooms.Count();
            ViewBag.ExpiredRoomCount = expiredRoomCount;

            //Tài khoản chờ duyệt
            var pendingAccounts = data.TAIKHOANs.Where(ct => ct.TrangThai == 0).ToList();
            int pendingAccountCount = pendingAccounts.Count();
            ViewBag.PendingAccountCount = pendingAccountCount;

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