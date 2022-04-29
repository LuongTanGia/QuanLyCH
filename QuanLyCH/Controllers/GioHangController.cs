using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyCH.common;
using QuanLyCH.Models;
namespace QuanLyCH.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        MydataDataContext data = new MydataDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int id, string strURL)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.Find(n => n.magiay == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
       
        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhtien);
            }

            if (tt <= 300000)
            {
                return tt;
            }
            else
            {
                if (tt > 300000 && tt <= 1000000)
                {
                    return tt * 0.7;
                }
                else
                {
                    return tt * 0.5;
                }
            }


        }
        public ActionResult Giohang()
        {
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TienGiam1 = TongTien() * 0.7;
            ViewBag.TienGiam2 = TongTien() * 0.5;

            return View(lstGiohang);
        }
        public ActionResult miniGiohang()
        {
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {

            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
          
            return PartialView();
        }

       
        public ActionResult XoaGiohang(int id )
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.magiay == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.magiay == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGiohang(int id, System.Web.Mvc.FormCollection collection)
        {
            var E_giay = data.Giays.First(m => m.magiay == id);
            ViewBag.Max =  Convert.ToInt32( E_giay.soluongton);
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.magiay == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSolg"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatCaGiohang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.TienGiam1 = TongTien() * 0.7;
            ViewBag.TienGiam2 = TongTien() * 0.5;
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);

        }
        public ActionResult DatHang(System.Web.Mvc.FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            Giay s = new Giay();
            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            double tong = 0;
            
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.magiay = item.magiay;
                ctdh.soluong = item.iSoluong;
                ctdh.gia = (decimal)item.giagiam;
                s = data.Giays.Single(n => n.magiay == item.magiay);
                s.soluongton -= ctdh.soluong;
                tong += item.dThanhtien;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/neworder.html"));

            content = content.Replace("{{CustomerName}}", dh.KhachHang.hoten);
            content = content.Replace("{{Phone}}", dh.KhachHang.dienthoai);
            content = content.Replace("{{Email}}", dh.KhachHang.email);
            content = content.Replace("{{Address}}", dh.KhachHang.diachi);
            content = content.Replace("{{Total}}", TongTien().ToString("N0"));
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();


            new MailHelper().SendMail(dh.KhachHang.email, "Đơn hàng mới từ cửa hàng giày GDNC", content);
            new MailHelper().SendMail(toEmail, "Đơn hàng", content);

            data.SubmitChanges();
            Session["Giohang"] = null;

            return RedirectToAction("XacnhanDonhang", "GioHang");
        }
        
        /*public ActionResult NguoiDung()
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];


            var Ten = kh.hoten;
            
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Ten = Ten;
            return PartialView();
        }*/
        public ActionResult XacnhanDonhang()
        {
            return PartialView();
        }
        public ActionResult XacNhanAdmin()
        {
            return View();
        }
        public ActionResult Ten()
        {
            
            return PartialView();
            

        }

       
    }
}