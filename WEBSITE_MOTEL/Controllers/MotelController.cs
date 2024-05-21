using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;
using WEBSITE_MOTEL.Models;
using Microsoft.Ajax.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace WEBSITE_MOTEL.Controllers
{
    public class MotelController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Motel
        public ActionResult Index(int? page, string strSearch)
        {
            ViewData["strSearch"] = strSearch;
            /*ViewBag.IdPH = new SelectList(data.PHONGTROs.ToList().OrderBy(n => n.TenPhong), "Id", "TenPhong");*/
            int iSize = 3;
            int iPageNum = (page ?? 1);
            if (!string.IsNullOrEmpty(strSearch))
            {
                var phong = (from a in data.PHONGTROs
                             join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro

                             join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id

                             where a.TrangThai == 1 && a.TenPhong.Contains(strSearch) || a.MoTa.Contains(strSearch) /*|| a.DienTich.Equals(strSearch) */
                             select new RoomDetail()
                             {
                                 sMa = a.Id,
                                 sTenPhong = a.TenPhong,
                                 sHoTen = e.HoTen,

                                 sDienTich = (int)a.DienTich,
                                 sSoluong = (int)a.SoLuong,
                                 sAnhBia = a.AnhBia,
                                 sMoTa = a.MoTa,
                                 dNgayCapNhat = (DateTime)a.Ngay,
                                 dGiaCa = (double)a.GiaCa,
                                 sSDT = e.SDT,
                                 sEmail = e.Email,
                                 sUrl_Path = c.Url_Path,
                                 sUrl_Path2 = c.Url_Path2,
                                 sUrl_Path3 = c.Url_Path3,
                                 sUrl_Path4 = c.Url_Path4,
                                 sDiaChi = a.Diachi,
                                 sDien = (double)a.Dien,
                                 sNuoc = (double)a.Nuoc,
                                 sGuiXe = (double)a.GuiXe,
                                 sInternet = (double)a.Internet,
                                 sDoiTuong = (byte)a.Doituong,
                                 sTrangThai = (byte)a.TrangThai,

                                 sTenKV = a.KhuVuc,
                             });
                return View(phong.OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
            }
            else
            {
                var phong = (from a in data.PHONGTROs
                             join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                             join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id
                             where a.TrangThai == 1
                             select new RoomDetail()
                             {
                                 sMa = a.Id,
                                 sTenPhong = a.TenPhong,
                                 sHoTen = e.HoTen,
                                 sDienTich = (int)a.DienTich,
                                 sSoluong = (int)a.SoLuong,
                                 sAnhBia = a.AnhBia,
                                 sMoTa = a.MoTa,
                                 dNgayCapNhat = (DateTime)a.Ngay,
                                 dGiaCa = (double)a.GiaCa,
                                 sSDT = e.SDT,
                                 sEmail = e.Email,
                                 sUrl_Path = c.Url_Path,
                                 sUrl_Path2 = c.Url_Path2,
                                 sUrl_Path3 = c.Url_Path3,
                                 sUrl_Path4 = c.Url_Path4,
                                 sDiaChi = a.Diachi,
                                 sDien = (double)a.Dien,
                                 sNuoc = (double)a.Nuoc,
                                 sGuiXe = (double)a.GuiXe,
                                 sInternet = (double)a.Internet,
                                 sDoiTuong = (byte)a.Doituong,
                                 sTrangThai = (byte)a.TrangThai,

                                 sTenKV = a.KhuVuc
                             });

                return View(phong.OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
            }
        }

        /*private List<PHONGTRO> LayPhong(int count)
        {

            return data.PHONGTROs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
*/
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        // GET: User
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            int state = Convert.ToInt32(Request.QueryString["id"]);
            ViewBag.TenDN = "Người dùng";
            var sTenDN = collection["TaiKhoan"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (tk != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = tk;
                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TaiKhoan"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TaiKhoan"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                        return RedirectToAction("Index", "Motel");
                    }
                    else
                    {
                        Response.Cookies["TaiKhoan"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                        return RedirectToAction("Post", "Motel");
                    }

                }
                else
                {
                    ViewBag.ThongBao = "Sai mật khẩu hoặc tài khoản";
                }
            }
            return View();

        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogout");
        }
        [HttpGet]
        public ActionResult Support()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddSupport(string name, string phone, string mota)
        {
            try
            {
                var ht = new HOTRO();
                ht.Ten = name;
                ht.Sdt = phone;
                ht.MotaVande = mota;
                data.HOTROs.InsertOnSubmit(ht);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Yêu cầu đã được gửi đi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Gửi thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchPartial()
        {
            var listKV = from a in data.KHUVUCs select a;
            return PartialView(listKV);
        }
        
        public ActionResult SearchKQ(int? page, string IdKV, string IdGia, string IdSL, string IdDT)
        {
            int iSize = 3;
            int iPageNum = (page ?? 1);
            var listSearch = (from a in data.PHONGTROs
                              join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                              join c in data.IMAGEs on a.Id equals c.Id_PhongTro

                              join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id
                              where a.TrangThai == 1
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,

                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = c.Url_Path,
                                  sUrl_Path2 = c.Url_Path2,
                                  sUrl_Path3 = c.Url_Path3,
                                  sUrl_Path4 = c.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sDoiTuong = (byte)a.Doituong,
                                  sTrangThai = (byte)a.TrangThai,

                                  sTenKV = a.KhuVuc,
                              });

            if (IdKV.Contains("Quận/Huyện") &&  IdSL.Contains("Số người ở") && IdDT.Contains("Diện tích(m2)") && !String.Equals(IdGia, "Mức giá?"))
            {
                string[] arrStr = IdGia.Split('-');
                listSearch = (from a in data.PHONGTROs
                              join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                              join c in data.IMAGEs on a.Id equals c.Id_PhongTro

                              join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id
                              where a.TrangThai == 1&& a.GiaCa > int.Parse(arrStr[0]) && a.GiaCa < int.Parse(arrStr[1])
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,

                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = c.Url_Path,
                                  sUrl_Path2 = c.Url_Path2,
                                  sUrl_Path3 = c.Url_Path3,
                                  sUrl_Path4 = c.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sDoiTuong = (byte)a.Doituong,
                                  sTrangThai = (byte)a.TrangThai,

                                  sTenKV = a.KhuVuc,
                              });
            }
            //else if (String.Equals(IdGia, "Trên 2000000"))
            //{
            //    string[] arrStr = IdGia.Split('>');
            //    int num = int.Parse(arrStr[1]);
            //    listSearch = from s in data.PHONGTROs
            //                 where int.Parse(arrStr[1]) > s.GiaCa && int.Parse(arrStr[0]) < s.GiaCa
            //                 select s;
            //}
            else if (!String.Equals(IdKV, "Quận/Huyện") && IdGia.Contains("Mức giá?") && IdDT.Contains("Diện tích(m2)") && IdSL.Contains("Số người ở"))
            {
                listSearch = (from a in data.PHONGTROs
                              join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                              join c in data.IMAGEs on a.Id equals c.Id_PhongTro

                              join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id
                              where a.TrangThai == 1 && a.KhuVuc == IdKV
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,

                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = c.Url_Path,
                                  sUrl_Path2 = c.Url_Path2,
                                  sUrl_Path3 = c.Url_Path3,
                                  sUrl_Path4 = c.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sDoiTuong = (byte)a.Doituong,
                                  sTrangThai = (byte)a.TrangThai,

                                  sTenKV = a.KhuVuc,
                              });
            }
            else if (!String.Equals(IdSL,"Số người ở") && IdGia.Contains("Giá cả?") && IdKV.Contains("Quận/Huyện") && IdDT.Contains("Diện tích(m2)"))
            {
                listSearch = (from a in data.PHONGTROs
                              join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                              join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                              join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id

                              where a.TrangThai == 1 && a.SoLuong == int.Parse(IdSL)
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,

                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = c.Url_Path,
                                  sUrl_Path2 = c.Url_Path2,
                                  sUrl_Path3 = c.Url_Path3,
                                  sUrl_Path4 = c.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sDoiTuong = (byte)a.Doituong,
                                  sTrangThai = (byte)a.TrangThai,

                                  sTenKV = a.KhuVuc,
                              });
            }
            else if (!String.Equals(IdKV, "Quận/Huyện") && !String.Equals(IdGia, "Mức giá?"))
            {
                string[] arrStr = IdGia.Split('-');
                listSearch = (from a in data.PHONGTROs
                              join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                              join c in data.IMAGEs on a.Id equals c.Id_PhongTro

                              join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id
                              where a.TrangThai == 1 && a.GiaCa > int.Parse(arrStr[0]) && a.GiaCa < int.Parse(arrStr[1]) && a.KhuVuc == IdKV
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,

                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = c.Url_Path,
                                  sUrl_Path2 = c.Url_Path2,
                                  sUrl_Path3 = c.Url_Path3,
                                  sUrl_Path4 = c.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sDoiTuong = (byte)a.Doituong,
                                  sTrangThai = (byte)a.TrangThai,

                                  sTenKV = a.KhuVuc,
                              });
            }
            else if (!String.Equals(IdSL, "Số người ở") && !String.Equals(IdGia, "Mức giá?"))
            {
                string[] arrStr = IdGia.Split('-');
                listSearch = (from a in data.PHONGTROs
                              join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                              join c in data.IMAGEs on a.Id equals c.Id_PhongTro

                              join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id
                              where a.TrangThai == 1 && a.SoLuong == int.Parse(IdSL) && a.TenPhong == IdKV
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,

                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = c.Url_Path,
                                  sUrl_Path2 = c.Url_Path2,
                                  sUrl_Path3 = c.Url_Path3,
                                  sUrl_Path4 = c.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sDoiTuong = (byte)a.Doituong,
                                  sTrangThai = (byte)a.TrangThai,

                                  sTenKV = a.KhuVuc,
                              });
            }
            else if (!String.Equals(IdKV, "Quận/Huyện") && !String.Equals(IdSL, "Số người ở"))
            {
                string[] arrStr = IdGia.Split('-');
                listSearch = (from a in data.PHONGTROs
                              join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                              join c in data.IMAGEs on a.Id equals c.Id_PhongTro

                              join e in data.TAIKHOANs on b.Id_TaiKhoan equals e.Id
                              where a.TrangThai == 1 && a.GiaCa > int.Parse(arrStr[0]) && a.GiaCa < int.Parse(arrStr[1]) && a.SoLuong == int.Parse(IdSL)
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,

                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = c.Url_Path,
                                  sUrl_Path2 = c.Url_Path2,
                                  sUrl_Path3 = c.Url_Path3,
                                  sUrl_Path4 = c.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sDoiTuong = (byte)a.Doituong,
                                  sTrangThai = (byte)a.TrangThai,

                                  sTenKV = a.KhuVuc,
                              });
            }

            // Kiểm tra xem kết quả tìm kiếm có rỗng không
            if (!listSearch.Any())
            {
                ViewBag.ErrorMessage = "Không tìm thấy kết quả phù hợp.";
                return View("Index", listSearch.ToPagedList(iPageNum, iSize));
            }

            return View("Index", listSearch.OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
        }
        public ActionResult Map()
        {
            return View();
        }
        public ActionResult TEST()
        {
            return View();
        }
        public ActionResult Guide()
        {
            return View();
        }
        public ActionResult News()
        {
            return View();
        }
        public ActionResult StartPage()
        {
            return View();
        }
        
        public ActionResult NewsPartial()
        {
            return PartialView();
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult QDinh()
        {
            return PartialView();
        }
        public ActionResult GThieu()
        {
            return PartialView();
        }
        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }
        [HttpGet]
        public JsonResult BookRoom()
        {
            try
            {
                TAIKHOAN tk = GetAcc();
                var bookroom = (from a in data.TAIKHOANs
                                join b in data.NGUOIDUNGs on a.Id equals b.Id_TaiKhoan
                                where (tk.Id == b.Id_TaiKhoan)
                                select new
                                {
                                    sHotenND = a.HoTen,
                                    sId = b.Id,
                                    sSDTND = a.SDT,

                                }).SingleOrDefault();
                return Json(new { code = 200, dh = bookroom, msg = "Đặt thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult BookRoom1(int idphong)
        {
            try
            {
                var bookroom = (from a in data.PHONGTROs
                                where (a.Id == idphong)
                                select new
                                {
                                    sTenPhong = a.TenPhong,
                                    sGia = a.GiaCa

                                }).SingleOrDefault();
                return Json(new { code = 200, dh = bookroom, msg = "Đặt thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveData(int idphong)
        {

            try
            {

                TAIKHOAN tk = GetAcc();
                var idnd = data.NGUOIDUNGs.SingleOrDefault(n => n.Id_TaiKhoan == tk.Id);
                var phong = data.PHONGTROs.SingleOrDefault(n => n.Id == idphong);
                var dh = new DONHANG();
                dh.NgayDat = DateTime.Now;
                dh.Id_NguoiDung = idnd.Id;
                dh.Id_Phong = idphong;
                phong.TrangThai = 2;
                data.DONHANGs.InsertOnSubmit(dh);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Đặt thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}