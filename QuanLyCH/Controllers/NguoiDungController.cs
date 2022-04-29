using QuanLyCH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using QuanLyCH.Controllers;


namespace QuanLyCH.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        MydataDataContext data = new MydataDataContext();

        [HttpGet]
        public ActionResult DangKy()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var makh = Convert.ToInt32(collection["makh"]);
            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var MatkhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);


            if (String.IsNullOrEmpty(MatkhauXacNhan))
                ViewData["NhapMKXN"] = "Phải nhập đủ thông tin!";
            if (String.IsNullOrEmpty(hoten))
                ViewData["NhapHoten"] = "Phải nhập đủ họ tên";
            if (String.IsNullOrEmpty(dienthoai))
                ViewData["nhapDT"] = "Phải nhập số điện thoại";
             if (String.IsNullOrEmpty(diachi))
                ViewData["nhapDC"] = "Phải nhập địa chỉ";
             if (String.IsNullOrEmpty(email))
                ViewData["nhapEmail"] = "Phải nhập Email";

             if (String.IsNullOrEmpty(tendangnhap))
                ViewData["NhapTK"] = "Phải nhập tên đăng nhập";
             if (String.IsNullOrEmpty(matkhau))
                ViewData["nhapMK"] = "Phải nhập mật khẩu";
             if (matkhau.Length > 8)
                ViewData["dodaiMK"] = "mật khẩu phải nhiều hơn 8 ký tự";
             if (String.IsNullOrEmpty(ngaysinh))
                ViewData["nhapNS"] = "Phải nhập ngày sinh";
            
            else
            {
                if (!matkhau.Equals(MatkhauXacNhan))
                {
                    ViewData["MatkhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else
                {
                    kh.makh = makh;
                    kh.hoten = hoten;
                    kh.tendangnhap = tendangnhap;
                    kh.matkhau = matkhau;
                    kh.email = email;
                    kh.diachi = diachi;
                    kh.dienthoai = dienthoai;
                    kh.ngaysinh = DateTime.Parse(ngaysinh);
/*                    kh.id = Convert.ToInt32(collection["id"]);
*/                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    return RedirectToAction("DangNhap");
                }
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.tendangnhap == tendangnhap && n.matkhau == matkhau);
            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["TaiKhoan"] = kh;
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Nhom()
        {
            return PartialView();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("DangNhap");
        }

        public ActionResult profile(int id)
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            kh.makh = id;
            var E_Pro = data.KhachHangs.First(m => m.makh == id);
            return View(E_Pro);
        }
        [HttpPost]
        public ActionResult profile(int id, FormCollection collection)
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            kh.makh = id;
            var E_Pro = data.KhachHangs.First(m => m.makh == id);

            var hoten = collection["hoten"];
            var tendangnhap = collection["tendangnhap"];
            var matkhau = collection["matkhau"];
            var MatkhauXacNhan = collection["MatKhauXacNhan"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["ngaysinh"]);

            E_Pro.makh = id;
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                
                kh.tendangnhap = tendangnhap;
                kh.matkhau = matkhau;
                kh.email = email;
                kh.diachi = diachi;
                kh.dienthoai = dienthoai;
                kh.ngaysinh = DateTime.Parse(ngaysinh);
                kh.id = Convert.ToInt32(collection["id"]);

                UpdateModel(E_Pro);
                data.SubmitChanges();
                return RedirectToAction("Index","Home");
            }
            return this.profile(id);
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



       /* public void sendPass(string pass , System.Web.Mvc.FormCollection collection )

        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.ngaydat = DateTime.Now;



            MailAddress fromAddress = new MailAddress("luong1tan1gia123@gmail.com", "Cua Hang Giay");

            MailAddress toAddress = new MailAddress(kh.email.ToString());

            const string fromPassword = "fccyjktykdfjcgaq";

            string subject = "Xac Nhan Don Hang";
                      
            string Tien = TongTien().ToString();

            string Soluong = TongSoLuongSanPham().ToString();

            string Ten = kh.hoten.ToString();

            string NgayDat = dh.ngaydat.ToString();

            string madh = dh.madon.ToString();

            
            SmtpClient smtp = new SmtpClient

            {

                Host = "smtp.gmail.com",

                Port = 587,

                EnableSsl = true,

                DeliveryMethod = SmtpDeliveryMethod.Network,

                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)

            };

            using (MailMessage message = new MailMessage(fromAddress, toAddress)

            {

                Subject = subject,

                Body =

                        "<p>Cam On Ban Da Dung Dich Vu Cua Shop Giay : </p>" +
                         "<p>Don Hang Gom : " + Soluong + " San Pham  </p>" +
                         "<p>Ten : " + Ten + "   </p>" +
                          "<p>Email : " + kh.email.ToString() + "   </p>" +
                         "<p color = red>Tong Gia Tien : " + Tien + " VND  </p>"
                        + "<p>Ngay Nhan Hang :" + NgayDat + "</p>"
                        + "<p>Ma Don Hang : " + madh + "</p>" +
                        "<button> Xac Nhan </Button>",

                IsBodyHtml = true,

            })
            {

                smtp.Send(message);


                url = "https://localhost:44391/momo/ConfirmPaymentClient";

                
            }
             

        }*/
        public ActionResult XacnhanDonhang()
        {
            return RedirectToAction("XacnhanDonhang", "GioHang");

        }


        public JsonResult CheckUsername(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = data.KhachHangs.Where(x => x.tendangnhap == userdata).SingleOrDefault();

            if (SearchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public JsonResult CheckEmail(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = data.KhachHangs.Where(x => x.email == userdata).SingleOrDefault();

            if (SearchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }

        public JsonResult CheckPhone(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = data.KhachHangs.Where(x => x.dienthoai.ToString() == userdata).SingleOrDefault();

            if (SearchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
    }
}