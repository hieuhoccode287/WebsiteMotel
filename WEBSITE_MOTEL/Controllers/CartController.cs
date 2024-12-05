using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;
using WEBSITE_MOTEL.Models;
using VNPAY_CS_ASPX;
using Microsoft.AspNetCore.Mvc;
using WebGrease;
using System.Globalization;

namespace WEBSITE_MOTEL.Controllers
{
    public class CartController : Controller
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        public TAIKHOAN GetAcc()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["TaiKhoan"];
            return tk;
        }
        public ActionResult CartP(int? page)
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/DangNhap");
            }

            else
            {
                TAIKHOAN tk = GetAcc();
                int iSize = 4;
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

                var phong = (from a in data.PHONGTROs
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                             join g in data.DONHANGs on a.Id equals g.Id_Phong
                             join e in data.TAIKHOANs on g.Id_NguoiDung equals e.Id
                             join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                             join f in data.DANHGIAs on a.Id equals f.Id_Phong into danhgia
                             from f in danhgia.DefaultIfEmpty()  // Left join DANHGIA để tránh trùng
                             where (tk.Id == g.Id_NguoiDung)   // Ensure tk is defined properly
                             group new { a, c, g, e, kv, f } by g.IdDH into groupedPhong
                             select new RoomDetail()
                             {
                                 sMa = groupedPhong.FirstOrDefault().g.IdDH,
                                 sMa2 = groupedPhong.FirstOrDefault().g.Id_Phong,
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
                                 sTenKV = groupedPhong.FirstOrDefault().kv.Ten ?? "",
                                 sTrangThai = (byte)groupedPhong.FirstOrDefault().g.TrangThai,
                                 sNgayDat = (DateTime)groupedPhong.FirstOrDefault().g.NgayDat,
                             });

                return View(phong.OrderBy(r => r.sTrangThai)  // Sắp xếp theo TrangThai
                 .ThenByDescending(r => r.sNgayDat)  // Sau đó sắp xếp theo NgayDat giảm dần
                 .ToPagedList(iPageNum, iSize));


            }
        }

        [HttpPost]
        public JsonResult CreateVnPayUrl(string orderId, decimal amount)
        {
            if (string.IsNullOrEmpty(orderId) || amount <= 0)
            {
                return Json(new { success = false, message = "Invalid order ID or amount." });
            }

            // Get Config Info
            string vnp_Returnurl = "https://localhost:44348/Cart/VnPayReturn"; // Return URL
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; // VNPay payment URL
            string vnp_TmnCode = "6LY4D08E"; // Terminal Id
            string vnp_HashSecret = "L32MNN2JMUFDPMQZXE1IQB3QFKV75MI3"; // Secret Key

            try
            {
                VnPayLibrary vnpay = new VnPayLibrary();
                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString("F0"));
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
                vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang: {orderId}");
                vnpay.AddRequestData("vnp_OrderType", "other");
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", orderId);
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
                vnpay.AddRequestData("vnp_Locale", "vn");


                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                return Json(new { success = true, paymentUrl });
            }
            catch (Exception ex)
            {
                // Log exception (optional)
                return Json(new { success = false, message = "Error creating payment URL.", error = ex.Message });
            }
        }

        public ActionResult VnPayReturn()
        {
            // Initialize display variables for the view
            string message = string.Empty;
            string terminalId = string.Empty;
            string txnRef = string.Empty;
            string vnpayTranNo = string.Empty;
            string amount = string.Empty;
            string bankCode = string.Empty;
            DateTime dt = DateTime.Now;

            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = "L32MNN2JMUFDPMQZXE1IQB3QFKV75MI3"; // Secret Key
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    // Get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                        
                    }
                }

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
                terminalId = vnpay.GetResponseData("vnp_TmnCode");
                amount = (Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100).ToString("N0");
                bankCode = vnpay.GetResponseData("vnp_BankCode");

                //@@@
                string payDate = vnpay.GetResponseData("vnp_PayDate");
                if (!string.IsNullOrEmpty(payDate))
                {
                    dt = DateTime.ParseExact(payDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        // Kiểm tra xem giao dịch đã tồn tại hay chưa
                        var existingTransaction = data.CHITIETDONHANGs.FirstOrDefault(x => x.Id_DH == orderId);
                        if (existingTransaction == null)
                        {
                            decimal? parsedAmount = null;
                            if (decimal.TryParse(amount, out decimal result))
                            {
                                parsedAmount = result;
                            }

                            // Thêm giao dịch mới nếu chưa tồn tại
                            var ctdh = new CHITIETDONHANG
                            {
                                Id_DH = (int)orderId,
                                TmnCode = terminalId,
                                TransactionNo = vnp_TransactionStatus,
                                NgayTao = dt,
                                Status = 1,
                                Amount = parsedAmount
                            };

                            var dh = data.DONHANGs.SingleOrDefault(n => n.IdDH == orderId);
                            if (dh != null)
                            {
                                dh.TrangThai = 3;
                            }

                            data.CHITIETDONHANGs.InsertOnSubmit(ctdh);
                            data.SubmitChanges();
                            message = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        }
                        else
                        {
                            message = "Giao dịch đã được xử lý trước đó.";
                        }
                    }
                    else
                    {
                        message = "Có lỗi xảy ra trong quá trình xử lý. Mã lỗi: " + vnp_ResponseCode;
                    }
                    txnRef = orderId.ToString();
                    vnpayTranNo = vnpayTranId.ToString();
                }
                else
                {
                    message = "Có lỗi xảy ra trong quá trình xử lý";
                }

            }

            // Pass values to the view
            ViewBag.Date = dt;
            ViewBag.Message = message;
            ViewBag.TerminalID = terminalId;
            ViewBag.TxnRef = txnRef;
            ViewBag.VnPayTranNo = vnpayTranNo;
            ViewBag.Amount = amount;
            ViewBag.BankCode = bankCode;

            return View();
        }

        [HttpPost]
        public JsonResult Delete(int maDH)
        {
            try
            {
                // Find the order by IdDH
                var dh = data.DONHANGs.SingleOrDefault(d => d.IdDH == maDH);
                if (dh == null)
                {
                    return Json(new { code = 404, msg = "Đơn hàng không tồn tại." }, JsonRequestBehavior.AllowGet);
                }

                // Delete the order
                data.DONHANGs.DeleteOnSubmit(dh);
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Xóa phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa phòng thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Checkout(int maDH)
        {
            try
            {
                // Find the order by IdDH
                var dh = data.DONHANGs.SingleOrDefault(d => d.IdDH == maDH);
                var p = data.PHONGTROs.SingleOrDefault(n => n.Id == dh.Id_Phong);
                if (dh == null)
                {
                    return Json(new { code = 404, msg = "Đơn hàng không tồn tại." }, JsonRequestBehavior.AllowGet);
                }
                p.SoLuong += 1;
                dh.TrangThai = 4;
                //data.SubmitChanges();

                return Json(new { code = 200, msg = "Trả phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Trả phòng thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SubmitReview(int sMa2, int userId,  string review, int rating)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(review) || rating < 1 || rating > 5)
                {
                    return Json(new { success = false, msg = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại!" });
                }

                // Tạo đối tượng review mới
                var newReview = new DANHGIA
                {
                    Id_Phong = sMa2,
                    Id_TaiKhoan = userId,
                    MoTa = review,
                    DanhGiaRating = rating,
                    NgayDanhGia = DateTime.Now
                };

                data.DANHGIAs.InsertOnSubmit(newReview);
                data.SubmitChanges();

                // Phản hồi thành công
                return Json(new { success = true, msg = "Đánh giá của bạn đã được gửi thành công!" });
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu cần)
                // Logger.Error(ex);

                // Phản hồi lỗi
                return Json(new { success = false, msg = "Đã xảy ra lỗi khi gửi đánh giá. Vui lòng thử lại!" });
            }
        }

    }
}