using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Controllers
{
    public class UserController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: UserT


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        // GET: User
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TaiKhoan"];
            var sMatKhau = collection["MatKhau"];

            if (String.IsNullOrEmpty(sTenDN))
            {
                TempData["Message"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                TempData["Message"] = "Phải nhập mật khẩu";
            }
            else
            {
                TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);

                if (tk != null)
                {
                    TAIKHOAN ct = data.TAIKHOANs.FirstOrDefault(n => n.Id == tk.Id && n.PhanQuyen == 2);
                    Session["ChuTro"] = ct;
                    Session["TaiKhoan"] = tk;

                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TaiKhoan"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TaiKhoan"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }

                    if (ct != null)
                    {
                        return RedirectToAction("Guide", "Motel");
                    }    

                    return RedirectToAction("Index", "Motel");
                }
                else
                {
                    TempData["Message111"] = "Sai mật khẩu hoặc tài khoản";
                }
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Motel");
        }
        /*[HttpGet]*/
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CheckUsername(string tendn)
        {
            var existingUser = data.TAIKHOANs.SingleOrDefault(u => u.TaiKhoan == tendn);

            if (existingUser != null)
            {
                return Json(new { available = false, message = "Tên đăng nhập đã tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { available = true, message = "Tên đăng nhập có sẵn." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult ADDTaiKhoan(string HoTen, string Email, string SDT, string DiaChi, DateTime NgaySinh, string CCCD, string Taikhoan, string Matkhau, int Phanquyen, HttpPostedFileBase[] idCardPhotos, HttpPostedFileBase[] idCardPhotos2)
        {
            try
            {
                // Kiểm tra đầu vào
                if (string.IsNullOrEmpty(HoTen) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Taikhoan) || string.IsNullOrEmpty(Matkhau) ||
                    idCardPhotos == null || idCardPhotos.Length == 0 || idCardPhotos2 == null || idCardPhotos2.Length == 0)
                {
                    return Json(new { code = 400, msg = "Đăng ký thất bại. Vui lòng cung cấp đầy đủ thông tin." }, JsonRequestBehavior.AllowGet);
                }

                var tk = new TAIKHOAN
                {
                    HoTen = HoTen,
                    Email = Email,
                    SDT = SDT,
                    DiaChi = DiaChi,
                    NgaySinh = NgaySinh,
                    CCCD = CCCD,
                    TaiKhoan = Taikhoan,
                    MatKhau = Matkhau,
                    PhanQuyen = Phanquyen,
                    TrangThai = 0
                };

                // Thêm tài khoản vào cơ sở dữ liệu
                data.TAIKHOANs.InsertOnSubmit(tk);
                data.SubmitChanges();

                // Lưu ảnh ID
                for (int i = 0; i < idCardPhotos.Length; i++)
                {
                    var idCardPhoto = idCardPhotos[i];
                    var idCardPhoto2 = idCardPhotos2[i];

                    // Kiểm tra xem cả hai ảnh có hợp lệ không
                    if (idCardPhoto != null && idCardPhoto.ContentLength > 0 && idCardPhoto2 != null && idCardPhoto2.ContentLength > 0)
                    {
                        var fileName1 = Path.GetFileName(idCardPhoto.FileName);
                        var fileName2 = Path.GetFileName(idCardPhoto2.FileName);
                        var path1 = Path.Combine(Server.MapPath("~/Images"), fileName1);
                        var path2 = Path.Combine(Server.MapPath("~/Images"), fileName2);

                        // Lưu ảnh vào server
                        idCardPhoto.SaveAs(path1);
                        idCardPhoto2.SaveAs(path2);

                        // Thêm thông tin ảnh vào cơ sở dữ liệu
                        var image = new ANH_CCCD
                        {
                            IdTaiKhoan = tk.Id,
                            IdCardPhoto = fileName1,
                            IdCardPhoto2 = fileName2
                        };

                        data.ANH_CCCDs.InsertOnSubmit(image);
                    }
                    else
                    {
                        // Nếu ảnh không hợp lệ, xóa tài khoản đã tạo
                        data.TAIKHOANs.DeleteOnSubmit(tk);
                        data.SubmitChanges();
                        return Json(new { code = 400, msg = "Đăng ký thất bại. Ảnh không hợp lệ." }, JsonRequestBehavior.AllowGet);
                    }
                }

                // Lưu tất cả các ảnh đã thêm vào cơ sở dữ liệu
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Đăng ký thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Đăng ký thất bại. Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}