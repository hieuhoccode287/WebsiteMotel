using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WEBSITE_MOTEL.Models;

namespace WEBSITE_MOTEL.Controllers
{
    public class CHUTROController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: CHUTRO
        public ActionResult Index(int? page)
        {
           
                int iSize = 3;
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
                                 sTenKV = kv.Ten,
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
                             });

                return View(phong.OrderByDescending(n => n.dNgayCapNhat).ToPagedList(iPageNum, iSize));

           
           
        }
    }
}