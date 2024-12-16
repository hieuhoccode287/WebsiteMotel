using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using WEBSITE_MOTEL.Models;


namespace WEBSITE_MOTEL.Controllers
{
    public class MotelController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Motel
        public ActionResult Index(int? page, string strSearch)
        {
            ViewData["strSearch"] = strSearch;
            /*ViewBag.IdPH = new SelectList(data.PHONGTROs.ToList().OrderBy(n => n.TenPhong), "Id", "TenPhong");*/
            int iSize = 5;
            int iPageNum = (page ?? 1);

            var userdanhgia = (from a in data.DANHGIAs
                               join b in data.TAIKHOANs on a.Id_TaiKhoan equals b.Id
                               select new DanhGia()
                               {
                                   sId = a.Id,
                                   sId_Phong = (int)a.Id_Phong,
                                   sTenNguoiDanhGia = b.HoTen,
                                   sDanhGiaRating = a.DanhGiaRating,
                                   sMoTaDanhGia = a.MoTa,
                                   dNgayDanhGia = (DateTime)a.NgayDanhGia,
                               });
            ViewBag.Reviews = userdanhgia;

            var allRatings = (from a in data.DANHGIAs
                               join b in data.PHONGTROs on a.Id_Phong equals b.Id
                               select new DanhGia()
                               {
                                   sId = a.Id,
                                   sId_Phong = (int)a.Id_Phong,
                                   sDanhGiaRating = a.DanhGiaRating,
                                   sMoTaDanhGia = a.MoTa,
                                   dNgayDanhGia = (DateTime)a.NgayDanhGia,
                               });
            ViewBag.AllRatings = allRatings;




            if (!string.IsNullOrEmpty(strSearch))
            {
                var phong = (from a in data.PHONGTROs
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro into images
                             from c in images.DefaultIfEmpty()  // Left join IMAGEs để tránh trùng
                             join e in data.TAIKHOANs on a.Id_ChuTro equals e.Id
                             join d in data.KHUVUCs on a.KhuVuc equals d.Id
                             join f in data.DANHGIAs on a.Id equals f.Id_Phong into danhgia
                             from f in danhgia.DefaultIfEmpty()  // Left join DANHGIA để tránh trùng
                             where a.TrangThai == 1 &&
                                   (string.IsNullOrEmpty(strSearch) || a.TenPhong.Contains(strSearch) || a.MoTa.Contains(strSearch))
                             group new { a, c, e, d, f } by a.Id into groupedPhong
                             select new RoomDetail()
                             {
                                 sMa = groupedPhong.FirstOrDefault().a.Id,
                                 sTenPhong = groupedPhong.FirstOrDefault().a.TenPhong ?? "",
                                 sHoTen = groupedPhong.FirstOrDefault().e.HoTen ?? "",
                                 sDanhGia = groupedPhong.FirstOrDefault().f != null ? (int?)groupedPhong.FirstOrDefault().f.DanhGiaRating : (int?)null,
                                 sDienTich = (int)groupedPhong.FirstOrDefault().a.DienTich,
                                 sSoluong = (int)groupedPhong.FirstOrDefault().a.SoLuong,
                                 sSoNguoiO = (int)groupedPhong.FirstOrDefault().a.SoNguoiO,
                                 sAnhBia = groupedPhong.FirstOrDefault().a.AnhBia ?? "",
                                 sMoTa = groupedPhong.FirstOrDefault().a.MoTa ?? "",
                                 sMoTaDanhGia = groupedPhong.FirstOrDefault().f != null ? groupedPhong.FirstOrDefault().f.MoTa : "",
                                 dNgayCapNhat = (DateTime)groupedPhong.FirstOrDefault().a.Ngay,
                                 dNgayDanhGia = groupedPhong.FirstOrDefault().f != null ? (DateTime?)groupedPhong.FirstOrDefault().f.NgayDanhGia : null,
                                 dGiaCa = (double)groupedPhong.FirstOrDefault().a.GiaCa,
                                 dGiaCoc = (double)groupedPhong.FirstOrDefault().a.GiaCoc,
                                 sSDT = groupedPhong.FirstOrDefault().e.SDT ?? "",
                                 sEmail = groupedPhong.FirstOrDefault().e.Email ?? "",
                                 sUrl_Path = groupedPhong.FirstOrDefault().c.Url_Path ?? "",
                                 sUrl_Path2 = groupedPhong.FirstOrDefault().c.Url_Path2 ?? "",
                                 sUrl_Path3 = groupedPhong.FirstOrDefault().c.Url_Path3 ?? "",
                                 sUrl_Path4 = groupedPhong.FirstOrDefault().c.Url_Path4 ?? "",
                                 sMap = groupedPhong.FirstOrDefault().a.Map ?? "",
                                 sDiaChi = groupedPhong.FirstOrDefault().a.Diachi ?? "",
                                 sDien = (double)groupedPhong.FirstOrDefault().a.Dien,
                                 sNuoc = (double)groupedPhong.FirstOrDefault().a.Nuoc,
                                 sGuiXe = (double)groupedPhong.FirstOrDefault().a.GuiXe,
                                 sInternet = (double)groupedPhong.FirstOrDefault().a.Internet,
                                 sTenKV = groupedPhong.FirstOrDefault().d.Ten ?? "",
                             })
                             .GroupBy(r => r.sMa)  // Nhóm theo Id_Phong (sMa)
                             .Select(g => g.First())  // Chọn bản ghi đầu tiên trong mỗi nhóm
                             .Distinct()
                             .ToList();


                return View(phong.OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));



            }
            else
            {
                var phong = (from a in data.PHONGTROs
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro into images
                             from c in images.DefaultIfEmpty()  // Left join IMAGEs để tránh trùng
                             join e in data.TAIKHOANs on a.Id_ChuTro equals e.Id
                             join d in data.KHUVUCs on a.KhuVuc equals d.Id
                             join f in data.DANHGIAs on a.Id equals f.Id_Phong into danhgia
                             from f in danhgia.DefaultIfEmpty()  // Left join DANHGIA để tránh trùng
                             where a.TrangThai == 1
                             group new { a, c, e, d, f } by a.Id into groupedPhong
                             select new RoomDetail()
                             {
                                 sMa = groupedPhong.FirstOrDefault().a.Id,
                                 sTenPhong = groupedPhong.FirstOrDefault().a.TenPhong ?? "",
                                 sHoTen = groupedPhong.FirstOrDefault().e.HoTen ?? "",
                                 sDanhGia = groupedPhong.FirstOrDefault().f != null ? (int?)groupedPhong.FirstOrDefault().f.DanhGiaRating : (int?)null,
                                 sDienTich = (int)groupedPhong.FirstOrDefault().a.DienTich,
                                 sSoluong = (int)groupedPhong.FirstOrDefault().a.SoLuong,
                                 sSoNguoiO = (int)groupedPhong.FirstOrDefault().a.SoNguoiO,
                                 sAnhBia = groupedPhong.FirstOrDefault().a.AnhBia ?? "",
                                 sMoTa = groupedPhong.FirstOrDefault().a.MoTa ?? "",
                                 sMoTaDanhGia = groupedPhong.FirstOrDefault().f != null ? groupedPhong.FirstOrDefault().f.MoTa : "",
                                 dNgayCapNhat = (DateTime)groupedPhong.FirstOrDefault().a.Ngay,
                                 dNgayDanhGia = groupedPhong.FirstOrDefault().f != null ? (DateTime?)groupedPhong.FirstOrDefault().f.NgayDanhGia : null,
                                 dGiaCa = (double)groupedPhong.FirstOrDefault().a.GiaCa,
                                 dGiaCoc = (double)groupedPhong.FirstOrDefault().a.GiaCoc,
                                 sSDT = groupedPhong.FirstOrDefault().e.SDT ?? "",
                                 sEmail = groupedPhong.FirstOrDefault().e.Email ?? "",
                                 sUrl_Path = groupedPhong.FirstOrDefault().c.Url_Path ?? "",
                                 sUrl_Path2 = groupedPhong.FirstOrDefault().c.Url_Path2 ?? "",
                                 sUrl_Path3 = groupedPhong.FirstOrDefault().c.Url_Path3 ?? "",
                                 sUrl_Path4 = groupedPhong.FirstOrDefault().c.Url_Path4 ?? "",
                                 sMap = groupedPhong.FirstOrDefault().a.Map ?? "",
                                 sDiaChi = groupedPhong.FirstOrDefault().a.Diachi ?? "",
                                 sDien = (double)groupedPhong.FirstOrDefault().a.Dien,
                                 sNuoc = (double)groupedPhong.FirstOrDefault().a.Nuoc,
                                 sGuiXe = (double)groupedPhong.FirstOrDefault().a.GuiXe,
                                 sInternet = (double)groupedPhong.FirstOrDefault().a.Internet,
                                 sTenKV = groupedPhong.FirstOrDefault().d.Ten ?? "",
                             })
                             .GroupBy(r => r.sMa)  // Nhóm theo Id_Phong (sMa)
                             .Select(g => g.First())  // Chọn bản ghi đầu tiên trong mỗi nhóm
                             .Distinct()
                             .ToList();

                return View(phong.OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));


            }
        }

        /*private List<PHONGTRO> LayPhong(int count)
        {

            return data.PHONGTROs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
*/
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        // GET: User
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            int state = Convert.ToInt32(Request.QueryString["id"]);
            ViewBag.TenDN = "Người dùng";
            var sTenDN = collection["TaiKhoan"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (tk != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = tk;
                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TaiKhoan"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatKhau;
                        Response.Cookies["TaiKhoan"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                        return RedirectToAction("Index", "Motel");
                    }
                    else
                    {
                        Response.Cookies["TaiKhoan"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                        return RedirectToAction("Post", "Motel");
                    }

                }
                else
                {
                    ViewBag.ThongBao = "Sai mật khẩu hoặc tài khoản";
                }
            }
            return View();

        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogout");
        }
        [HttpGet]
        public ActionResult Support()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddSupport(string name, string phone, string mota)
        {
            try
            {
                var ht = new HOTRO();
                ht.Ten = name;
                ht.Sdt = phone;
                ht.MotaVande = mota;
                data.HOTROs.InsertOnSubmit(ht);
                data.SubmitChanges();
                return Json(new { code = 200, msg = "Yêu cầu đã được gửi đi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Gửi thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SearchPartial()
        {
            var listKV = from a in data.KHUVUCs select a;
            return PartialView(listKV);
        }

        public ActionResult SearchKQ(int? page, int IdKV, string IdGia, string IdSL, string IdDT)
        {
            int iSize = 5;
            int iPageNum = (page ?? 1);

            var userdanhgia = (from a in data.DANHGIAs
                               join b in data.TAIKHOANs on a.Id_TaiKhoan equals b.Id
                               select new DanhGia()
                               {
                                   sId = a.Id,
                                   sId_Phong = (int)a.Id_Phong,
                                   sTenNguoiDanhGia = b.HoTen,
                                   sDanhGiaRating = a.DanhGiaRating,
                                   sMoTaDanhGia = a.MoTa,
                                   dNgayDanhGia = (DateTime)a.NgayDanhGia,
                               });
            ViewBag.Reviews = userdanhgia;

            var allRatings = (from a in data.DANHGIAs
                              join b in data.PHONGTROs on a.Id_Phong equals b.Id
                              select new DanhGia()
                              {
                                  sId = a.Id,
                                  sId_Phong = (int)a.Id_Phong,
                                  sDanhGiaRating = a.DanhGiaRating,
                                  sMoTaDanhGia = a.MoTa,
                                  dNgayDanhGia = (DateTime)a.NgayDanhGia,
                              });
            ViewBag.AllRatings = allRatings;

            var listSearch = from a in data.PHONGTROs
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro into images
                             from c in images.DefaultIfEmpty()  // Left join IMAGEs để tránh trùng
                             join e in data.TAIKHOANs on a.Id_ChuTro equals e.Id
                             join d in data.KHUVUCs on a.KhuVuc equals d.Id
                             join f in data.DANHGIAs on a.Id equals f.Id_Phong into danhgia
                             from f in danhgia.DefaultIfEmpty()  // Left join DANHGIA để tránh trùng
                             where a.TrangThai == 1
                             group new { a, c, e, d, f } by a.Id into groupedPhong
                             select new RoomDetail()
                             {
                                 sMa = groupedPhong.FirstOrDefault().a.Id,
                                 sTenPhong = groupedPhong.FirstOrDefault().a.TenPhong ?? "",
                                 sHoTen = groupedPhong.FirstOrDefault().e.HoTen ?? "",
                                 sDanhGia = groupedPhong.FirstOrDefault().f != null ? (int?)groupedPhong.FirstOrDefault().f.DanhGiaRating : (int?)null,
                                 sDienTich = (int)groupedPhong.FirstOrDefault().a.DienTich,
                                 sSoluong = (int)groupedPhong.FirstOrDefault().a.SoLuong,
                                 sSoNguoiO = (int)groupedPhong.FirstOrDefault().a.SoNguoiO,
                                 sAnhBia = groupedPhong.FirstOrDefault().a.AnhBia ?? "",
                                 sMoTa = groupedPhong.FirstOrDefault().a.MoTa ?? "",
                                 sMoTaDanhGia = groupedPhong.FirstOrDefault().f != null ? groupedPhong.FirstOrDefault().f.MoTa : "",
                                 dNgayCapNhat = (DateTime)groupedPhong.FirstOrDefault().a.Ngay,
                                 dNgayDanhGia = groupedPhong.FirstOrDefault().f != null ? (DateTime?)groupedPhong.FirstOrDefault().f.NgayDanhGia : null,
                                 dGiaCa = (double)groupedPhong.FirstOrDefault().a.GiaCa,
                                 dGiaCoc = (double)groupedPhong.FirstOrDefault().a.GiaCoc,
                                 sSDT = groupedPhong.FirstOrDefault().e.SDT ?? "",
                                 sEmail = groupedPhong.FirstOrDefault().e.Email ?? "",
                                 sUrl_Path = groupedPhong.FirstOrDefault().c.Url_Path ?? "",
                                 sUrl_Path2 = groupedPhong.FirstOrDefault().c.Url_Path2 ?? "",
                                 sUrl_Path3 = groupedPhong.FirstOrDefault().c.Url_Path3 ?? "",
                                 sUrl_Path4 = groupedPhong.FirstOrDefault().c.Url_Path4 ?? "",
                                 sMap = groupedPhong.FirstOrDefault().a.Map ?? "",
                                 sDiaChi = groupedPhong.FirstOrDefault().a.Diachi ?? "",
                                 sDien = (double)groupedPhong.FirstOrDefault().a.Dien,
                                 sNuoc = (double)groupedPhong.FirstOrDefault().a.Nuoc,
                                 sGuiXe = (double)groupedPhong.FirstOrDefault().a.GuiXe,
                                 sInternet = (double)groupedPhong.FirstOrDefault().a.Internet,
                                 sTenKV = groupedPhong.FirstOrDefault().d.Ten ?? "",
                                 sIdKV = (int)groupedPhong.FirstOrDefault().a.KhuVuc,
                             };
            // Filter by Khu vực (Area)
            if (IdKV != 0)
            {
                listSearch = listSearch.Where(a => a.sIdKV == IdKV);
            }

            // Filter by number of occupants (Số người ở)
            if (!string.IsNullOrEmpty(IdSL) && IdSL != "Số người ở")
            {
                int numberOfOccupants;
                if (int.TryParse(IdSL, out numberOfOccupants))
                {
                    listSearch = listSearch.Where(a => a.sSoNguoiO == numberOfOccupants);
                }
            }

            // Filter by area (Diện tích)
            if (!string.IsNullOrEmpty(IdDT) && IdDT != "Diện tích(m2)")
            {
                string[] areaRange = IdDT.Split(' ');
                if (areaRange.Length >= 2)
                {
                    int areaMin = 0;
                    int areaMax = int.MaxValue;
                    if (areaRange[0] == "Dưới")
                    {
                        int.TryParse(areaRange[1], out areaMax);
                        listSearch = listSearch.Where(a => a.sDienTich < areaMax);
                    }
                    else if (areaRange[0] == "Trên")
                    {
                        int.TryParse(areaRange[1], out areaMin);
                        listSearch = listSearch.Where(a => a.sDienTich > areaMin);
                    }
                    else if (areaRange.Length == 3 && areaRange[1] == "-")
                    {
                        int.TryParse(areaRange[0], out areaMin);
                        int.TryParse(areaRange[2], out areaMax);
                        listSearch = listSearch.Where(a => a.sDienTich >= areaMin && a.sDienTich <= areaMax);
                    }
                }
            }

            // Filter by price (Giá)
            if (!string.IsNullOrEmpty(IdGia) && IdGia != "Mức giá?")
            {
                string[] priceRange = IdGia.Split(' ');
                if (priceRange.Length >= 2)
                {
                    double priceMin;
                    double priceMax;
                    if (priceRange[0] == "Dưới")
                    {
                        double.TryParse(priceRange[1], out priceMax);
                        listSearch = listSearch.Where(a => a.dGiaCa < priceMax);
                    }
                    else if (priceRange[0] == "Trên")
                    {
                        double.TryParse(priceRange[1], out priceMin);
                        listSearch = listSearch.Where(a => a.dGiaCa > priceMin);
                    }
                    else if (priceRange.Length == 3 && priceRange[1] == "-")
                    {
                        double.TryParse(priceRange[0], out priceMin);
                        double.TryParse(priceRange[2], out priceMax);
                        listSearch = listSearch.Where(a => a.dGiaCa >= priceMin && a.dGiaCa <= priceMax);
                    }
                }
            }

            // Check if no results were found
            if (!listSearch.Any())
            {
                ViewBag.ErrorMessage = "Không tìm thấy kết quả phù hợp.";
            }

            // Pass the search filters back to the ViewBag
            ViewBag.IdKV = IdKV;
            ViewBag.IdGia = IdGia;
            ViewBag.IdSL = IdSL;
            ViewBag.IdDT = IdDT;

            // Return the view with results, ordered by the update date
            return View("Index", listSearch.OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));
        }



        public ActionResult Map()
        {
            return View();
        }
        public ActionResult TEST()
        {
            return View();
        }
        public ActionResult Guide()
        {
            return View();
        }
        public ActionResult News()
        {
            return View();
        }
        public ActionResult StartPage()
        {
            return View();
        }
        
        public ActionResult NewsPartial()
        {
            return PartialView();
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult QDinh()
        {
            return PartialView();
        }
        public ActionResult GThieu()
        {
            return PartialView();
        }
        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }
        [HttpGet]
        public JsonResult BookRoom()
        {
            try
            {
                TAIKHOAN tk = GetAcc();
                var bookroom = (from a in data.TAIKHOANs
                                where (tk.Id == a.Id)
                                select new
                                {
                                    sHotenND = a.HoTen,
                                    sId = a.Id,
                                    sSDTND = a.SDT,

                                }).SingleOrDefault();
                return Json(new { code = 200, dh = bookroom, msg = "Đặt thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult BookRoom1(int idphong)
        {
            try
            {
                var bookroom = (from a in data.PHONGTROs
                                where (a.Id == idphong)
                                select new
                                {
                                    sTenPhong = a.TenPhong,
                                    sGia = a.GiaCa

                                }).SingleOrDefault();
                return Json(new { code = 200, dh = bookroom, msg = "Đặt thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveData(int idphong)
        {
            try
            {
                // Get the current user account
                TAIKHOAN tk = GetAcc();
                if (tk == null)
                {
                    return Json(new { code = 400, msg = "Tài khoản không hợp lệ." }, JsonRequestBehavior.AllowGet);
                }

                // Fetch user information
                var nd = data.TAIKHOANs.SingleOrDefault(n => n.Id == tk.Id);
                if (nd == null)
                {
                    return Json(new { code = 400, msg = "Người dùng không tồn tại." }, JsonRequestBehavior.AllowGet);
                }

                // Check if the room exists
                var phong = data.PHONGTROs.SingleOrDefault(n => n.Id == idphong);
                if (phong == null)
                {
                    return Json(new { code = 400, msg = "Phòng trọ không tồn tại." }, JsonRequestBehavior.AllowGet);
                }

                // Proceed with saving the booking and approval information

                var dh = new DONHANG();
                dh.NgayDat = DateTime.Now;
                dh.Id_NguoiDung = nd.Id;
                dh.Id_Phong = idphong;
                dh.TrangThai = 2;
                dh.TienCoc = phong.GiaCoc;


                data.DONHANGs.InsertOnSubmit(dh);
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Đặt thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lỗi. " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}