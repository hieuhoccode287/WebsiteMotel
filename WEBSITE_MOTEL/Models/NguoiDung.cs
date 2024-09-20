using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBSITE_MOTEL.Models
{
    public class NguoiDung
    {
        public int sId { get; set; }

        public string sTaiKhoanND { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        public string sHotenND { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string sSDTND { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string sEmailND { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự.")]
        public string sMatKhauND { get; set; }

        public string sMatKhauNhapLai { get; set; }
    }
}
