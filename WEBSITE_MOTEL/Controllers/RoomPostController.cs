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

            else
            {

                return View();
            }

        }
    
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(PHONGTRO phong, FormCollection f, HttpPostedFileBase[] fFileUpLoad, IMAGE images, CHUTRO chutro, TAIKHOAN taikhoan)
        {

            if (fFileUpLoad == null)
            {

                ViewBag.ThongBao = "Hay chon anh bia";

                ViewBag.TenPhong = f["sTieuDe"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.DienTich = f["sDienTich"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaCa = decimal.Parse(f["mGiaCa"]);

                return View();

            }
            else
            {
                if (ModelState.IsValid)
                {
                    int dem = 0;
                    foreach (HttpPostedFileBase file in fFileUpLoad)
                    {
                        if (file != null || file.ContentLength > 0)
                        {
                            var sFileName = Path.GetFileName(file.FileName);
                            dem++;
                            if (dem == 1)
                            {
                                phong.AnhBia = sFileName;
                            }
                            else if (dem == 2)
                            {
                                images.Url_Path = sFileName;
                            }
                            else if (dem == 3)
                            {
                                images.Url_Path2 = sFileName;
                            }
                            else if (dem == 4)
                            {
                                images.Url_Path3 = sFileName;
                            }
                            else if (dem == 5)
                            {
                                images.Url_Path4 = sFileName;
                            }
                            var path = Path.Combine(Server.MapPath("~/Images"), sFileName);
                            if (!System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                                file.SaveAs(path);
                            }
                            else
                            {
                                file.SaveAs(path);
                            }
                        }
                    }

                    phong.Id_ChuTro = chutro.Id;
                    phong.TenPhong = f["sTieuDe"];
                    phong.MoTa = HttpUtility.HtmlDecode(f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n"));


                    images.Id_PhongTro = phong.Id;

                    phong.DienTich = int.Parse(f["sDienTich"]);
                    phong.Ngay = Convert.ToDateTime(f["dNgayCapNhat"]);
                    /* phong.SoLuong = int.Parse(f["iSoLuong"]);*/
                    phong.GiaCa = decimal.Parse(f["mGiaCa"]);
                    phong.Dien = decimal.Parse(f["sDien"]);
                    phong.Nuoc = decimal.Parse(f["sNuoc"]);
                    phong.Internet = decimal.Parse(f["sInternet"]);
                    phong.GuiXe = decimal.Parse(f["sGuiXe"]);
                    phong.Diachi = f["sDiaChi"];
                    phong.SoLuong = int.Parse(f["sSoLuong"]);
                    phong.Doituong = byte.Parse(f["sDoiTuong"]);
                    phong.TrangThai = byte.Parse(f["sTrangThai"]);
                    phong.Map = f["sMap"];
                    /*   chutro.HoTen = f["sHoTen"];*/
                    taikhoan.SDT = f["sSDT"];
                    phong.KhuVuc = f["sKhuVuc"];
                    phong.Id_ChuTro = int.Parse(f["sid"]);
                    data.PHONGTROs.InsertOnSubmit(phong);
                    data.SubmitChanges();
                    images.Id_PhongTro = phong.Id;
                    data.IMAGEs.InsertOnSubmit(images);
                    data.SubmitChanges();
                    return RedirectToAction("Index", "Motel");
                }
                return View();
            }
        }

    }
}