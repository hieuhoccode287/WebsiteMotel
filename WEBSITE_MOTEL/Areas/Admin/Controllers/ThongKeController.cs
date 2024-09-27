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
            var rooms = data.PHONGTROs.ToList();
            var orders = data.DONHANGs.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách thống kê");

                // Create headers
                CreateHeader(worksheet);

                // Fill data
                FillData(worksheet, rooms, orders);

                // Formatting
                worksheet.Cells.AutoFitColumns();
                worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // Return Excel file
                return CreateExcelFile(package, "Danh sách thống kê.xlsx");
            }
        }

        private void CreateHeader(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "STT";
            worksheet.Cells[1, 2].Value = "Thống kê";
            worksheet.Cells[1, 3].Value = "Số lượng";

            for (int col = 1; col <= 3; col++)
            {
                worksheet.Cells[1, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            }
        }

        private void FillData(ExcelWorksheet worksheet, List<PHONGTRO> rooms, List<DONHANG> orders)
        {
            var row = 2;
            int stt = 1;

            if (rooms != null && orders != null)
            {
                worksheet.Cells[row, 1].Value = stt++;
                worksheet.Cells[row, 2].Value = "Phòng đang đăng tin";
                worksheet.Cells[row++, 3].Value = rooms.Count(n => n.TrangThai == 1);

                worksheet.Cells[row, 1].Value = stt++;
                worksheet.Cells[row, 2].Value = "Phòng đã cho thuê";
                worksheet.Cells[row++, 3].Value = orders.Count(n => n.TrangThai == 3);

                worksheet.Cells[row, 1].Value = stt++;
                worksheet.Cells[row, 2].Value = "Phòng đang chờ duyệt";
                worksheet.Cells[row++, 3].Value = orders.Count(n => n.TrangThai == 2);
            }
            else
            {
                worksheet.Cells[row, 1].Value = "Không có dữ liệu";
                worksheet.Cells[row, 2].Style.Font.Italic = true;
                worksheet.Cells[row++, 2, row, 3].Merge = true;
            }
        }

        private ActionResult CreateExcelFile(ExcelPackage package, string fileName)
        {
            var fileStream = new MemoryStream(package.GetAsByteArray());
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(fileStream, contentType, fileName);
        }

    }
}