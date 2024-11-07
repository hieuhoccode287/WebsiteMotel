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
                                    dGiaCoc = (double)a.GiaCoc,
                                    sSDT = e.SDT,
                                    sEmail = e.Email,
                                    sUrl_Path = b.Url_Path,
                                    sUrl_Path2 = b.Url_Path2,
                                    sUrl_Path3 = b.Url_Path3,
                                    sUrl_Path4 = b.Url_Path4,
                                    sMap = a.Map,
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
            var pagedList = phongTroList
                .OrderByDescending(n => n.dNgayCapNhat == null) // Ưu tiên những phòng có dNgayCapNhat là null
                .ThenBy(n => n.sTrangThai) // Ưu tiên sắp xếp theo TrangThai (giá trị lớn hơn trước)
                .ThenByDescending(n => n.dNgayCapNhat) // Sau đó sắp xếp giảm dần theo dNgayCapNhat (nếu không null)
                .ToPagedList(pageNumber, pageSize);

            ViewBag.KhuVucList = khuVucList; // Pass the area list to the view
            return View(pagedList);
        }


        private void UpdateExpiredPhongTroStatus()
        {
            // Lấy danh sách các phòng trọ đã hết hạn
            var expiredPhongTroList = data.PHONGTROs
                                    .Where(p => p.TrangThai == 1)
                                    .ToList()
                                    .Where(p => {
                                        DateTime ngay;
                                        return DateTime.TryParse(p.Ngay.ToString(), out ngay) &&
                                               ngay.AddMonths(3) < DateTime.Today;
                                    })
                                    .ToList();

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
                                  dGiaCoc = (double)a.GiaCoc,
                                  sSDT = e.SDT,
                                  sEmail = e.Email,
                                  sUrl_Path = b.Url_Path,
                                  sUrl_Path2 = b.Url_Path2,
                                  sUrl_Path3 = b.Url_Path3,
                                  sUrl_Path4 = b.Url_Path4,
                                  sMap = a.Map,
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

                // Execute the query and get the result
                var roomDetails = getroom.FirstOrDefault(); // Use FirstOrDefault to get a single result

                if (roomDetails != null) // Check if roomDetails is not null
                {
                    ViewBag.ID = roomDetails.sMa; // Now you can access sMa from the single result
                }
                else
                {
                    ViewBag.ID = null; // Handle the case when no room is found
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
        [HttpPost]
        public JsonResult UpdateRoom()
        {
            try
            {
                // Xác thực phiên
                TAIKHOAN tk = GetAcc();
                if (tk == null)
                {
                    return Json(new { code = 401, msg = "Unauthorized access." }, JsonRequestBehavior.AllowGet);
                }

                // Lấy dữ liệu từ request
                int sMa = int.Parse(Request.Form["sMa"]);
                string sTenPhong = Request.Form["sTenPhong"];
                decimal dGiaCa = decimal.Parse(Request.Form["dGiaCa"]);
                decimal dGiaCoc = decimal.Parse(Request.Form["dGiaCoc"]);
                int sIdKV = int.Parse(Request.Form["sIdKV"]);
                int sDienTich = int.Parse(Request.Form["sDienTich"]);
                int sSoluong = int.Parse(Request.Form["sSoluong"]);
                int sSoNguoiO = int.Parse(Request.Form["sSoNguoiO"]);
                string dNgayCapNhat = Request.Form["dNgayCapNhat"];
                int sDien = int.Parse(Request.Form["sDien"]);
                int sNuoc = int.Parse(Request.Form["sNuoc"]);
                int sGuiXe = int.Parse(Request.Form["sGuiXe"]);
                int sInternet = int.Parse(Request.Form["sInternet"]);
                string sMoTa = Request.Form["sMoTa"];
                string sDiaChi = Request.Form["sDiaChi"];
                string sMap = Request.Form["sMap"];

                // Tìm phòng cần cập nhật trong cơ sở dữ liệu
                var roomToUpdate = data.PHONGTROs.SingleOrDefault(r => r.Id == sMa);
                var imgdetail = data.IMAGEs.SingleOrDefault(r => r.Id_PhongTro == roomToUpdate.Id);
                if (roomToUpdate == null)
                {
                    return Json(new { code = 404, msg = "Room not found." }, JsonRequestBehavior.AllowGet);
                }

                // Cập nhật các thuộc tính phòng
                roomToUpdate.TenPhong = sTenPhong;
                roomToUpdate.GiaCa = dGiaCa;
                roomToUpdate.GiaCoc = dGiaCoc;
                roomToUpdate.KhuVuc = sIdKV;
                roomToUpdate.DienTich = sDienTich;
                roomToUpdate.SoLuong = sSoluong;
                roomToUpdate.SoNguoiO = sSoNguoiO;
                roomToUpdate.MoTa = sMoTa;
                roomToUpdate.Diachi = sDiaChi;
                roomToUpdate.Dien = sDien;
                roomToUpdate.Nuoc = sNuoc;
                roomToUpdate.GuiXe = sGuiXe;
                roomToUpdate.Internet = sInternet;
                roomToUpdate.Ngay = null;
                roomToUpdate.TrangThai = 0;
                if (!string.IsNullOrEmpty(sMap))
                {
                    roomToUpdate.Map = sMap;
                }

                var roomImage = Request.Files["roomImage"];
                string roomImageFileName = null;

                if (roomImage != null && roomImage.ContentLength > 0)
                {
                    // Đường dẫn lưu ảnh bìa
                    var imagePath = Path.Combine(Server.MapPath("~/Images"), roomImage.FileName);
                    roomImage.SaveAs(imagePath);
                    roomToUpdate.AnhBia = roomImage.FileName;
                    roomImageFileName = roomImage.FileName;
                }

                // Biến để đếm các ảnh bổ sung, bắt đầu từ 1 cho các Url_Path1, Url_Path2,...
                int additionalImageIndex = 1;

                // Xử lý các ảnh bổ sung
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var additionalImage = Request.Files[i]; // Lấy tệp theo chỉ số

                    // Kiểm tra nếu ảnh hiện tại không phải là roomImage (nếu roomImage không null)
                    if (additionalImage != null && additionalImage.ContentLength > 0
                        && (roomImageFileName == null || additionalImage.FileName != roomImageFileName))
                    {
                        // Đường dẫn lưu ảnh bổ sung
                        var additionalImagePath = Path.Combine(Server.MapPath("~/Images"), additionalImage.FileName);
                        additionalImage.SaveAs(additionalImagePath);

                        // Cập nhật tên ảnh vào cơ sở dữ liệu dựa trên additionalImageIndex
                        switch (additionalImageIndex)
                        {
                            case 1:
                                imgdetail.Url_Path = additionalImage.FileName;
                                break;
                            case 2:
                                imgdetail.Url_Path2 = additionalImage.FileName;
                                break;
                            case 3:
                                imgdetail.Url_Path3 = additionalImage.FileName;
                                break;
                            case 4:
                                imgdetail.Url_Path4 = additionalImage.FileName;
                                break;
                        }
                        // Tăng chỉ mục cho ảnh bổ sung
                        additionalImageIndex++;
                    }
                }


                // Lưu thay đổi vào cơ sở dữ liệu
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Room updated successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Ghi lại ngoại lệ
                Console.WriteLine(ex.Message);
                return Json(new { code = 500, msg = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        public ActionResult DangLai(int id)
        {
            // Thực hiện cập nhật trạng thái của phòng trọ có id tương ứng
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);

            if (phongTro != null)
            {
                phongTro.TrangThai = 0;
                phongTro.Ngay = null;
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
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            // Lấy thông tin phòng trọ cần xóa
            var phongTro = data.PHONGTROs.SingleOrDefault(pt => pt.Id == id);
            var img = data.IMAGEs.SingleOrDefault(n => n.Id_PhongTro == phongTro.Id);
            if (phongTro == null)
            {
                TempData["Message"] = "Không tìm thấy phòng trọ có mã số tương ứng!";
                return RedirectToAction("Manage");
            }

            data.IMAGEs.DeleteOnSubmit(img);
            data.PHONGTROs.DeleteOnSubmit(phongTro);
            data.SubmitChanges();

            TempData["Message"] = "Xóa phòng trọ thành công!";
            return RedirectToAction("Manage");
        }

    }
        
}