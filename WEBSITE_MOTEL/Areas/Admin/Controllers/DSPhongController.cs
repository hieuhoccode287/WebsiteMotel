using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using WEBSITE_MOTEL.Models;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;

namespace WEBSITE_MOTEL.Areas.Admin.Controllers
{
    public class DSPhongController : Controller
    {
        // GET: Admin/DSPhong
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        public ActionResult Index(int? page, string strSearch)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            ViewData["strSearch"] = strSearch;
            ViewBag.IdPH = new SelectList(data.PHONGTROs.ToList().OrderBy(n => n.TenPhong), "Id", "TenPhong");
            int iSize = 4;
            int iPageNum = (page ?? 1);
            if (!string.IsNullOrEmpty(strSearch))
            {
                var phong = (from a in data.PHONGTROs
                             join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                             join d in data.TAIKHOANs on b.Id_TaiKhoan equals d.Id

                             where a.TenPhong.Contains(strSearch) || a.MoTa.Contains(strSearch) || a.DienTich.Equals(strSearch)
                             select new RoomDetail()
                             {
                                 sMa = (int)a.Id,
                                 sTenPhong = a.TenPhong,
                                 /*sHoTen = b.HoTen,*/
                                 sTenKV = a.KhuVuc,
                                 sDienTich = (int)a.DienTich,
                                 sSoluong = (int)a.SoLuong,
                                 sAnhBia = a.AnhBia,
                                 sMoTa = a.MoTa,
                                 sDiaChi = a.Diachi,
                                 sTrangThai = (byte)a.TrangThai,
                                 dNgayCapNhat = (DateTime)a.Ngay,
                                 dGiaCa = (double)a.GiaCa,
                                 sSDT = d.SDT,
                                 sEmail = d.Email,
                                 sUrl_Path = c.Url_Path,
                                 sUrl_Path2 = c.Url_Path2,
                                 sUrl_Path3 = c.Url_Path3,
                                 sUrl_Path4 = c.Url_Path4,
                                 sIDCT = (int)a.Id_ChuTro,
                             });
                return View(phong.ToList().Where(n => n.sTrangThai == 1).OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
            }
            else
            {
                var phong = (from a in data.PHONGTROs
                             join b in data.CHUTROs on a.Id_ChuTro equals b.Id
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                             join d in data.TAIKHOANs on b.Id_TaiKhoan equals d.Id

                             select new RoomDetail()
                             {
                                 sMa = (int)a.Id,
                                 sTenPhong = a.TenPhong,
                                 /*sHoTen = b.HoTen,*/
                                 sTenKV = a.KhuVuc,
                                 sDienTich = (int)a.DienTich,
                                 sSoluong = (int)a.SoLuong,
                                 sAnhBia = a.AnhBia,
                                 sMoTa = a.MoTa,
                                 sDiaChi = a.Diachi,
                                 dNgayCapNhat = (DateTime)a.Ngay,
                                 dGiaCa = (double)a.GiaCa,
                                 sSDT = d.SDT,
                                 sTrangThai = (byte)a.TrangThai,
                                 sEmail = d.Email,
                                 sUrl_Path = c.Url_Path,
                                 sUrl_Path2 = c.Url_Path2,
                                 sUrl_Path3 = c.Url_Path3,
                                 sUrl_Path4 = c.Url_Path4,
                                 sIDCT = (int)a.Id_ChuTro,
                             });

                return View(phong.ToList().Where(n => n.sTrangThai == 1).OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
            }
        }
        public ActionResult ExportToExcel()
        {
            TimNhaTroDataContext data = new TimNhaTroDataContext();
            var phong = data.PHONGTROs.Where(n => n.TrangThai == 1);

            // Tạo file Excel
            // Tạo một file Excel mới
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Danh Sách Phòng Đang Đăng Tin");

            // Thêm dòng tiêu đề
            worksheet.Cells[1, 1].Value = "Thống kê Danh Sách các phòng đang được đăng tin";
            using (ExcelRange range = worksheet.Cells[1, 1, 1, 7])
            {
                range.Merge = true;
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 176, 240));
                range.Style.Font.Color.SetColor(Color.White);
            }

            // Tạo header cho bảng
            worksheet.Column(1).Width = 40;
            worksheet.Column(2).Width = 80;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 10;
            worksheet.Column(5).Width = 60;
            worksheet.Column(6).Width = 10;
            worksheet.Column(7).Width = 25;
            worksheet.Cells[2, 1].Value = "Tên phòng";
            worksheet.Cells[2, 2].Value = "Mô tả";
            worksheet.Cells[2, 3].Value = "Giá cả";
            worksheet.Cells[2, 4].Value = "Diện tích";
            worksheet.Cells[2, 5].Value = "Địa chỉ";
            worksheet.Cells[2, 6].Value = "Số lượng";
            worksheet.Cells[2, 7].Value = "Ngày đăng";

            // Định dạng header
            using (ExcelRange range = worksheet.Cells[2, 1, 2, 7])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                range.Style.Font.Color.SetColor(Color.White);
            }

            int row = 3;
            foreach (var item in phong)
            {
                worksheet.Cells[row, 1].Value = item.TenPhong;
                worksheet.Cells[row, 2].Value = item.MoTa;
                worksheet.Cells[row, 3].Value = item.GiaCa;
                worksheet.Cells[row, 3].Style.Numberformat.Format = "#,##0 đ";
                worksheet.Cells[row, 4].Value = item.DienTich + " m2";
                worksheet.Cells[row, 5].Value = item.Diachi;
                worksheet.Cells[row, 6].Value = item.SoLuong;
                worksheet.Cells[row, 7].Value = item.Ngay.ToString();

                row++;
            }

            // Lưu file Excel vào bộ nhớ đệm
            var stream = new MemoryStream();
            package.SaveAs(stream);

            // Trả về file Excel dưới dạng download
            stream.Position = 0;
            var fileName = "DanhSachPhong.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}