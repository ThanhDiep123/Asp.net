using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBookStore.Models
{
    public class Giohang
    {
        dbQLBANSACHDataContext data = new dbQLBANSACHDataContext();

        public int iMasach { set; get; }
        public String sTensach { set; get; }
        public String sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int Masach)
        {
            iMasach = Masach;
            SACH sach = data.SACHes.Single(n => n.Masach == iMasach);
            sTensach = sach.Tensach;
            sAnhbia = sach.Anhbia;
            dDongia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}