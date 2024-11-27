using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Controllers
{
    public class RoomPostController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: RoomPost
        public ActionResult Index()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            // Get the list of KhuVuc
            var khuVucList = data.KHUVUCs.ToList();

            return View(khuVucList); // Pass the list to the view
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(PHONGTRO phong,FormCollection f, HttpPostedFileBase[] fFileUpLoad, IMAGE images)
        {
            // Check if there are any files to upload
            if (fFileUpLoad == null || !fFileUpLoad.Any())
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa";
                // Preserve the input values for the view
                ViewBag.TenPhong = f["sTieuDe"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.DienTich = f["sDienTich"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaCa = decimal.Parse(f["mGiaCa"]);
                ViewBag.GiaCoc = decimal.Parse(f["mGiaCoc"]);

                return View();
            }

            // Validate model state before proceeding
            if (ModelState.IsValid)
            {
                int dem = 0;
                foreach (var file in fFileUpLoad)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var sFileName = Path.GetFileName(file.FileName);
                        dem++;
                        // Assign file names to respective properties
                        switch (dem)
                        {
                            case 1: phong.AnhBia = sFileName; break;
                            case 2: images.Url_Path = sFileName; break;
                            case 3: images.Url_Path2 = sFileName; break;
                            case 4: images.Url_Path3 = sFileName; break;
                            case 5: images.Url_Path4 = sFileName; break;
                        }

                        var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                        if (!System.IO.File.Exists(path))
                        {
                            file.SaveAs(path); // Save the file
                        }
                    }
                }

                // Populate the PHONGTRO model with values from the form
                phong.TenPhong = f["sTieuDe"];
                phong.MoTa = HttpUtility.HtmlDecode(f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n"));

                if (int.TryParse(f["sDienTich"], out int dienTich)) phong.DienTich = dienTich;
                else ModelState.AddModelError("DienTich", "Invalid value for DienTich");

                /*if (DateTime.TryParse(f["dNgayCapNhat"], out DateTime ngayCapNhat)) phong.Ngay = ngayCapNhat;
                else ModelState.AddModelError("NgayCapNhat", "Invalid value for NgayCapNhat");*/

                string giaCaString = f["mGiaCa"].Replace(".", ""); // Bỏ dấu chấm
                if (decimal.TryParse(giaCaString, out decimal giaCa))
                    phong.GiaCa = giaCa;
                else
                    ModelState.AddModelError("GiaCa", "Invalid value for GiaCa");

                string giaCocString = f["mGiaCoc"].Replace(".", ""); // Bỏ dấu chấm
                if (decimal.TryParse(giaCocString, out decimal giaCoc))
                    phong.GiaCoc = giaCoc;
                else
                    ModelState.AddModelError("GiaCoc", "Invalid value for GiaCoc");


                string dienString = f["sDien"].Replace(".", ""); // Bỏ dấu chấm
                if (decimal.TryParse(dienString, out decimal dien))
                    phong.Dien = dien;
                else
                    ModelState.AddModelError("Dien", "Invalid value for Dien");


                string nuocString = f["sNuoc"].Replace(".", ""); // Bỏ dấu chấm
                if (decimal.TryParse(nuocString, out decimal nuoc))
                    phong.Nuoc = nuoc;
                else
                    ModelState.AddModelError("Nuoc", "Invalid value for Nuoc");

                string internetString = f["sInternet"].Replace(".", ""); // Bỏ dấu chấm
                if (decimal.TryParse(internetString, out decimal internet))
                    phong.Internet = internet;
                else
                    ModelState.AddModelError("Internet", "Invalid value for Internet");

                string guixeString = f["sGuiXe"].Replace(".", ""); // Bỏ dấu chấm
                if (decimal.TryParse(guixeString, out decimal guixe))
                    phong.GuiXe = guixe;
                else
                    ModelState.AddModelError("GuiXe", "Invalid value for GuiXe");


                if (int.TryParse(f["sSoPhong"], out int soPhong)) phong.SoLuong = soPhong;
                else ModelState.AddModelError("SoPhong", "Invalid value for SoPhong");

                if (int.TryParse(f["sSoLuong"], out int soNguoiO)) phong.SoNguoiO = soNguoiO;
                else ModelState.AddModelError("SoLuong", "Invalid value for SoNguoiO");

                if (byte.TryParse(f["sTrangThai"], out byte trangThai)) phong.TrangThai = trangThai;
                else ModelState.AddModelError("TrangThai", "Invalid value for TrangThai");

                phong.Map = f["sMap"];

                phong.Diachi = f["sDiaChi"];
                string selectedKhuVucId = f["sKhuVuc"];
                if (int.TryParse(selectedKhuVucId, out int khuVucId)) // Use TryParse for safe conversion
                {
                    phong.KhuVuc = khuVucId; // Successfully parsed, assign to phong.KhuVuc
                }
                else
                {
                    ModelState.AddModelError("sKhuVuc", "Vui lòng chọn Phường/Xã hợp lệ."); // Validation error
                }

                phong.Id_ChuTro = int.Parse(f["sid"]);

                if (ModelState.IsValid) // Re-check if all fields are valid before saving to the database
                {
                    // Save data to database
                    data.PHONGTROs.InsertOnSubmit(phong);
                    data.SubmitChanges();
                    images.Id_PhongTro = phong.Id;
                    data.IMAGEs.InsertOnSubmit(images);
                    data.SubmitChanges();

                    TempData["SuccessMessage"] = "Đăng tin thành công! Tin của bạn đang chờ duyệt.";
                    return RedirectToAction("Index", "RoomPost");
                }
            }

            // If model state is not valid, return the view with the model to show error messages
            return View();
        }



    }
}