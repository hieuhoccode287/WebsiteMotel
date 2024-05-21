using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Areas.Admin.Controllers
{
    public class QLPhongController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Admin/QLPhong
        public ActionResult Index(int? page, string strSearch)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            ViewData["strSearch"] = strSearch;
            ViewBag.IdPH = new SelectList(data.PHONGTROs.ToList().OrderBy(n => n.TenPhong), "Id", "TenPhong");
            int iSize = 3;
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
                return View(phong.ToList().Where(n=>n.sIDCT == 1).OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
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

                return View(phong.ToList().Where(n => n.sIDCT == 1).OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var phong = data.PHONGTROs.SingleOrDefault(n => n.Id == id);
            if (phong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phong);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var phong = data.PHONGTROs.SingleOrDefault(n => n.Id == id);
            if (phong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
           
           
            data.PHONGTROs.DeleteOnSubmit(phong);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}