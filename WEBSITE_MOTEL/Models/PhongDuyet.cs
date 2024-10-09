using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBSITE_MOTEL.Models
{
    public class PhongDuyet
    {
        public int sMa { get; set; }
        public int sIdDonHang { get; set; }
        public string sTenPhong { get; set; }
        public string sHoTen { get; set; }
        public int sDienTich { get; set; }
        public string sAnhBia { get; set; }
        public string sTenND { get; set; }
        public string sSDTND { get; set; }
        public int sIDCT { get; set; }
        public int sSoLuong { get; set; }
        public int sSoNguoiO { get; set; }
        [DataType(DataType.Date)]
        public DateTime? dNgayCapNhat { get; set; }
        public DateTime? dNgayDat { get; set; }
        public string sMoTa { get; set; }
        public double dGiaCa { get; set; }
        public string sSDT { get; set; }
        public string sEmail { get; set; }
        public double sNuoc { get; set; }
        public double sDien { get; set; }
        public double sInternet { get; set; }
        public double sGuiXe { get; set; }
        public string sDiaChi { get; set; }
        public byte sTrangThai { get; set; }
        public string sTenKV { get; set; }
    }
}