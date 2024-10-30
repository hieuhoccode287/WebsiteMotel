using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;
using PagedList;
using System.Net.NetworkInformation;

namespace WEBSITE_MOTEL.Controllers
{
    public class ApproveController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        public ActionResult Approve(int? page)
        {
            // Cập nhật trạng thái của các phòng trọ đã hết hạn
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            if (tk == null)
            {
                // Handle session not found (e.g., redirect to login)
                return RedirectToAction("DangNhap", "User");
            }

            var chutro = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);
            if (chutro == null)
            {
                // Handle the case where the landlord is not found
                return HttpNotFound();
            }

            // Sử dụng PagedList để phân trang
            int pageSize = 5; // Số mục trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại

            // Lấy danh sách phòng trọ
            var phongTroList = from a in data.PHONGTROs
                               join c in data.DONHANGs on a.Id equals c.Id_Phong
                               join e in data.TAIKHOANs on c.Id_NguoiDung equals e.Id
                               join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                               where (c.TrangThai == 1 || c.TrangThai == 2 || c.TrangThai == 3) && a.Id_ChuTro == chutro.Id
                               select new PhongDuyet()
                               {
                                   sMa = a.Id,
                                   sIdDonHang = c.IdDH,
                                   sTenPhong = a.TenPhong,
                                   sTenND = e.HoTen,
                                   sSDTND = e.SDT,
                                   sDienTich = (int)a.DienTich,
                                   sSoLuong = (int)a.SoLuong,
                                   sSoNguoiO = (int)a.SoNguoiO,
                                   sAnhBia = a.AnhBia,
                                   sMoTa = a.MoTa,
                                   dNgayCapNhat = (DateTime)a.Ngay,
                                   dGiaCa = (double)a.GiaCa,
                                   sSDT = e.SDT,
                                   sEmail = e.Email,
                                   sDiaChi = a.Diachi,
                                   sDien = (double)a.Dien,
                                   sNuoc = (double)a.Nuoc,
                                   sGuiXe = (double)a.GuiXe,
                                   sInternet = (double)a.Internet,
                                   sTrangThai = (byte)c.TrangThai,
                                   sTenKV = kv.Ten,
                               };

            var pagedList = phongTroList.OrderByDescending(n => n.sIdDonHang).ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }
        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }

        [HttpGet]
        public JsonResult GetRoom(int id)
        {
            try
            {
                TAIKHOAN tk = GetAcc();
                var getroom = from a in data.PHONGTROs
                              join c in data.DONHANGs on a.Id equals c.Id_Phong
                              join e in data.TAIKHOANs on c.Id_NguoiDung equals e.Id
                              join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                              where (c.TrangThai == 2 || c.TrangThai == 3) && c.IdDH == id
                              select new PhongDuyet()
                              {
                                  sMa = a.Id,
                                  sIdDonHang = c.IdDH,
                                  sTenPhong = a.TenPhong,
                                  sTenND = e.HoTen,
                                  sSDTND = e.SDT,
                                  sDienTich = (int)a.DienTich,
                                  sSoLuong = (int)a.SoLuong,
                                  sSoNguoiO = (int)a.SoNguoiO,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dNgayDat = (DateTime)c.NgayDat,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sTrangThai = (byte)c.TrangThai,
                                  sTenKV = kv.Ten,
                              };

                // Check if the room data exists
                if (getroom == null)
                {
                    return Json(new { code = 404, msg = "Room not found." }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { code = 200, dh = getroom, msg = "Lấy thông tin phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                return Json(new { code = 500, msg = "Lỗi: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DuyetPhong(int id)
        {
            var dh = data.DONHANGs.SingleOrDefault(n => n.IdDH == id);
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == dh.Id_Phong);
            if (dh != null && phongTro != null && dh.TrangThai == 2)
            {
                dh.TrangThai = 1;
                phongTro.SoLuong -= 1;
                data.SubmitChanges();
                TempData["Message"] = "Duyệt phòng thành công!";
            }

            // Chuyển hướng về trang quản lý
            return RedirectToAction("Approve");
        }

        [HttpPost]
        public ActionResult DuyetPhongAjax(int id)
        {
            var dh = data.DONHANGs.SingleOrDefault(n => n.IdDH == id);
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == dh.Id_Phong);
            if (dh != null && phongTro != null && dh.TrangThai == 2)
            {
                dh.TrangThai = 3;
                phongTro.SoLuong -= 1;
                data.SubmitChanges();
                return Json(new { success = true, message = "Duyệt phòng thành công!" });
            }
            return Json(new { success = false, message = "Lỗi không duyệt được phòng!" });
        }
        public ActionResult BoDuyetPhong(int id)
        {
            var dh = data.DONHANGs.SingleOrDefault(n => n.IdDH == id);
            if (dh != null && dh.TrangThai == 2)
            {
                // Delete the order
                dh.TrangThai = 1;
                data.SubmitChanges();
                TempData["Message"] = "Bỏ duyệt phòng thành công!";
            }

            // Chuyển hướng về trang quản lý
            return RedirectToAction("Approve");
        }
        [HttpPost]
        public ActionResult BoDuyetPhongAjax(int id)
        {
            var dh = data.DONHANGs.SingleOrDefault(n => n.IdDH == id);
            if (dh == null)
            {
                return Json(new { code = 404, msg = "Đơn hàng không tồn tại." }, JsonRequestBehavior.AllowGet);
            }
            if (dh != null && dh.TrangThai == 2)
            {
                // Delete the order
                dh.TrangThai = 1;
                data.SubmitChanges();
                return Json(new { success = true, message = "Bỏ duyệt phòng thành công!" });
            }
            return Json(new { success = false, message = "Bỏ duyệt phòng không thành công!" });
        }
    }
}
