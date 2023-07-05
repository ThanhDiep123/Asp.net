using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebBookStore.Controllers
{
    public class AdminController : Controller
    {

        dbQLBANSACHDataContext db = new dbQLBANSACHDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach(int? page, string searchstring, string current)
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
                lst = db.SACHes.Where(n => n.Tensach.Contains(searchstring)).ToList();
            }
            else
            {
                lst = db.SACHes.ToList();
            }
            ViewBag.CurrentFilter = searchstring;
            int pagesize = 6;
            int pagenum = (page ?? 1);

            lst = lst.OrderByDescending(n => n.Masach).ToList();


            return View(lst.ToPagedList(pagenum, pagesize));
        }
    
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // gán các tri nguoi2 dùng nhập du liệu
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                /// gán các gia tri cho dối tượng dược tao mới
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            //dua du lieu vao dropdownList
            //Lay danh sach chu de, sap xep theo tang dan ten chu de
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileUpload)
        {
            //dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDl
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file, luu y bổ sung thư viện using system.Io
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    //kiem tra hinh anh ton tai chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //luu hinh ảnh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    //luu vao CSDl
                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sach");
            } 
        }
        //sua
        [HttpGet]
        public ActionResult Suasach(int id)

        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            if (sach == null)
            {
                Response.StatusCode = 403;
                return null;
            }
            //dua du lieu vao dropdownList
            //Lay danh sach chu de, sap xep theo tang dan ten chu de
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileUpload)
        {
            //dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDl
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file, luu y bổ sung thư viện using system.Io
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham/"), fileName);
                    //kiem tra hinh anh ton tai chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //luu hinh ảnh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sach.Anhbia = fileName;
                    //luu vao CSDl
                    UpdateModel(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
        }
        //hien thi san pham
        public ActionResult Chitietsach(int id)
        {
            //lay ra doi tuong sach thepo ma
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost,ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }
    }


}