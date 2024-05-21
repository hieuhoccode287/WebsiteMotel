using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBSITE_MOTEL.Models
{
    public class Search
    {
        TimNhaTroDataContext data = new TimNhaTroDataContext();
        private string keyword;
        private int totalRecord;

        public Search()
        {
        }

        public Search(string keyword, ref int totalRecord)
        {
            this.keyword = keyword;
            this.totalRecord = totalRecord;
        }

        public List<string> ListName(string keyword)
        {
            return data.PHONGTROs.Where(x=>x.TenPhong.Contains(keyword)).Select(x=>x.TenPhong).ToList();
        }
        public List<ViewPhong> SearchS (string keyword,ref int totalRecord)
        {
            totalRecord = data.PHONGTROs.Where(x => x.TenPhong.Contains(keyword)).Count();
            var model = from a in data.PHONGTROs
                        join b in data.CHUTROs
                        on a.Id_ChuTro equals b.Id
                        where a.TenPhong.Contains(keyword)
                        select new ViewPhong()
                        {
                            sHotenCT= b.SDT,
                            dNgayCapNhat = (DateTime)a.Ngay,
                        };
            model.OrderByDescending(x => x.dNgayCapNhat);
                return model.ToList();
        }
    }
}