using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using WEBSITE_MOTEL.Models;
using System.Linq; // Giả sử bạn có mô hình RoomDetail trong thư mục Models

namespace WEBSITE_MOTEL.Controllers
{
    public class PaymentController : Controller
    {
        // URL thanh toán
        private string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        private string vnp_Returnurl = "https://localhost:44300/Payment/PaymentReturn"; // Thay đổi URL này cho phù hợp với dự án
        private string vnp_TmnCode = "VNPAY_TEST"; // Mã Website tại VNPAY
        private string vnp_HashSecret = "SECRETKEY"; // Chuỗi bí mật của bạn

        public ActionResult CreatePayment(decimal amount)
        {
            string vnp_TxnRef = DateTime.Now.Ticks.ToString(); // Mã giao dịch (unique)
            string vnp_OrderInfo = "Thanh toán tiền cọc cho phòng trọ";
            string vnp_OrderType = "other"; // Loại hàng hóa
            string vnp_Amount = ((int)amount * 100).ToString(); // Đơn vị VND, nhân 100
            string vnp_Locale = "vn"; // Ngôn ngữ
            string vnp_IpAddr = Request.UserHostAddress;

            // Cấu hình tham số cho URL
            var vnpay = new SortedList<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", vnp_TmnCode },
                { "vnp_Amount", vnp_Amount },
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", vnp_TxnRef },
                { "vnp_OrderInfo", vnp_OrderInfo },
                { "vnp_OrderType", vnp_OrderType },
                { "vnp_ReturnUrl", vnp_Returnurl },
                { "vnp_IpAddr", vnp_IpAddr },
                { "vnp_Locale", vnp_Locale },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") }
            };

            // Tạo URL thanh toán
            string query = string.Join("&", vnpay.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            string signData = string.Join("&", vnpay.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            vnpay["vnp_SecureHashType"] = "SHA256";
            vnpay["vnp_SecureHash"] = HmacSHA256(signData, vnp_HashSecret);

            string paymentUrl = $"{vnp_Url}?{query}&vnp_SecureHash={vnpay["vnp_SecureHash"]}";

            return Redirect(paymentUrl);
        }

        // Hàm xử lý khi có phản hồi từ VNPAY
        public ActionResult PaymentReturn()
        {
            // Lấy các tham số phản hồi từ VNPAY
            var vnp_ResponseCode = Request.QueryString["vnp_ResponseCode"];
            if (vnp_ResponseCode == "00")
            {
                ViewBag.Message = "Giao dịch thành công!";
                // Cập nhật trạng thái thanh toán ở đây
            }
            else
            {
                ViewBag.Message = "Giao dịch thất bại!";
            }
            return View();
        }

        private static string HmacSHA256(string data, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
