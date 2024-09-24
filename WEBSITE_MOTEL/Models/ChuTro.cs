using System;
using System.ComponentModel.DataAnnotations;

namespace WEBSITE_MOTEL.Models
{
    public class ChuTro
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        public string sHotenCT { get; set; }

        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        public string sTaiKhoanCT { get; set; }

        public string sMatKhauCT { get; set; }

        public int sId { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string sSDTCT { get; set; }

        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string sEmailCT { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        public DateTime? sNgaySinh { get; set; }

        public string sFacebook { get; set; }

        [Required(ErrorMessage = "Căn cước công dân là bắt buộc.")]
        public string sCCCD { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string sDiaChi { get; set; }

        public int sPhanQuyen { get; set; }
    }
}
