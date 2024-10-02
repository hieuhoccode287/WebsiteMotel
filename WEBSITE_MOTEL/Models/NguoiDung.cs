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
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Số điện thoại phải có 10 chữ số.")]
        public string sSDTND { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string sEmailND { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string sMatKhauND { get; set; }
    }
}
