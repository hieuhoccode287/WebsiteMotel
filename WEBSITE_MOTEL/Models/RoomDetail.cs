using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBSITE_MOTEL.Models
{
    public class RoomDetail
    {
        public int sMa { get; set; }
        public string sTenPhong { get; set; }
        public string sHoTen { get; set; }
        public int sDienTich { get; set; }
        public string sAnhBia { get; set; }
        public string sTen { get; set; }
        public int sIDCT { get; set; }
        public int sSoluong { get; set; }
        [DataType(DataType.Date)]
        public DateTime? dNgayCapNhat { get; set; }
        public string sMoTa { get; set; }
        public double dGiaCa { get; set; }
        public string sSDT { get; set; }
        public string sEmail { get; set; }
        public string sUrl_Path { get; set; }
        public string sUrl_Path2 { get; set; }
        public string sUrl_Path3 { get; set; }
        public string sUrl_Path4 { get; set; }
        public double sNuoc { get; set; }
        public double sDien { get; set; }
        public double sInternet { get; set; }
        public double sGuiXe { get; set; }
        public string sDiaChi { get; set; }
        public byte sDoiTuong { get; set; }
        public byte sTrangThai { get; set; }
        
        public string sTenKV { get; set; }
    }
}