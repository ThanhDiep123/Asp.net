using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;


using PagedList;
using PagedList.Mvc;
namespace WebBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        dbQLBANSACHDataContext data = new dbQLBANSACHDataContext();
       
        // GET: BookStore
      public ActionResult Diep_2120120212()
        {
            return View(data.asp_2120120212s.ToList());
        }
        
        
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        // GET: BookStore
        public ActionResult Index(int? page,string searchstring,string current)
        {
            var lst = new List<SACH>();
            if (searchstring != null)
            {
                page = 1;
            }
            else
            {
                searchstring = current;
            }
            if (!string.IsNullOrEmpty(searchstring))
            {
                lst = data.SACHes.Where(n => n.Tensach.Contains(searchstring)).ToList();
            }
            else
            {
                lst = data.SACHes.ToList();
            }
            ViewBag.CurrentFilter = searchstring;
            int pagesize = 6;
            int pagenum = (page ?? 1);
           
            lst = lst.OrderByDescending(n => n.Masach).ToList();


            return View(lst.ToPagedList(pagenum, pagesize));
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhaxuatban()
        {
            var Nhaxuatban = from nxb in data.NHAXUATBANs select nxb; 
            return PartialView(Nhaxuatban);
        }
        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
        
    }
}