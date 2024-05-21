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

    public class ThongKeController : Controller
    {
        // GET: Admin/ThongKe
        TimNhaTroDataContext data = new TimNhaTroDataContext();

        public ActionResult Index()
        {
            return View(data.PHONGTROs);
        }
        public ActionResult ExportToExcel()
        {
            var model = data.PHONGTROs; // Lấy dữ liệu từ model của bạn

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách thống kê");

                // Tạo header
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Thống kê";
                worksheet.Cells[1, 3].Value = "Số lượng";

                worksheet.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                worksheet.Cells["B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                worksheet.Cells["C1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                // Đổ dữ liệu vào sheet
                var row = 2;
                var stt = 1;

                if (model != null)
                {
                    worksheet.Cells[row, 1].Value = stt++;
                    worksheet.Cells[row, 2].Value = "Phòng đang đăng tin";
                    worksheet.Cells[row++, 3].Value = model.Count(n => n.TrangThai == 1);

                    worksheet.Cells[row, 1].Value = stt++;
                    worksheet.Cells[row, 2].Value = "Phòng đã cho thuê";
                    worksheet.Cells[row++, 3].Value = model.Count(n => n.TrangThai == 3);

                    worksheet.Cells[row, 1].Value = stt++;
                    worksheet.Cells[row, 2].Value = "Phòng đang chờ duyệt";
                    worksheet.Cells[row++, 3].Value = model.Count(n => n.TrangThai == 2);
                }
                else
                {
                    worksheet.Cells[row, 1].Value = "Không có dữ liệu";
                    worksheet.Cells[row, 2].Style.Font.Italic = true;
                    worksheet.Cells[row++, 2, row, 3].Merge = true;
                }

                // Thiết lập định dạng cho sheet
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                // Gửi file Excel về client
                var fileStream = new MemoryStream(package.GetAsByteArray());
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "Danh sách thống kê.xlsx";

                return File(fileStream, contentType, fileName);
            }
        }
    }
}