using System;
using System.ComponentModel.DataAnnotations;

namespace WEBSITE_MOTEL.Models
{
    public class RoomDetail
    {
        public int sMa { get; set; }

        [Required(ErrorMessage = "Tên phòng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên phòng không được vượt quá 100 ký tự.")]
        public string sTenPhong { get; set; }

        [Required(ErrorMessage = "Tên chủ phòng là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên chủ phòng không được vượt quá 100 ký tự.")]
        public string sHoTen { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Diện tích phải lớn hơn 0.")]
        public int sDienTich { get; set; }

        public string sAnhBia { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string sTen { get; set; }

        public int sIDCT { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int sSoluong { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số người ở phải lớn hơn 0.")]
        public int sSoNguoiO { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dNgayCapNhat { get; set; }

        public string sMoTa { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá cả phải là một số hợp lệ.")]
        public double dGiaCa { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Giá cọc phải là một số hợp lệ.")]
        public double dGiaCoc { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string sSDT { get; set; }

        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string sEmail { get; set; }

        public string sUrl_Path { get; set; }
        public string sUrl_Path2 { get; set; }
        public string sUrl_Path3 { get; set; }
        public string sUrl_Path4 { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá nước phải là một số hợp lệ.")]
        public double sNuoc { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá điện phải là một số hợp lệ.")]
        public double sDien { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá internet phải là một số hợp lệ.")]
        public double sInternet { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá gửi xe phải là một số hợp lệ.")]
        public double sGuiXe { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        public string sDiaChi { get; set; }

        public int sTrangThai { get; set; }
        public int sIdKV { get; set; }
        public string sTenKV { get; set; }

        [DataType(DataType.Date)]
        public DateTime? sNgayDat { get; set; }

        public string sMap { get; set; }
    }
}
