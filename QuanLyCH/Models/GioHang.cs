using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyCH.Models
{
    public class GioHang
    {
        MydataDataContext data = new MydataDataContext();

            
       
        public int magiay { get; set; }
        public int max { get; set; }
        [Display(Name = "Tên Giày")]
        public string tengiay { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string hinh { get; set; }
        [Display(Name = "Giá Bán")]
        public Double giagiam { get; set; }
        [Display(Name = "Số lượng")]
        public int iSoluong { get; set; }
        [Display(Name = "Thành tiền")]
        public Double dThanhtien
        {
            get { return iSoluong * giagiam; }
        }
        public GioHang(int id )
        {
            
            magiay = id;
            Giay giay = data.Giays.Single(n => n.magiay == magiay);
            tengiay = giay.tengiay;
            max = (int)giay.soluongton;
            hinh = giay.hinh;
            giagiam = double.Parse(giay.giagiam.ToString());
            iSoluong = 1;
        }

    }
}