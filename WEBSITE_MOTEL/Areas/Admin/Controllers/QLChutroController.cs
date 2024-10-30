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
                          where b.PhanQuyen == 2 && b.TrangThai == 1
                          select new ChuTro()
                          {
                              sHotenCT = b.HoTen,
                              sTaiKhoanCT = b.TaiKhoan,
                              sMatKhauCT = b.MatKhau,
                              sNgaySinh = b.NgaySinh,
                              sEmailCT = b.Email,
                              sDiaChi = b.DiaChi,
                              sSDTCT = b.SDT,
                              sPhanQuyen = (int)b.PhanQuyen,
                              sCCCD = b.CCCD,
                              sId = b.Id,
                          })
                  .OrderByDescending(ct => ct.sId); // Sắp xếp theo Id


            var pagedChutro = new PagedList<ChuTro>(chutro, pageNumber, pageSize);
            return View(pagedChutro);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var chuTro = data.TAIKHOANs.SingleOrDefault(pt => pt.Id == id);
                if (chuTro != null)
                {
                    data.TAIKHOANs.DeleteOnSubmit(chuTro);
                    data.SubmitChanges();
                    TempData["ThongBao"] = "Xóa thành công";
                }
                else
                {
                    TempData["ThongBao"] = "Không tìm thấy thông tin tài khoản";
                }
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Xóa thất bại. Lỗi: " + ex.Message;
            }

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
                          where b.PhanQuyen == 2 && b.Id == id
                          select new ChuTro()
                          {
                              sHotenCT = b.HoTen,
                              sTaiKhoanCT = b.TaiKhoan,
                              sMatKhauCT = b.MatKhau,
                              sNgaySinh = b.NgaySinh,
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ChuTro model)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }

            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors if the model state is invalid
            }

            // Find the TAIKHOAN entity based on the user ID from the model
            var taikhoan = data.TAIKHOANs.FirstOrDefault(t => t.Id == model.sId); // Replace 'Id' with the actual primary key field of your TAIKHOAN model

            if (taikhoan != null)
            {
                // Update the fields with the new data from the model
                taikhoan.HoTen = model.sHotenCT;
                taikhoan.MatKhau = model.sMatKhauCT;
                taikhoan.SDT = model.sSDTCT;
                taikhoan.Email = model.sEmailCT;
                taikhoan.CCCD = model.sCCCD;
                taikhoan.DiaChi = model.sDiaChi;
                taikhoan.NgaySinh = Convert.ToDateTime(model.sNgaySinh);

                // Submit changes to the database
                data.SubmitChanges();

                // Update the session with the new account information
                Session["TaiKhoan"] = taikhoan;
                TempData["SuccessMessage"] = "Thông tin đã được cập nhật thành công!";

                // Redirect to the Index action after successful update
                return RedirectToAction("Edit");
            }

            // If no account found, return the view with the model for user feedback
            ModelState.AddModelError("", "Không tìm thấy tài khoản để cập nhật.");
            return View(model);
        }

        public ActionResult Details(int? page,int id)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            int iSize = 4;
            int iPageNum = (page ?? 1);

            var test = data.TAIKHOANs.FirstOrDefault(n => n.Id == id);

            var tenct = test?.HoTen;
            var idct = test?.Id;

            ViewBag.TenCT = tenct;
            ViewBag.ID = idct;

            var phong = (from a in data.PHONGTROs
                         join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                         join d in data.TAIKHOANs on a.Id_ChuTro equals d.Id
                         join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                         select new RoomDetail()
                         {
                             sMa = (int)a.Id,
                             sTenPhong = a.TenPhong,
                             sHoTen = d.HoTen,
                             sTrangThai = (int)a.TrangThai,
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