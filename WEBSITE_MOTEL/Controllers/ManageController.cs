﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;
using System.IO;
using System.Data.Linq.SqlClient;

namespace WEBSITE_MOTEL.Controllers
{
    public class ManageController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        //*/ GET: Manage

        public ActionResult Manage()
        {
            // Cập nhật trạng thái của các phòng trọ đã hết hạn
            UpdateExpiredPhongTroStatus();
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            var chutro = data.CHUTROs.SingleOrDefault(n => n.Id_TaiKhoan == tk.Id);
            return View(data.PHONGTROs.Where(n => n.Id_ChuTro == chutro.Id && n.TrangThai != 4).ToList());
        }

        private void UpdateExpiredPhongTroStatus()
        {
            // Lấy danh sách các phòng trọ đã hết hạn
            var expiredPhongTroList = data.PHONGTROs.Where(p => p.Ngay.GetValueOrDefault().AddMonths(3) < DateTime.Today && p.TrangThai == 1).ToList();

            // Cập nhật trạng thái của các phòng trọ đã hết hạn
            foreach (var phongTro in expiredPhongTroList)
            {
                phongTro.TrangThai = 0;
            }

            // Lưu thay đổi vào database
            data.SubmitChanges();
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
                phongTro.TrangThai = 0;
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
                phongTro.TrangThai = 0;
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

        public ActionResult DuyetPhong(int id)
        {
            // Thực hiện cập nhật trạng thái của phòng trọ có id tương ứng
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null && phongTro.TrangThai == 2)
            {
                phongTro.TrangThai = 3;
                phongTro.Ngay = DateTime.Now;
                data.SubmitChanges();
                TempData["Message"] = "Duyệt phòng thành công!";
            }

            // Chuyển hướng về trang quản lý
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult DuyetPhongAjax(int id)
        {
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null && phongTro.TrangThai == 2)
            {
                phongTro.TrangThai = 3;
                data.SubmitChanges();
                return Json(new { success = true, message = "Duyệt phòng thành công!" });
            }
            return Json(new { success = false, message = "Lỗi không duyệt được phòng!" });
        }
        public ActionResult BoDuyetPhong(int id)
        {
            // Thực hiện cập nhật trạng thái của phòng trọ có id tương ứng
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null && phongTro.TrangThai == 2)
            {
                phongTro.TrangThai = 1;
                phongTro.Ngay = DateTime.Now;
                data.SubmitChanges();
                TempData["Message"] = "Bỏ duyệt phòng thành công!";
            }

            // Chuyển hướng về trang quản lý
            return RedirectToAction("Manage");
        }
        [HttpPost]
        public ActionResult BoDuyetPhongAjax(int id)
        {
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            if (phongTro != null && phongTro.TrangThai == 2)
            {
                phongTro.TrangThai = 1;
                data.SubmitChanges();
                return Json(new { success = true, message = "Bỏ duyệt phòng thành công!" });
            }
            return Json(new { success = false, message = "Bỏ duyệt phòng không thành công!" });
        }
    }
}