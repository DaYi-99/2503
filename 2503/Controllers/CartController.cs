using _2503.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2503.Controllers
{
    public class CartController : Controller
    {
        dbSQL_2503DataContext db = new dbSQL_2503DataContext();

        public List<Cart> GetCart()
        {
            List<Cart> listcart = Session["Cart"] as List<Cart>;
            if (listcart == null)
            {
                // nếu  giỏ hàng chưa tồn tại thì khởi tạo
                listcart = new List<Cart>();
                Session["Cart"] = listcart;
            }
            return listcart;
        }

        // GET: Cart
        public ActionResult Index()
        {
            // Lấy ra session giỏ hàng
            List<Cart> listcart = GetCart();
            if (listcart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.SumQ = Sum_Quantities();
            ViewBag.SumM = Sum_Money();
            return View(listcart); 
        }

        public ActionResult CartPartial()
        {
            ViewBag.SumQ = Sum_Quantities();
            ViewBag.SumM = Sum_Money();
            return PartialView();
        }


        private int Sum_Quantities()
        {
            int iSumQ = 0;
            List<Cart> listcart = Session["Cart"] as List<Cart>;
            if (listcart != null)
            {
                iSumQ = listcart.Sum(n => n.iQuantity);
            }
            return iSumQ;
        }

        private double Sum_Money()
        {
            double iSumM = 0;
            List<Cart> listcart = Session["Cart"] as List<Cart>;
            if (listcart != null)
            {
                iSumM = listcart.Sum(n => n.iIntoMoney);
            }
            return iSumM;
        }

        public ActionResult AddtoCart(int id_pro, string url)
        {
            // Lấy ra session giỏ hàng
            List<Cart> listcart = GetCart();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            Cart pro = listcart.Find(n => n.iId_pro == id_pro);
            if (pro == null)
            {
                pro = new Cart(id_pro);
                listcart.Add(pro);
                return Redirect(url);
            }
            else
            {   // nếu có rồi thì cộng thêm
                pro.iQuantity++;
                return Redirect(url);
            }
        }

        public ActionResult DeleteProduct(int id_pro)
        {
            // Lấy ra session giỏ hàng
            List<Cart> listcart = GetCart();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            Cart pro = listcart.Find(n => n.iId_pro == id_pro);

            if (pro != null)
            {
                listcart.RemoveAll(n => n.iId_pro == id_pro);
                return RedirectToAction("Index");
            }
            if (listcart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            return RedirectToAction("Index");
        }


        public ActionResult UpdateProduct(int id_pro, FormCollection f)
        {
            // Lấy ra session giỏ hàng
            List<Cart> listcart = GetCart();

            // Kiểm tra sản phẩm đã có trong giỏ hàng chưa
            Cart pro = listcart.SingleOrDefault(n => n.iId_pro == id_pro);


            // Nếu tồn tại thì cho sửa số lượng
            if (pro != null)
            {
                pro.iQuantity = int.Parse(f["txtSL"].ToString());
            }
            return RedirectToAction("Index");
        }


        public ActionResult DeleteCart()
        {
            // Lấy ra session giỏ hàng
            List<Cart> listcart = GetCart();
            listcart.Clear();
            return RedirectToAction("Index", "Cart");
        }


        [HttpGet]
        public ActionResult Order()
        {
            //kiểm tra đăng nhập
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                //fill thông tin ra texbox
                return RedirectToAction("Login", "Customer");
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> listcart = GetCart();
            ViewBag.SumQ = Sum_Quantities();
            ViewBag.SumM = Sum_Money();
            ////ViewBag.Total = Total();
            return View(listcart);
        }

        [HttpPost]
        public ActionResult Order(FormCollection collection)
        {
            //var id = collection["idkh"];
            //var tenkh = collection["tenkh"];
            //var 



            //Them don hang
            ORDER ord = new ORDER();
            CUSTOMER cus = (CUSTOMER)Session["User"];
            List<Cart> cart = GetCart();

            ord.ID_CUS = cus.ID_CUS;
            ord.DATEORDER = DateTime.Now;
            var dateship = String.Format("{0:MM/dd/yyyy}", collection["Dateship"]);
            ord.DATESHIP = DateTime.Parse(dateship);
            ord.STTSHIP = false;
            ord.PAYMENT = false;

            db.ORDERs.InsertOnSubmit(ord);
            db.SubmitChanges();

            //them chi tiet don hang
            foreach (var item in cart)
            {
                ORDER_DETAIL odd = new ORDER_DETAIL();
                odd.ID_ORD = ord.ID_ORD;
                odd.ID_PRO = item.iId_pro;
                odd.QUANTITY = item.iQuantity;
                odd.UNITPRICE = (decimal)item.iPrice;
                db.ORDER_DETAILs.InsertOnSubmit(odd);
            }

            db.SubmitChanges();
            Session["Cart"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}