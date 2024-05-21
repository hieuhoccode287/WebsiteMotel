using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;
using System.Web.UI;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Areas.Admin.Controllers
{
    public class HoTroController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Admin/HoTro
        [HttpGet]
        public ActionResult Index(int? page, string strSearch)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            ViewData["strSearch"] = strSearch;
            
            int iSize = 5;
            int iPageNum = (page ?? 1);
            return View(data.HOTROs.OrderBy(n=>n.Id).ToPagedList(iPageNum, iSize));
        }
        public ActionResult XoaHoTro(int id)
        {
            try
            {
                var hotro = data.HOTROs.SingleOrDefault(p => p.Id == id);
                if (hotro != null)
                {
                    data.HOTROs.DeleteOnSubmit(hotro);
                    data.SubmitChanges();
                    TempData["ThongBao"] = "Xóa thành công";
                }
                else
                {
                    TempData["ThongBao"] = "Không tìm thấy thông tin hỗ trợ";
                }
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Xóa thất bại. Lỗi: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
        public ActionResult ExportExcel()
        {
            // Lấy dữ liệu cần xuất ra Excel từ database
            TimNhaTroDataContext db = new TimNhaTroDataContext();
            var hotros = db.HOTROs.ToList();

            // Tạo một file Excel mới
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Danh sách hỗ trợ");
            worksheet.Cells["B1"].Value = "DANH SÁCH CÁC VẤN ĐỀ CẦN ĐƯỢC HỖ TRỢ ";
            // Định dạng font chữ
            worksheet.Cells["A2:C2"].Style.Font.Bold = true;

            // Định dạng ô đầu tiên thành màu xám
            worksheet.Cells["A2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["A2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
            worksheet.Cells["B2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["B2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
            worksheet.Cells["C2"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells["C2"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

            // Căn giữa dòng tiêu đề
            worksheet.Cells["A2:C2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Thiết lập định dạng tiêu đề cột
            worksheet.Column(1).Width = 20;
            worksheet.Column(2).Width = 50;
            worksheet.Column(3).Width = 20;
            worksheet.Cells["A2"].Value = "Họ tên";
            worksheet.Cells["B2"].Value = "Mô tả";
            worksheet.Cells["C2"].Value = "Số điện thoại";

            // Thêm dữ liệu vào các ô
            var row = 3;
            foreach (var hotro in hotros)
            {
                worksheet.Cells[row, 1].Value = hotro.Ten;
                worksheet.Cells[row, 2].Value = hotro.MotaVande;
                worksheet.Cells[row, 3].Value = hotro.Sdt;
                row++;
            }

            // Lưu file Excel vào bộ nhớ đệm
            var stream = new MemoryStream();
            package.SaveAs(stream);

            // Trả về file Excel dưới dạng download
            stream.Position = 0;
            var fileName = "DanhSachHoTro.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}