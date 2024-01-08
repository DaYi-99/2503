using _2503.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2503.Controllers
{
    public class CustomerController : Controller
    {

        dbSQL_2503DataContext db = new dbSQL_2503DataContext();

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection, CUSTOMER cus)
        {
            //tác vụ đăng ký
                var name = collection["NAME"];
                var un = collection["UN"];
                var pw = collection["PW"];
                var email = collection["EMAIL"];
                var address = collection["ADDRESS"];
                var phone = collection["PHONENUMBER"];
                //var date = String.Format("{0:dd/MM/yyyy}", collection["DATEOFBIRTH"]);
                var date = String.Format(DateTime.Now.ToString("dd/MM/yyyy"), collection["DATEOFBIRTH"]);
            if (String.IsNullOrEmpty(name))
                {
                    ViewData["Err1"] = "Chưa nhập tên khách hàng";
                }
                else if (String.IsNullOrEmpty(un))
                {
                    ViewData["Err2"] = "Chưa nhập tên đăng nhập";
                }
                else if (String.IsNullOrEmpty(pw))
                {
                    ViewData["Err3"] = "Chưa nhập mật khẩu";
                }
                //else if (String.IsNullOrEmpty(email))
                //{
                //    ViewData["Err4"] = "Chưa nhập email";
                //}
                else if (String.IsNullOrEmpty(address))
                {
                    ViewData["Err5"] = "Chưa nhập địa chỉ";
                }
                else if (String.IsNullOrEmpty(phone))
                {
                    ViewData["Err6"] = "Chưa nhập số điện thoại";
                }
                //else if (String.IsNullOrEmpty(date))
                //{
                //    ViewData["Err6"] = "Chưa nhập ngày sinh";
                //}
                else
                {
                    //- Thêm mới khách hàng
                    cus.NAME = name;
                    cus.UN = un;
                    cus.PW = pw;
                    cus.EMAIL = email;
                    cus.ADDRESS = address;
                    cus.PHONENUMBER = phone;
                    cus.DATEOFBIRTH = DateTime.Parse(date);

                    db.CUSTOMERs.InsertOnSubmit(cus);
                    db.SubmitChanges();
                    return RedirectToAction("Index","Home");
                }
                return this.Register();
        }


        [HttpGet]
        public ActionResult Login(/*FormCollection collection*/)
        {
            ////tác vụ đăng nhập
            //string un = collection["tendn"];
            //string pw = collection["mk"];
            //if (un.ToString() == "" && pw.ToString() == "")
            //{
            //    ViewData["login"] = "Vui lòng điền đầy đủ thông tin";
            //}

            //else if (un.ToString() != "" && pw.ToString() != "")
            //{
            //    //gán các giá trị cho đối tượng
            //    CUSTOMER cu = db.CUSTOMERs.SingleOrDefault(n => n.UN == un && n.PW == pw);
            //    if (cu != null)
            //    {
            //        ViewBag.Inform = "Chúc mừng đăng nhập thành công";
            //        Session["User"] = cu;
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //        ViewBag.Inform = "Tên đăng nhập hoặc mật khẩu không đúng";
            //}
            return View();
        }


        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //tác vụ đăng nhập
            string un = collection["tendn"];
            string pw = collection["mk"];
            if (un.ToString() == "" && pw.ToString() == "")
            {
                ViewData["login"] = "Vui lòng điền đầy đủ thông tin";
            }

            else if (un.ToString() != "" && pw.ToString() != "")
            {
                //gán các giá trị cho đối tượng
                CUSTOMER cu = db.CUSTOMERs.SingleOrDefault(n => n.UN == un && n.PW == pw);
                if (cu != null)
                {
                    ViewBag.Inform = "Chúc mừng đăng nhập thành công";
                    Session["User"] = cu;
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Inform = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}



//tác vụ đăng nhập
//string un = user;
//string pw = pass;
//if (un.ToString() != "" && pw.ToString() != "")
//{
//    //gán các giá trị cho đối tượng
//    CUSTOMER cus = db.CUSTOMERs.SingleOrDefault(n => n.UN == user && n.PW == pass);
//    if (cus != null)
//    {
//        ViewBag.Inform = "Chúc mừng đăng nhập thành công";
//        Session["User"] = cus;
//        return RedirectToAction("Index", "Home");
//    }
//    else
//        ViewBag.Inform = "Tên đăng nhập hoặc mật khẩu không đúng";
//}
//return View();