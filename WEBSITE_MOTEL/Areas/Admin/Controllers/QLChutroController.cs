using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Areas.Admin.Controllers
{
    public class QLChutroController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Admin/QLChutro
        public ActionResult Index(int? page, string strSearch)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }

            ViewData["strSearch"] = strSearch;
            ViewBag.IdCT = new SelectList(data.TAIKHOANs.ToList().OrderBy(n => n.HoTen), "Id", "HoTen");

            int pageSize = 8;
            int pageNumber = page ?? 1;

            // Truy vấn danh sách chủ trọ
            var chutroQuery = from b in data.TAIKHOANs
                              where b.TrangThai == 1
                              select new ChuTro()
                              {
                                  sHotenCT = b.HoTen,
                                  sTaiKhoanCT = b.TaiKhoan,
                                  sMatKhauCT = b.MatKhau,
                                  sNgaySinh = b.NgaySinh,
                                  sEmailCT = b.Email,
                                  sDiaChi = b.DiaChi,
                                  sSDTCT = b.SDT,
                                  sPhanQuyen = (int)b.PhanQuyen,
                                  sCCCD = b.CCCD,
                                  sId = b.Id,
                              };

            // Materialize chutroQuery in memory
            var chutroList = chutroQuery.ToList();

            // Tính số phòng chờ duyệt cho mỗi chủ trọ
            Dictionary<int, int> pendingRoomsCounts = new Dictionary<int, int>();

            foreach (var chuTro in chutroList)
            {
                var roomsForChuTro = data.PHONGTROs.Where(p => p.Id_ChuTro == chuTro.sId);
                var pendingRoomsCount = roomsForChuTro.Count(p => p.TrangThai == 0); // Số phòng chờ duyệt
                pendingRoomsCounts[chuTro.sId] = pendingRoomsCount; // Store count by ChuTro Id
            }

            ViewBag.PendingRoomsCounts = pendingRoomsCounts;

            // Giả sử bạn có bảng ChuTro với cột TrangThai
            var pendingAccounts = data.TAIKHOANs.Where(ct => ct.TrangThai == 0).ToList();

            // Tính số tài khoản chủ trọ có trạng thái chờ duyệt
            int pendingAccountCount = pendingAccounts.Count();

            ViewBag.PendingAccountCount = pendingAccountCount; // Lưu số tài khoản chờ duyệt vào ViewBag

            // Sort the list by pending rooms count in descending order, then by Id if counts are equal
            var chutroPaged = chutroList
                .OrderByDescending(ct => pendingRoomsCounts.ContainsKey(ct.sId) ? pendingRoomsCounts[ct.sId] : 0)
                .ThenByDescending(ct => ct.sId)
                .ToList();

            var pagedChutro = new PagedList<ChuTro>(chutroPaged, pageNumber, pageSize);

            return View(pagedChutro);
        }



        public ActionResult Delete(int id)
        {
            try
            {
                // Lấy tài khoản chủ trọ theo id
                var chuTro = data.TAIKHOANs.SingleOrDefault(ct => ct.Id == id);
                if (chuTro != null)
                {
                    // Lấy tất cả các phòng trọ liên kết với tài khoản này
                    var phongTros = data.PHONGTROs.Where(pt => pt.Id_ChuTro == chuTro.Id).ToList();

                    // Xóa tất cả các phòng trọ liên kết
                    foreach (var phong in phongTros)
                    {
                        var images = data.IMAGEs.Where(i => i.Id_PhongTro == phong.Id).ToList();
                        data.IMAGEs.DeleteAllOnSubmit(images); // Xóa tất cả ảnh liên quan đến phòng trọ
                        data.PHONGTROs.DeleteOnSubmit(phong); // Xóa phòng trọ
                    }
                    data.TAIKHOANs.DeleteOnSubmit(chuTro);
                    data.SubmitChanges();

                    TempData["ThongBao"] = "Xóa thành công";
                }
                else
                {
                    TempData["ThongBao"] = "Không tìm thấy thông tin tài khoản";
                }
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Xóa thất bại. Lỗi: " + ex.Message;
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            var chutro = (from b in data.TAIKHOANs
                          where b.Id == id
                          select new ChuTro()
                          {
                              sHotenCT = b.HoTen,
                              sTaiKhoanCT = b.TaiKhoan,
                              sMatKhauCT = b.MatKhau,
                              sNgaySinh = b.NgaySinh,
                              sEmailCT = b.Email,
                              sDiaChi = b.DiaChi,
                              sSDTCT = b.SDT,
                              sPhanQuyen = (int)b.PhanQuyen,
                              sCCCD = b.CCCD,
                              sId = b.Id,
                          }).FirstOrDefault();
            if (chutro == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chutro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChuTro model)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }

            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors if the model state is invalid
            }

            // Find the TAIKHOAN entity based on the user ID from the model
            var taikhoan = data.TAIKHOANs.FirstOrDefault(t => t.Id == model.sId); // Replace 'Id' with the actual primary key field of your TAIKHOAN model

            if (taikhoan != null)
            {
                // Update the fields with the new data from the model
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

                // Redirect to the Index action after successful update
                return RedirectToAction("Edit");
            }

            // If no account found, return the view with the model for user feedback
            ModelState.AddModelError("", "Không tìm thấy tài khoản để cập nhật.");
            return View(model);
        }

        public ActionResult Details(int? page,int? id)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            int iSize = 4;
            int iPageNum = (page ?? 1);


            var allRatings = (from a in data.DANHGIAs
                              join b in data.PHONGTROs on a.Id_Phong equals b.Id
                              select new DanhGia()
                              {
                                  sId = a.Id,
                                  sId_Phong = (int)a.Id_Phong,
                                  sDanhGiaRating = a.DanhGiaRating,
                                  sMoTaDanhGia = a.MoTa,
                                  dNgayDanhGia = (DateTime)a.NgayDanhGia,
                              });
            ViewBag.AllRatings = allRatings;

            var test = data.TAIKHOANs.FirstOrDefault(n => n.Id == id);

            var tenct = test?.HoTen;
            var idct = test?.Id;

            ViewBag.TenCT = tenct;
            ViewBag.ID = idct;

            var phong = (from a in data.PHONGTROs
                         join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                         join d in data.TAIKHOANs on a.Id_ChuTro equals d.Id
                         join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                         select new RoomDetail()
                         {
                             sMa = (int)a.Id,
                             sTenPhong = a.TenPhong,
                             sHoTen = d.HoTen,
                             sTrangThai = (int)a.TrangThai,
                             sDienTich = (int)a.DienTich,
                             sSoluong = (int)a.SoLuong,
                             sAnhBia = a.AnhBia,
                             sMoTa = a.MoTa,
                             dNgayCapNhat = (DateTime?)a.Ngay, // Đảm bảo kiểu nullable cho dNgayCapNhat
                             dGiaCa = (double)a.GiaCa,
                             sSDT = d.SDT,
                             sEmail = d.Email,
                             sUrl_Path = c.Url_Path,
                             sUrl_Path2 = c.Url_Path2,
                             sUrl_Path3 = c.Url_Path3,
                             sUrl_Path4 = c.Url_Path4,
                             sIDCT = (int)a.Id_ChuTro,
                             sTenKV = kv.Ten,
                         });


            // Kiểm tra nếu không có phòng nào
            if (phong == null || !phong.Any())
            {
                Response.StatusCode = 404;
                return null;
            }

            // Áp dụng các điều kiện sắp xếp:
            // 1. Ưu tiên phòng có dNgayCapNhat là null
            // 2. Sắp xếp theo TrangThai (giá trị lớn hơn trước)
            // 3. Sắp xếp giảm dần theo dNgayCapNhat nếu không null
            return View(phong
                        .Where(n => n.sIDCT == id) // Lọc theo ID chủ trọ
                        .OrderByDescending(n => n.dNgayCapNhat.HasValue ? 0 : 1) // Ưu tiên phòng có dNgayCapNhat là null (null được sắp xếp trước)
                        .ThenByDescending(n => n.sTrangThai) // Sắp xếp theo TrangThai (lớn hơn trước)
                        .ThenByDescending(n => n.dNgayCapNhat) // Sắp xếp giảm dần theo dNgayCapNhat nếu không null
                        .ToPagedList(iPageNum, iSize)); // Phân trang

        }

        public ActionResult DuyetPhong(int id)
        {
            PHONGTRO phong = data.PHONGTROs.FirstOrDefault(p => p.Id == id && p.TrangThai == 0);
            var tk = data.TAIKHOANs.SingleOrDefault(t => t.Id == phong.Id_ChuTro);
            if (phong != null)
            {
                phong.TrangThai = 1;
                phong.Ngay = DateTime.Now;
                data.SubmitChanges();
                TempData["Message"] = "Duyệt phòng thành công!";
            }

            return RedirectToAction("Details", new { id = tk.Id });
        }

        public ActionResult TuChoiPhong(int id)
        {
            // Lấy thông tin phòng trọ cần xóa
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            var img = data.IMAGEs.SingleOrDefault(n => n.Id_PhongTro == phongTro.Id);
            var tk = data.TAIKHOANs.SingleOrDefault(t => t.Id == phongTro.Id_ChuTro);
            if (phongTro == null)
            {
                TempData["Message"] = "Không tìm thấy phòng trọ có mã số tương ứng!";
                return RedirectToAction("Details", new { id = tk.Id });
            }

            data.IMAGEs.DeleteOnSubmit(img);
            data.PHONGTROs.DeleteOnSubmit(phongTro);
            data.SubmitChanges();

            TempData["Message"] = "Xóa phòng trọ thành công!";
            return RedirectToAction("Details", new { id = tk.Id });
        }
    }
}