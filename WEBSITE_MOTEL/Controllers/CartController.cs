using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Controllers
{
    public class CartController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }
        public ActionResult CartP(int? page)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN tk = GetAcc();
                int iSize = 4;
                int iPageNum = (page ?? 1);
                var phong = (from a in data.PHONGTROs
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                             join g in data.DONHANGs on a.Id equals g.Id_Phong
                             join e in data.TAIKHOANs on g.Id_NguoiDung equals e.Id
                             join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                             where (tk.Id == g.Id_NguoiDung) && (g.TrangThai == 2 || g.TrangThai == 3)
                             select new RoomDetail()
                             {
                                 sMa = g.IdDH,
                                 sTenPhong = a.TenPhong,
                                 sHoTen = e.HoTen,
                                 sDienTich = (int)a.DienTich,
                                 sSoluong = (int)a.SoLuong,
                                 sSoNguoiO = (int)a.SoNguoiO,
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
                                 sTenKV = kv.Ten,
                                 sTrangThai = (byte)g.TrangThai,
                                 sNgayDat = (DateTime)g.NgayDat,
                             });
                return View(phong.ToList().OrderByDescending(n => n.sNgayDat).ToPagedList(iPageNum, iSize));
            }
        }

        [HttpPost]
        public JsonResult Delete(int maDH)
        {
            try
            {
                // Find the order by IdDH
                var dh = data.DONHANGs.SingleOrDefault(d => d.IdDH == maDH);
                if (dh == null)
                {
                    return Json(new { code = 404, msg = "Đơn hàng không tồn tại." }, JsonRequestBehavior.AllowGet);
                }

                // Delete the order
                data.DONHANGs.DeleteOnSubmit(dh);
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Xóa phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa phòng thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Checkout(int maDH)
        {
            try
            {
                // Find the order by IdDH
                var dh = data.DONHANGs.SingleOrDefault(d => d.IdDH == maDH);
                var p = data.PHONGTROs.SingleOrDefault(n => n.Id == dh.Id_Phong);
                if (dh == null)
                {
                    return Json(new { code = 404, msg = "Đơn hàng không tồn tại." }, JsonRequestBehavior.AllowGet);
                }
                p.SoLuong += 1;
                // Delete the order
                data.DONHANGs.DeleteOnSubmit(dh);
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Trả phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Trả phòng thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}