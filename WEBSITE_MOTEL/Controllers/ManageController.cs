using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;
using System.IO;
using System.Data.Linq.SqlClient;
using PagedList;


namespace WEBSITE_MOTEL.Controllers
{
    public class ManageController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        //*/ GET: Manage

        public ActionResult Manage(int? page)
        {
            // Cập nhật trạng thái của các phòng trọ đã hết hạn
            UpdateExpiredPhongTroStatus();

            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            if (tk == null)
            {
                // Handle session not found (e.g., redirect to login)
                return RedirectToAction("DangNhap", "User");
            }

            var chutro = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);

            // Lấy danh sách phòng trọ
            var phongTroList = (from a in data.PHONGTROs
                                join b in data.IMAGEs on a.Id equals b.Id_PhongTro
                                join e in data.TAIKHOANs on a.Id_ChuTro equals e.Id
                                join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                                where e.Id == chutro.Id
                                select new RoomDetail()
                                {
                                    sMa = a.Id,
                                    sTenPhong = a.TenPhong,
                                    sHoTen = e.HoTen,
                                    sTrangThai = (byte)a.TrangThai,
                                    sDienTich = (int)a.DienTich,
                                    sSoluong = (int)a.SoLuong,
                                    sSoNguoiO = (int)a.SoNguoiO,
                                    sAnhBia = a.AnhBia,
                                    sMoTa = a.MoTa,
                                    dNgayCapNhat = (DateTime)a.Ngay,
                                    dGiaCa = (double)a.GiaCa,
                                    sSDT = e.SDT,
                                    sEmail = e.Email,
                                    sUrl_Path = b.Url_Path,
                                    sUrl_Path2 = b.Url_Path2,
                                    sUrl_Path3 = b.Url_Path3,
                                    sUrl_Path4 = b.Url_Path4,
                                    sDiaChi = a.Diachi,
                                    sDien = (double)a.Dien,
                                    sNuoc = (double)a.Nuoc,
                                    sGuiXe = (double)a.GuiXe,
                                    sInternet = (double)a.Internet,
                                    sTenKV = kv.Ten,
                                    sIdKV = kv.Id,
                                });

            // Lấy danh sách khu vực
            var khuVucList = data.KHUVUCs.ToList();

            // Sử dụng PagedList để phân trang
            int pageSize = 5; // Số mục trên mỗi trang
            int pageNumber = (page ?? 1); // Số trang hiện tại
            var pagedList = phongTroList.ToPagedList(pageNumber, pageSize);

            ViewBag.KhuVucList = khuVucList; // Pass the area list to the view
            return View(pagedList);
        }


        private void UpdateExpiredPhongTroStatus()
        {
            // Lấy danh sách các phòng trọ đã hết hạn
            var expiredPhongTroList = data.PHONGTROs.Where(p => p.Ngay.GetValueOrDefault().AddMonths(3) < DateTime.Today && p.TrangThai == 1).ToList();

            // Cập nhật trạng thái của các phòng trọ đã hết hạn
            foreach (var phongTro in expiredPhongTroList)
            {
                phongTro.TrangThai = 2;
            }

            // Lưu thay đổi vào database
            data.SubmitChanges();
        }

        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }

        [HttpGet]
        public JsonResult EditRoom(int id)
        {
            try
            {
                TAIKHOAN tk = GetAcc();
                var khuVucList = data.KHUVUCs.ToList(); // Lấy danh sách Khu Vực từ bảng KHUVUC
                ViewBag.KhuvucList = khuVucList;
                var getroom = from a in data.PHONGTROs
                              join b in data.IMAGEs on a.Id equals b.Id_PhongTro
                              join e in data.TAIKHOANs on a.Id_ChuTro equals e.Id
                              join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                              where a.Id == id
                              select new RoomDetail()
                              {
                                  sMa = a.Id,
                                  sTenPhong = a.TenPhong,
                                  sHoTen = e.HoTen,
                                  sTrangThai = (byte)a.TrangThai,
                                  sDienTich = (int)a.DienTich,
                                  sSoluong = (int)a.SoLuong,
                                  sSoNguoiO = (int)a.SoNguoiO,
                                  sAnhBia = a.AnhBia,
                                  sMoTa = a.MoTa,
                                  dNgayCapNhat = (DateTime)a.Ngay,
                                  dGiaCa = (double)a.GiaCa,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = b.Url_Path,
                                  sUrl_Path2 = b.Url_Path2,
                                  sUrl_Path3 = b.Url_Path3,
                                  sUrl_Path4 = b.Url_Path4,
                                  sDiaChi = a.Diachi,
                                  sDien = (double)a.Dien,
                                  sNuoc = (double)a.Nuoc,
                                  sGuiXe = (double)a.GuiXe,
                                  sInternet = (double)a.Internet,
                                  sIdKV = kv.Id,
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

        public ActionResult DangLai(int id)
        {
            // Thực hiện cập nhật trạng thái của phòng trọ có id tương ứng
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);

            if (phongTro != null)
            {
                phongTro.TrangThai = 1;
                phongTro.Ngay = DateTime.Now;
                data.SubmitChanges();
                TempData["Message"] = "Đăng lại tin thành công!";
            }

            // Chuyển hướng về trang quản lý
            return RedirectToAction("Manage");
        }

        public ActionResult NgungDang(int id)
        {
            // Thực hiện cập nhật trạng thái của phòng trọ có id tương ứng
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null)
            {
                phongTro.TrangThai = 2;
                phongTro.Ngay = null;
                data.SubmitChanges();
                TempData["Message"] = "Ngừng đăng tin thành công!";
            }

            // Chuyển hướng về trang quản lý
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult DangLaiAjax(int id)
        {
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null)
            {
                phongTro.TrangThai = 1;
                data.SubmitChanges();
                return Json(new { success = true, message = "Đăng lại tin thành công!" });
            }
            return Json(new { success = false, message = "Không tìm thấy phòng trọ có mã số tương ứng!" });
        }

        [HttpPost]
        public ActionResult NgungDangAjax(int id)
        {
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null)
            {
                phongTro.TrangThai = 2;
                data.SubmitChanges();
                return Json(new { success = true, message = "Ngừng đăng tin thành công!" });
            }
            return Json(new { success = false, message = "Không tìm thấy phòng trọ có mã số tương ứng!" });
        }

        public ActionResult Xoa(int id)
        {
            // Lấy thông tin phòng trọ cần xóa
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro == null)
            {
                TempData["Message"] = "Không tìm thấy phòng trọ có mã số tương ứng!";
                return RedirectToAction("Manage");
            }

            // Xóa phòng trọ
            data.PHONGTROs.DeleteOnSubmit(phongTro);
            data.SubmitChanges();

            TempData["Message"] = "Xóa phòng trọ thành công!";
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult XoaAjax(int id)
        {
            // Lấy thông tin phòng trọ cần xóa
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null)
            {
                try
                {
                    data.PHONGTROs.DeleteOnSubmit(phongTro);
                    data.SubmitChanges();
                    return Json(new
                    {
                        success = true,
                        message = "Xóa phòng trọ thành công!"
                    });
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Lỗi xóa phòng trọ: " + ex.Message
                    });
                }
            }
            return Json(new
            {
                success = false,
                message = "Không tìm thấy phòng trọ có mã số tương ứng!"
            });
        }
    }
        
}