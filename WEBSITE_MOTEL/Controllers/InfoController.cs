using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Controllers
{
    public class InfoController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }
        // GET: Info
        [HttpGet]
        public ActionResult ManageInfo()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN nd = (TAIKHOAN)Session["TaiKhoan"];


                var adminn = (from a in data.ADMINs
                              join b in data.TAIKHOANs on a.Id_TaiKhoan equals b.Id
                              

                              select new Admin()
                              {
                                  sTaiKhoanAD = b.TaiKhoan,
                                  sHotenAD = b.HoTen,
                                  sMatKhauAD = b.MatKhau,
                                  sId = b.Id,
                                  sSDTAD = a.SDT,
                                  sEmailAD = a.Email
                              }).FirstOrDefault();


                return View(adminn);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageInfo(FormCollection f)
        {
            var thongtin = data.TAIKHOANs.SingleOrDefault(n => n.Id == int.Parse(f["sId"]));
            if(ModelState.IsValid)
            {
                thongtin.HoTen = f["sHotenAD"];
                thongtin.MatKhau = f["sMatKhauAD"];
                data.SubmitChanges();
                return RedirectToAction("ManageInfo");
            }
            return View(thongtin);
        }
        public ActionResult ManageInfoCT()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN nd = (TAIKHOAN)Session["TaiKhoan"];


                var chutro = (from a in data.TAIKHOANs

                              join b in data.CHUTROs on a.Id equals b.Id_TaiKhoan
                              where (nd.Id== b.Id_TaiKhoan)
                              select new ChuTro()
                              {
                                  sTaiKhoanCT = a.TaiKhoan,
                                  sHotenCT = a.HoTen,
                                  sMatKhauCT = a.MatKhau,
                                  sId = b.Id,
                                  sSDTCT = a.SDT,
                                  sEmailCT = a.Email,
                                  sCCCD = a.CCCD,
                                  sDiaChi = a.DiaChi,
                                  sNgaySinh = (DateTime)a.NgaySinh
                              }).FirstOrDefault();


                return View(chutro);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageInfoCT(FormCollection f)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            var thongtin = data.CHUTROs.SingleOrDefault(n => n.Id_TaiKhoan == tk.Id);
            if (ModelState.IsValid)
            {
                
                tk.HoTen = f["sHotenCT"];
                tk.MatKhau = f["sMatKhauCT"];
                tk.SDT = f["sSDTCT"];
                tk.Email = f["sEmailCT"];
                tk.CCCD = f["sCCCD"];
                tk.DiaChi = f["sDiaChi"];
                tk.NgaySinh = Convert.ToDateTime(f["sNgaySinh"]);
                data.SubmitChanges();
                return RedirectToAction("ManageInfoCT");
            }
            return View(thongtin);
        }
        [HttpGet]
        public ActionResult ManageInfoND()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN nd = (TAIKHOAN)Session["TaiKhoan"];
                var nguoidung = (from a in data.TAIKHOANs

                              join b in data.NGUOIDUNGs on a.Id equals b.Id_TaiKhoan
                                 where (nd.Id == b.Id_TaiKhoan)
                                 select new NguoiDung()
                              {
                                  sTaiKhoanND = a.TaiKhoan,
                                  sHotenND = a.HoTen,
                                  sMatKhauND = a.MatKhau,
                                  sId = b.Id,
                                  sSDTND=a.SDT,
                                  sEmailND=a.Email
                              }).FirstOrDefault();


                return View(nguoidung);
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ManageInfoND(FormCollection f)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];

            var nguoidung = (from a in data.TAIKHOANs

                             join b in data.NGUOIDUNGs on a.Id equals b.Id_TaiKhoan
                             where (tk.Id == b.Id_TaiKhoan)
                             select new NguoiDung()
                             {
                                 sTaiKhoanND = a.TaiKhoan,
                                 sHotenND = a.HoTen,
                                 sMatKhauND = a.MatKhau,
                                 sId = b.Id,
                                 sSDTND = a.SDT,
                                 sEmailND = a.Email
                             }).FirstOrDefault();

            var taikhoan = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);
            if (ModelState.IsValid)
            {
                taikhoan.HoTen = f["sHotenND"];
                taikhoan.MatKhau = f["sMatKhauND"];
                taikhoan.SDT = f["sSDTND"];
                taikhoan.Email = f["sEmailND"];
                
                data.SubmitChanges();
                return RedirectToAction("ManageInfoND");
            }
            return View(nguoidung);
        }

    }
}