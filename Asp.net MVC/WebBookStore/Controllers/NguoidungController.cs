﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class NguoidungController : Controller
    {
        // GET: Nguoidung
        dbQLBANSACHDataContext db = new dbQLBANSACHDataContext();

        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }


        // Post: ham dk(post)
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            // gán các gia trị người dùng nhập liệu
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập điện thoại";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Phải nhập điện thoại";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi7"] = "Phải nhập Địa chỉ";
            }
            else
            {
                ///Gan giá trị cho dồi tượng được tạo mới (kh)
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");

            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap() {
            return View(); 
        }
        public ActionResult Dangnhap(FormCollection collection)
        {
            //gán các gia trị người dùng nhập liệu
            var tendn = collection["TenDn"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phai nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phai nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mửng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                   
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }


      
        
    }
}