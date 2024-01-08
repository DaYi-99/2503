using _2503.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Windows;

namespace _2503.Controllers
{
    public class AdminController : Controller
    {

        dbSQL_2503DataContext db = new dbSQL_2503DataContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            //gan gia tri nguoi dung nhap vao
            var un = collection["username"];
            var pw = collection["password"];
            if (String.IsNullOrEmpty(un))
            {
                ViewData["Err1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(pw))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UN == un && n.PW == pw);
                if (ad != null)
                {
                    Session["Admin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Inform = "Sai thông tin đăng nhập";
            }
            return View();
        }


        public ActionResult Category()
        {
            return View(db.CATEGORies.ToList());
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddCategory(CATEGORy cat, FormCollection col)
        {
            //  Gán giá trị
            var tenmh = col["NAME"];
            if (String.IsNullOrEmpty(tenmh))
            {
                ViewData["Err"] = "Vui lòng nhập tên mặt hàng!";
                return View();
            }
            else
            {
                ////ViewBag.Info = "Thêm mới mặt hàng thành công";
                MessageBox.Show("Thêm mới mặt hàng thành công");
                db.CATEGORies.InsertOnSubmit(cat);
                db.SubmitChanges();
            }
            return RedirectToAction("Category");
        }



        public ActionResult Product(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.PRODUCTs.ToList().OrderBy(n => n.ID_PRO).ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.ID_CAT = new SelectList(db.CATEGORies.ToList().OrderBy(n => n.NAME), "ID_CAT", "NAME");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProduct(PRODUCT pro, HttpPostedFileBase up, FormCollection col)
        {
            ViewBag.ID_CAT = new SelectList(db.CATEGORies.ToList().OrderBy(n => n.NAME), "ID_CAT", "NAME");

            var tensp = col["NAME"];
            var giaban = col["PRICE"];
            var size = col["SIZE"];
            var mota = col["DESCRIPTION"];
            var hinh = col["IMAGE"];
            var ngaycapnhat = String.Format(DateTime.Now.ToString("dd/MM/yyyy"), col["DATEUPDATE"]);
            var soluong = col["QUANTITIES"];
            ////var mamh = col["ID_CAT"];

            if (String.IsNullOrEmpty(tensp))
            {
                ViewData["Err1"] = "Vui lòng nhập tên sản phẩm";
                return View();
            }
            else if (String.IsNullOrEmpty(giaban))
            {
                ViewData["Err2"] = "Vui lòng nhập giá bán";
                return View();
            }
            //else if (String.IsNullOrEmpty(size))
            //{
            //    ViewData["Err3"] = "Vui lòng nhập size";
            //    return View();
            //}
            //else if (String.IsNullOrEmpty(mota))
            //{
            //    ViewData["Err4"] = "Vui lòng nhập tên sản phẩm";
            //}
            // hinh
            //else if (String.IsNullOrEmpty(ngaycapnhat))
            //{
            //    ViewData["Err1"] = "Vui lòng nhập tên sản phẩm";
            //}

            else if (up == null) // kiem tra duong dan hinh anh
            {
                ViewData["img"] = "Vui lòng chọn ảnh bìa";
                return View();
            }

            else if (String.IsNullOrEmpty(soluong))
            {
                ViewData["Err4"] = "Vui lòng nhập số lượng";
                return View();
            }
            ////else if (String.IsNullOrEmpty(mamh))
            ////{
            ////    ViewData["Err5"] = "Vui lòng nhập tên mặt hàng";
            ////    return View();
            ////}




            //  Kiểm tra đường dẫn hình ảnh
            ////if (up == null)
            ////{
            ////    ViewBag.Inform = "Vui lòng chọn ảnh bìa";
            ////    return View();
            ////}
            //them vao co so du lieu
            else
            {
                if(ModelState.IsValid)
                {
                    //luu ten file
                    var filename = Path.GetFileName(up.FileName);

                    //luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images/product"), filename);

                    //kiem tra hinh anh ton tai chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewData["Errimg"] = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //luu hinh anh vao duong dan
                        up.SaveAs(path);
                    }
                    pro.IMAGE = filename;
                    //luu vao csdl
                    db.PRODUCTs.InsertOnSubmit(pro);
                    db.SubmitChanges();
                }
                return RedirectToAction("Product");
            }

            //ViewBag.ID_CAT = new SelectList(db.CATEGORies.ToList().OrderBy(n => n.NAME), "ID_CAT", "NAME");
            //return RedirectToAction("Product");
        }


        public ActionResult Delete(int id)
        {
            PRODUCT pro = db.PRODUCTs.FirstOrDefault(x => x.ID_PRO == id);
            db.PRODUCTs.DeleteOnSubmit(pro);
            db.SubmitChanges();
            return RedirectToAction("Product");
        }


        //quản lý hoá đơn
        public ActionResult Order(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.ORDERs.ToList().OrderBy(n => n.ID_ORD).ToPagedList(pageNumber, pageSize));
        }


    }
}