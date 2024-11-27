using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Areas.Admin.Controllers
{
    public class DUYETCTController : Controller
    {
        // GET: Admin/DUYETCT
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        public ActionResult CTApprove(int? page, string strSearch)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            ViewData["strSearch"] = strSearch;

            int iSize = 5;
            int iPageNum = (page ?? 1);
            if (!string.IsNullOrEmpty(strSearch))
            {
                var ct = (from a in data.TAIKHOANs
                          where a.TrangThai == 0
                          select new TaiKhoan()
                          {
                              sId = a.Id,
                              sTaiKhoanCT = a.TaiKhoan,
                              sMatKhauCT = a.MatKhau,
                              sHotenCT = a.HoTen,
                              sCCCD = a.CCCD,
                              sDiaChi = a.DiaChi,
                              sTrangThai = (int)a.TrangThai,
                              sEmailCT = a.Email,
                              sNgaySinh = (DateTime)a.NgaySinh,
                              sSDTCT = a.SDT,
                              sidcardphoto = a.IdCardPhoto,
                              sidcardphoto2 = a.IdCardPhoto2,
                          });
                return View(ct.OrderByDescending(n => n.sTrangThai == 0).ToPagedList(iPageNum, iSize));
            }
            else
            {
                var ct = (from a in data.TAIKHOANs
                          where a.TrangThai == 0
                          select new TaiKhoan()
                          {
                              sId = a.Id,
                              sTaiKhoanCT = a.TaiKhoan,
                              sMatKhauCT = a.MatKhau,
                              sHotenCT = a.HoTen,
                              sCCCD = a.CCCD,
                              sDiaChi = a.DiaChi,
                              sTrangThai = (int)a.TrangThai,
                              sEmailCT = a.Email,
                              sNgaySinh = (DateTime)a.NgaySinh,
                              sSDTCT = a.SDT,
                              sidcardphoto = a.IdCardPhoto,
                              sidcardphoto2 = a.IdCardPhoto2,
                          });
                return View(ct.OrderByDescending(n => n.sTrangThai == 0).ToPagedList(iPageNum, iSize));
            }
        }
        public ActionResult DuyetCT(int id)
        {
            TAIKHOAN CT = data.TAIKHOANs.FirstOrDefault(p => p.TrangThai == 0);
            if (CT != null)
            {
                CT.TrangThai = 1;
                data.SubmitChanges();
                TempData["Message"] = "Duyệt tài khoản thành công!";
            }
            return RedirectToAction("CTApprove");

        }
        public ActionResult BoDuyetCT(int id)
        {
            data.SubmitChanges();
            var chutros = data.TAIKHOANs.Where(p => p.Id == id);
            data.TAIKHOANs.DeleteAllOnSubmit(chutros);
            data.SubmitChanges();
            var tk = data.TAIKHOANs.SingleOrDefault(p => p.Id == id);
            if (tk != null)
            {
                data.TAIKHOANs.DeleteOnSubmit(tk);
                data.SubmitChanges();
                ViewBag.ThongBao = "Xóa tài khoản thành công";
            }
            return RedirectToAction("CTApprove");
        }
    }
}