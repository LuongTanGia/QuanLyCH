using PagedList;
using QuanLyCH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCH.Controllers
{
    public class GiayController : Controller
    {
        // GET: Giay
        MydataDataContext data = new MydataDataContext();
        public ActionResult ListGiay()
        {

           

            KhachHang kh = (KhachHang)Session["Taikhoan"];
                if (Session["Taikhoan"] == null)
                {
                    return RedirectToAction("XacNhanAdmin", "GioHang");

                }
                else
                {
                    if (kh.id == 1)
                    {
                    var all_giay = from ss in data.Giays select ss;
                    return View(all_giay);
                }
                    else
                        return RedirectToAction("XacNhanAdmin", "GioHang");
                }

                return View();
                //MyDataDataContext data = new MyDataDataContext();   
        }
        public ActionResult Listdanhmuc() 
        {
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("XacNhanAdmin", "GioHang");

            }
            else
            {
                if (kh.id == 1)
                {
                    var all_danhmuc = from ss in data.TheLoais select ss;
                    return View(all_danhmuc);
                }
                else
                    return RedirectToAction("XacNhanAdmin", "GioHang");
            }

            return View();
        }
        public ActionResult Detail(int id)

        {
           /* if (Session["Taikhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }*/
            
            var D_giay = data.Giays.Where(m => m.magiay == id).First();
            return View(D_giay);
        }
        public ActionResult Creates()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Creates(FormCollection collection, TheLoai s)
        {
            var E_TenLoai = collection["tenloai"];
            var E_MaLoai = Convert.ToInt32(collection["maloai"]);

            if (string.IsNullOrEmpty(E_TenLoai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.tenloai = E_TenLoai.ToString();
                s.maloai = E_MaLoai;

                data.TheLoais.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("Listdanhmuc");
            }
            return this.Creates();
        }
        public ActionResult Edits(int id)
        {
            var E_TheLoai = data.TheLoais.First(m => m.maloai == id);
            return View(E_TheLoai);
        }
        [HttpPost]
        public ActionResult Edits(int id, FormCollection collection)
        {
            var E_TheLoai = data.TheLoais.First(m => m.maloai == id);
            var E_TenLoai = collection["tenloai"];
            var E_MaLoai = Convert.ToInt32(collection["maloai"]);

            E_TheLoai.maloai = id;
            if (string.IsNullOrEmpty(E_TenLoai))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_TheLoai.tenloai = E_TenLoai;
                E_TheLoai.maloai = E_MaLoai;
                
                UpdateModel(E_TheLoai);
                data.SubmitChanges();
                return RedirectToAction("ListTheLoai");
            }
            return this.Edits(id);
        }
        public ActionResult Deletes(int id)
        {
            var D_TheLoai = data.TheLoais.First(m => m.maloai == id);
            return View(D_TheLoai);
        }
        [HttpPost]
        public ActionResult Deletes(int id, FormCollection collection)
        {
            var D_TheLoai = data.TheLoais.Where(m => m.maloai == id).First();
            data.TheLoais.DeleteOnSubmit(D_TheLoai);
            data.SubmitChanges();
            return RedirectToAction("Listdanhmuc");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Giay s)
        {
            var E_tengiay = collection["tengiay"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_Discoun = Convert.ToDecimal(collection["giagiam"]);
            if (string.IsNullOrEmpty(E_tengiay))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.tengiay = E_tengiay.ToString();
                s.hinh = E_hinh.ToString();
                s.giaban = E_giaban;
                s.ngaycapnhat = E_ngaycapnhat;
                s.soluongton = E_soluongton;
                s.giagiam = E_giaban;
                s.giamgia = 1.0;
                data.Giays.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("ListGiay");
            }
            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var E_giay = data.Giays.First(m => m.magiay == id);
            return View(E_giay);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_giay = data.Giays.First(m => m.magiay == id);
            var E_tengiay = collection["tengiay"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            var E_Discount = Convert.ToDouble(collection["giamgia"]);
            var E_DisPrice = Convert.ToDecimal(collection["giaban"]) * Convert.ToDecimal(collection["giamgia"]);
            E_giay.magiay = id;
            if (string.IsNullOrEmpty(E_tengiay))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_giay.tengiay = E_tengiay;
                E_giay.hinh = E_hinh;
                E_giay.giaban = E_giaban;
                E_giay.ngaycapnhat = E_ngaycapnhat;
                E_giay.soluongton = E_soluongton;
                E_giay.giamgia = E_Discount;
                E_giay.giagiam = E_DisPrice;
                UpdateModel(E_giay);
                data.SubmitChanges();
                return RedirectToAction("ListGiay");
            }
            return this.Edit(id);
        }

        public ActionResult Delete(int id)
        {
            var D_giay = data.Giays.First(m => m.magiay == id);
            return View(D_giay);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_giay = data.Giays.Where(m => m.magiay == id).First();
            data.Giays.DeleteOnSubmit(D_giay);
            data.SubmitChanges();
            return RedirectToAction("ListGiay");
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }

        public ActionResult LoaiSanPham()
        {
            var loaisanpham = from s in data.TheLoais select s;
            return PartialView(loaisanpham);
        }
       
        
        public ActionResult SPTheoLoai(int id)
        {
            var sptl = from ss in data.Giays where ss.maloai == id select ss;
            return PartialView(sptl);
            
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
        public ActionResult Map()
        {
            return PartialView();
        }

    }
}