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
    public class KhuVucController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();

        [HttpGet]
        public ActionResult Index(int? page, string strSearch)
        {
            if (Session["Admin"] == null || Session["Admin"].ToString() == "")
            {
                return Redirect("~/Admin/Home/Login");
            }
            ViewData["strSearch"] = strSearch;

            int iSize = 8;
            int iPageNum = (page ?? 1);
            return View(data.KHUVUCs.OrderByDescending(n => n.Id).ToPagedList(iPageNum, iSize));
        }

        [HttpPost]
        public ActionResult CreateKhuVuc(KHUVUC khuVuc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    data.KHUVUCs.InsertOnSubmit(khuVuc);
                    data.SubmitChanges();
                    TempData["ThongBao"] = "Thêm mới thành công";
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }

        [HttpPost]
        public ActionResult EditKhuVuc(int id, string ten)
        {
            try
            {
                var khuVuc = data.KHUVUCs.SingleOrDefault(p => p.Id == id);
                if (khuVuc != null)
                {
                    khuVuc.Ten = ten;
                    data.SubmitChanges();
                    TempData["ThongBao"] = "Cập nhật thành công";
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không tìm thấy khu vực tương ứng." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        public ActionResult XoaKhuVuc(int id)
        {
            try
            {
                var kv = data.KHUVUCs.SingleOrDefault(p => p.Id == id);
                if (kv != null)
                {
                    data.KHUVUCs.DeleteOnSubmit(kv);
                    data.SubmitChanges();
                    TempData["ThongBao"] = "Xóa thành công";
                }
                else
                {
                    TempData["ThongBao"] = "Không tìm thấy khu vực tương ứng";
                }
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Xóa thất bại. Lỗi: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
        

    }
}