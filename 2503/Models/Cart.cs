using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2503.Models
{
    public class Cart
    {
        dbSQL_2503DataContext db = new dbSQL_2503DataContext();

        public int iId_pro { set; get; }
        public string iName { set; get; }
        public string iImage { set; get; }
        public Double iPrice { set; get; }

        public int iQuantity { set; get; }
        public Double iIntoMoney
        {
            get { return iQuantity * iPrice; }
        }


        //khởi tạo giỏ hàng theo mã sách với số lượng mặc định là 1
        public Cart(int id_pro)
        {
            iId_pro = id_pro;
            PRODUCT pro = db.PRODUCTs.Single(n => n.ID_PRO == iId_pro);
            iName = pro.NAME;
            iImage = pro.IMAGE;
            iPrice = double.Parse(pro.PRICE.ToString());
            iQuantity = 1;
        }
    }
}