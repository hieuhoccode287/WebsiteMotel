using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Controllers
{
    public class InfoController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        public NguoiDung NguoiDung { get; set; }
        public ChuTro Chutro { get; set; }
        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }
        // GET: Info
        [HttpGet]
        public ActionResult ManageInfoCT()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN nd = (TAIKHOAN)Session["TaiKhoan"];


                var chutro = (from a in data.TAIKHOANs
                              where a.PhanQuyen == 2
                              where (nd.Id== a.Id)
                              select new ChuTro()
                              {
                                  sTaiKhoanCT = a.TaiKhoan,
                                  sHotenCT = a.HoTen,
                                  sMatKhauCT = a.MatKhau,
                                  sId = a.Id,
                                  sSDTCT = a.SDT,
                                  sEmailCT = a.Email,
                                  sCCCD = a.CCCD,
                                  sDiaChi = a.DiaChi,
                                  sNgaySinh = (DateTime)a.NgaySinh
                              }).FirstOrDefault();


                return View(chutro);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageInfoCT(ChuTro model)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];

            if (tk == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if the user is not logged in
            }

            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors if the model state is invalid
            }
            var taikhoan = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);
            // Update the TAIKHOAN entity

            if (taikhoan != null)
            {
                taikhoan.HoTen = model.sHotenCT;
                taikhoan.MatKhau = model.sMatKhauCT;
                taikhoan.SDT = model.sSDTCT;
                taikhoan.Email = model.sEmailCT;
                taikhoan.CCCD = model.sCCCD;
                taikhoan.DiaChi = model.sDiaChi;
                taikhoan.NgaySinh = Convert.ToDateTime(model.sNgaySinh);

                // Submit changes to the database
                data.SubmitChanges();

                // Update the session with the new account information
                Session["TaiKhoan"] = taikhoan;
                TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công!";
                // Redirect to the same action to refresh the page
                return RedirectToAction("ManageInfoCT");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ManageInfoND()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN nd = (TAIKHOAN)Session["TaiKhoan"];
                var nguoidung = (from a in data.TAIKHOANs
                                 where (nd.Id == a.Id)
                                 select new NguoiDung()
                              {
                                  sTaiKhoanND = a.TaiKhoan,
                                  sHotenND = a.HoTen,
                                  sMatKhauND = a.MatKhau,
                                  sId = a.Id,
                                  sSDTND=a.SDT,
                                  sEmailND=a.Email
                              }).FirstOrDefault();


                return View(nguoidung);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageInfoND(NguoiDung model)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];

            if (tk == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if the user is not logged in
            }

            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors if the model state is invalid
            }

            var taikhoan = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);

            if (taikhoan != null)
            {
                taikhoan.HoTen = model.sHotenND;
                taikhoan.MatKhau = model.sMatKhauND; // Handle password hashing here
                taikhoan.SDT = model.sSDTND;
                taikhoan.Email = model.sEmailND;

                data.SubmitChanges();

                Session["TaiKhoan"] = taikhoan;
                TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công!";
                return RedirectToAction("ManageInfoND"); // Refresh the page with success message
            }

            return View(model); // Return the view if the account is not found
        }


    }
}