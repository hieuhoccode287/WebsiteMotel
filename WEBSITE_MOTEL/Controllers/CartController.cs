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
                var phong = (from a in data.PHONGTROs
                             join c in data.IMAGEs on a.Id equals c.Id_PhongTro
                             join g in data.DONHANGs on a.Id equals g.Id_Phong
                             join e in data.TAIKHOANs on g.Id_NguoiDung equals e.Id
                             join kv in data.KHUVUCs on a.KhuVuc equals kv.Id
                             where (tk.Id == g.Id_NguoiDung) && (g.TrangThai == 2 || g.TrangThai == 3|| g.TrangThai == 1)
                             select new RoomDetail()
                             {
                                 sMa = g.IdDH,
                                 sTenPhong = a.TenPhong,
                                 sHoTen = e.HoTen,
                                 sDienTich = (int)a.DienTich,
                                 sSoluong = (int)a.SoLuong,
                                 sSoNguoiO = (int)a.SoNguoiO,
                                 sAnhBia = a.AnhBia,
                                 sMoTa = a.MoTa,
                                 dNgayCapNhat = (DateTime)a.Ngay,
                                 dGiaCa = (double)a.GiaCa,
                                 dGiaCoc = (double)a.GiaCoc,
                                 sSDT = e.SDT,
                                 sEmail = e.Email,
                                 sUrl_Path = c.Url_Path,
                                 sUrl_Path2 = c.Url_Path2,
                                 sUrl_Path3 = c.Url_Path3,
                                 sUrl_Path4 = c.Url_Path4,
                                 sMap = a.Map,
                                 sDiaChi = a.Diachi,
                                 sDien = (double)a.Dien,
                                 sNuoc = (double)a.Nuoc,
                                 sGuiXe = (double)a.GuiXe,
                                 sInternet = (double)a.Internet,
                                 sTenKV = kv.Ten,
                                 sTrangThai = (byte)g.TrangThai,
                                 sNgayDat = (DateTime)g.NgayDat,
                             });
                return View(phong.ToList().OrderByDescending(n => n.sNgayDat).ToPagedList(iPageNum, iSize));
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

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        decimal? parsedAmount = null;
                        if (decimal.TryParse(amount, out decimal result))
                        {
                            parsedAmount = result;
                        }

                        var ctdh = new CHITIETDONHANG
                        {
                            Id_DH = (int)orderId,
                            TmnCode = terminalId,
                            TransactionNo = vnp_TransactionStatus,
                            NgayTao = DateTime.Now,
                            Status = 0,
                            Amount = parsedAmount
                        };
                        data.CHITIETDONHANGs.InsertOnSubmit(ctdh);
                        data.SubmitChanges();
                        message = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
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
                // Delete the order
                data.DONHANGs.DeleteOnSubmit(dh);
                data.SubmitChanges();

                return Json(new { code = 200, msg = "Trả phòng thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Trả phòng thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}