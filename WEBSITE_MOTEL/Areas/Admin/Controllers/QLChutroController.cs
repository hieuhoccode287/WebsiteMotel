using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Areas.Admin.Controllers
{
    public class QLChutroController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Admin/QLChutro
        public ActionResult Index(int? page, string strSearch)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }

            ViewData["strSearch"] = strSearch;
            ViewBag.IdCT = new SelectList(data.TAIKHOANs.ToList().OrderBy(n => n.HoTen), "Id", "HoTen");

            int pageSize = 5;
            int pageNumber = page ?? 1;

            var chutro = (from b in data.TAIKHOANs 
                          where b.PhanQuyen == 2
                          select new ChuTro()
                          {
                              sHotenCT = b.HoTen,
                              sTaiKhoanCT = b.TaiKhoan,
                              sMatKhauCT = b.MatKhau,
                              /* sNgaySinh =  a.NgaySinh,*/
                              sEmailCT = b.Email,
                              sDiaChi = b.DiaChi,
                              sSDTCT = b.SDT,
                              sPhanQuyen = (int)b.PhanQuyen,
                              sCCCD = b.CCCD,
                              sId = b.Id,
                          });

            var pagedChutro = new PagedList<ChuTro>(chutro, pageNumber, pageSize);
            return View(pagedChutro);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var chutro = (from b in data.TAIKHOANs
                          where b.PhanQuyen == 2
                          select new ChuTro()
                          {
                              sHotenCT = b.HoTen,
                              sTaiKhoanCT = b.TaiKhoan,
                              sMatKhauCT = b.MatKhau,
                              /* sNgaySinh =  a.NgaySinh,*/
                              sEmailCT = b.Email,
                              sDiaChi = b.DiaChi,
                              sSDTCT = b.SDT,
                              sPhanQuyen = (int)b.PhanQuyen,
                              sCCCD = b.CCCD,
                              sId = b.Id,
                          }).SingleOrDefault(n=>n.sId == id);
            if (chutro == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chutro);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var chutro = data.TAIKHOANs.SingleOrDefault(n => n.Id == id);
            if (chutro == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            data.TAIKHOANs.DeleteOnSubmit(chutro);
            data.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            var chutro = (from b in data.TAIKHOANs
                          where b.PhanQuyen == 2
                          select new ChuTro()
                          {
                              sHotenCT = b.HoTen,
                              sTaiKhoanCT = b.TaiKhoan,
                              sMatKhauCT = b.MatKhau,
                              /* sNgaySinh =  a.NgaySinh,*/
                              sEmailCT = b.Email,
                              sDiaChi = b.DiaChi,
                              sSDTCT = b.SDT,
                              sPhanQuyen = (int)b.PhanQuyen,
                              sCCCD = b.CCCD,
                              sId = b.Id,
                          }).FirstOrDefault();
            if (chutro == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chutro);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            var chutro = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);
            if (ModelState.IsValid)
            {
                tk.HoTen = f["sHotenCT"];
                tk.MatKhau = f["sMatKhauCT"];
                tk.SDT = f["sSDTCT"];
                tk.Email = f["sEmailCT"];
                tk.CCCD = f["sCCCD"];
                tk.DiaChi = f["sDiaChi"];
                tk.NgaySinh = Convert.ToDateTime(f["sNgaySinh"]);
   
                /*chutro.sCCCD = f["sCCCD"];
                chutro.sFacebook = f["sFacebook"];*/
                data.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(chutro);
        }
        public ActionResult Details(int? page,int id)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            int iSize = 4;
            int iPageNum = (page ?? 1);
            var phong = (from a in data.PHONGTROs
                         join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                         join d in data.TAIKHOANs on a.Id_ChuTro equals d.Id
                         join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                         select new RoomDetail()
                         {
                             sMa = (int)a.Id,
                             sTenPhong = a.TenPhong,
                             /*sHoTen = b.HoTen,*/
                            
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
                             sTenKV = kv.Ten,
                         });
            if (phong == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phong.ToList().Where(n => n.sIDCT == id).OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
        }
        [HttpGet]
        public ActionResult DeleteP(int id)
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
        public ActionResult DeletePConfirm(int id, FormCollection f)
        {
            var phong = data.PHONGTROs.SingleOrDefault(n => n.Id == id);
            if (phong == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            data.PHONGTROs.DeleteOnSubmit(phong);
            data.SubmitChanges();
            return Redirect("~/Areas/Admin/QLChutro");
        }
    }
}