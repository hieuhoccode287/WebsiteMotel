using System;
using System.ComponentModel.DataAnnotations;

namespace WEBSITE_MOTEL.Models
{
    public class ChuTro
    {
        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string sHotenCT { get; set; }

        public string sTaiKhoanCT { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
        public string sMatKhauCT { get; set; }

        public int sId { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có 10 chữ số.")]
        public string sSDTCT { get; set; }

        [Required(ErrorMessage = "Địa chỉ email không được để trống.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string sEmailCT { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống.")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2100", ErrorMessage = "Ngày sinh phải trong khoảng {1} đến {2}.")]
        public DateTime? sNgaySinh { get; set; }

        public string sFacebook { get; set; }

        [Required(ErrorMessage = "Căn cước công dân không được để trống.")]
        [StringLength(12, ErrorMessage = "CCCD phải có 12 ký tự.", MinimumLength = 12)]
        public string sCCCD { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự.")]
        public string sDiaChi { get; set; }

        public int sPhanQuyen { get; set; }  // E.g., 0 = User, 1 = Admin
    }
}
