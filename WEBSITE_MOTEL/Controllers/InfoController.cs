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
        public ActionResult ManageInfo()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN nd = (TAIKHOAN)Session["TaiKhoan"];


                var adminn = (from a in data.ADMINs
                              join b in data.TAIKHOANs on a.Id_TaiKhoan equals b.Id
                              

                              select new Admin()
                              {
                                  sTaiKhoanAD = b.TaiKhoan,
                                  sHotenAD = b.HoTen,
                                  sMatKhauAD = b.MatKhau,
                                  sId = b.Id,
                                  sSDTAD = a.SDT,
                                  sEmailAD = a.Email
                              }).FirstOrDefault();


                return View(adminn);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageInfo(FormCollection f)
        {
            var thongtin = data.TAIKHOANs.SingleOrDefault(n => n.Id == int.Parse(f["sId"]));
            if(ModelState.IsValid)
            {
                thongtin.HoTen = f["sHotenAD"];
                thongtin.MatKhau = f["sMatKhauAD"];
                data.SubmitChanges();
                return RedirectToAction("ManageInfo");
            }
            return View(thongtin);
        }
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

                              join b in data.CHUTROs on a.Id equals b.Id_TaiKhoan
                              where (nd.Id== b.Id_TaiKhoan)
                              select new ChuTro()
                              {
                                  sTaiKhoanCT = a.TaiKhoan,
                                  sHotenCT = a.HoTen,
                                  sMatKhauCT = a.MatKhau,
                                  sId = b.Id,
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
        public ActionResult ManageInfoCT(FormCollection f)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            if (tk == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect if not logged in
            }

            // Fetch the current user's information
            var taikhoan = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);

            // Validate input fields
            if (string.IsNullOrEmpty(f["sHotenCT"]))
            {
                ModelState.AddModelError("sHotenCT", "Vui lòng nhập họ và tên.");
            }
            if (string.IsNullOrEmpty(f["sMatKhauCT"]))
            {
                ModelState.AddModelError("sMatKhauCT", "Vui lòng nhập mật khẩu mới.");
            }
            if (string.IsNullOrEmpty(f["sSDTCT"]))
            {
                ModelState.AddModelError("sSDTCT", "Vui lòng nhập số điện thoại.");
            }
            if (string.IsNullOrEmpty(f["sEmailCT"]))
            {
                ModelState.AddModelError("sEmailCT", "Vui lòng nhập email.");
            }

            // Check if the ModelState is valid
            if (!ModelState.IsValid)
            {
                // Return the current model with validation errors
                return View();
            }

            // Update the TAIKHOAN entity
            
            if (taikhoan != null)
            {
                taikhoan.HoTen = f["sHotenCT"];
                taikhoan.MatKhau = f["sMatKhauCT"]; // Ensure you handle password hashing
                taikhoan.SDT = f["sSDTCT"];
                taikhoan.Email = f["sEmailCT"];
                taikhoan.CCCD = f["sCCCD"];
                taikhoan.DiaChi = f["sDiaChi"];
                taikhoan.NgaySinh = Convert.ToDateTime(f["sNgaySinh"]);

                // Submit changes to the database
                data.SubmitChanges();

                // Update the session with the new account information
                Session["TaiKhoan"] = taikhoan;
                TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công!";
                // Redirect to the same action to refresh the page
                return RedirectToAction("ManageInfoCT");
            }
            return View();
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

                              join b in data.NGUOIDUNGs on a.Id equals b.Id_TaiKhoan
                                 where (nd.Id == b.Id_TaiKhoan)
                                 select new NguoiDung()
                              {
                                  sTaiKhoanND = a.TaiKhoan,
                                  sHotenND = a.HoTen,
                                  sMatKhauND = a.MatKhau,
                                  sId = b.Id,
                                  sSDTND=a.SDT,
                                  sEmailND=a.Email
                              }).FirstOrDefault();


                return View(nguoidung);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageInfoND(FormCollection f)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];

            if (tk == null)
            {
                return RedirectToAction("Login", "Account"); // Chuyển hướng tới trang đăng nhập nếu chưa đăng nhập
            }

            var taikhoan = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);

            // Kiểm tra các trường dữ liệu
            if (string.IsNullOrEmpty(f["sHotenND"]))
            {
                ModelState.AddModelError("sHotenND", "Vui lòng nhập họ và tên.");
            }
            if (string.IsNullOrEmpty(f["sMatKhauND"]))
            {
                ModelState.AddModelError("sMatKhauND", "Vui lòng nhập mật khẩu mới.");
            }
            if (string.IsNullOrEmpty(f["sSDTND"]))
            {
                ModelState.AddModelError("sSDTND", "Vui lòng nhập số điện thoại.");
            }
            if (string.IsNullOrEmpty(f["sEmailND"]))
            {
                ModelState.AddModelError("sEmailND", "Vui lòng nhập email.");
            }

            // Nếu có lỗi, trả về view với thông báo lỗi
            if (!ModelState.IsValid)
            {
                return View(); // Trả về view với thông báo lỗi
            }

            // Cập nhật thông tin nếu không có lỗi
            if (taikhoan != null)
            {
                taikhoan.HoTen = f["sHotenND"];
                taikhoan.MatKhau = f["sMatKhauND"];
                taikhoan.SDT = f["sSDTND"];
                taikhoan.Email = f["sEmailND"];

                data.SubmitChanges();
                Session["TaiKhoan"] = taikhoan;
                TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công!";
                return RedirectToAction("ManageInfoND"); // Tải lại trang với thông báo thành công
            }
            return View(); // Trả về view nếu không tìm thấy tài khoản
        }

    }
}