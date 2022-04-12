using QuanLyCH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QuanLyCH.Controllers
{
    public class HomeController : Controller
    {
        MydataDataContext data = new MydataDataContext();
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Tongsoluong = TongSoLuongSanPham();
            ViewBag.Keyword = searchString;
            if (page == null) page = 1;
            var all_giay = (from s in data.Giays select s).OrderBy(m => m.magiay);
            if (!string.IsNullOrEmpty(searchString)) all_giay = (IOrderedQueryable<Giay>)all_giay.Where(a => a.tengiay.Contains(searchString));
            int pageSize = 6;
            int pageNum = page ?? 1;
            return View(all_giay.ToPagedList(pageNum, pageSize));
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
    }
}