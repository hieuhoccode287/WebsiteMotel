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
                    CHUTRO ct = data.CHUTROs.FirstOrDefault(n => n.Id_TaiKhoan == tk.Id);
                    Session["ChuTro"] = ct;
                    Session["TaiKhoan"] = tk;

                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TaiKhoan"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TaiKhoan"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
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
        [HttpPost]
        public JsonResult ADDTaiKhoan(string HoTen, string Email, string SDT, string DiaChi, DateTime NgaySinh, string CCCD, string Taikhoan, string Matkhau, int Phanquyen, HttpPostedFileBase[] idCardPhotos, HttpPostedFileBase[] idCardPhotos2)
        {
            try
            {
                var tk = new TAIKHOAN();
                tk.HoTen = HoTen;
                tk.Email = Email;
                tk.SDT = SDT;
                tk.DiaChi = DiaChi;
                tk.NgaySinh = NgaySinh;
                tk.CCCD = CCCD;
                tk.TaiKhoan = Taikhoan;
                tk.MatKhau = Matkhau;
                tk.PhanQuyen = Phanquyen;

                data.TAIKHOANs.InsertOnSubmit(tk);
                data.SubmitChanges();

                if (Phanquyen == 3)
                {
                    var nd = new NGUOIDUNG();
                    nd.Id_TaiKhoan = tk.Id;
                    data.NGUOIDUNGs.InsertOnSubmit(nd);
                    data.SubmitChanges();
                }
                else if (Phanquyen == 2)
                {
                    var ct = new CHUTRO();
                    ct.Id_TaiKhoan = tk.Id;
                    ct.TrangThai = 0;
                    data.CHUTROs.InsertOnSubmit(ct);
                    data.SubmitChanges();
                }

                if (idCardPhotos != null && idCardPhotos.Length > 0 && idCardPhotos2 != null && idCardPhotos2.Length > 0)
                {
                    for (int i = 0; i < idCardPhotos.Length; i++)
                    {
                        var idCardPhoto = idCardPhotos[i];
                        var idCardPhoto2 = idCardPhotos2[i];
                        if (idCardPhoto != null && idCardPhoto.ContentLength > 0 && idCardPhoto2 != null && idCardPhoto2.ContentLength > 0)
                        {
                            var fileName1 = Path.GetFileName(idCardPhoto.FileName);
                            var fileName2 = Path.GetFileName(idCardPhoto2.FileName);
                            var path1 = Path.Combine(Server.MapPath("~/Images"), fileName1);
                            var path2 = Path.Combine(Server.MapPath("~/Images"), fileName2);
                            idCardPhoto.SaveAs(path1);
                            idCardPhoto2.SaveAs(path2);

                            var image = new ANH_CCCD
                            {
                                IdTaiKhoan = tk.Id,
                                IdCardPhoto = fileName1,
                                IdCardPhoto2 = fileName2
                                // Gán các thuộc tính khác liên quan đến hình ảnh
                            };

                            data.ANH_CCCDs.InsertOnSubmit(image);
                            data.SubmitChanges();
                        }
                    }
                }

                return Json(new { code = 200, msg = "Đăng kí thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Đăng kí thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}