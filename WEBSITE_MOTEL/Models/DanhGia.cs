using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEBSITE_MOTEL.Models
{
    public class DanhGia
    {
        public int sId { get; set; }
        public int sId_Phong { get; set; }
        public string sTenNguoiDanhGia { get; set; }
        [DataType(DataType.Date)]
        public DateTime? dNgayDanhGia { get; set; }
        public string sMoTaDanhGia { get; set; }
        public int? sDanhGiaRating { get; set; }
        public double? AverageRating { get; set; }
    }
}