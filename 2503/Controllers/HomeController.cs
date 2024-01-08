using _2503.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2503.Controllers
{
    public class HomeController : Controller
    {
        dbSQL_2503DataContext db = new dbSQL_2503DataContext();


        private List<PRODUCT> GetPRODUCT(int count)
        {
            return db.PRODUCTs.OrderBy(a => a.ID_PRO).Take(count).ToList();
        }

        // GET: Home
        public ActionResult Index()
        {
            var pro = GetPRODUCT(10);
            return View(pro);
        }

        public ActionResult Categories()
        {
            var cate = from s in db.CATEGORies select s;
            return PartialView(cate);
        }


        public ActionResult Product_Categories(int id)
        {
            var prc = from s in db.PRODUCTs where s.ID_CAT == id select s;
            return View(prc);
        }

        public ActionResult Product_Details(int id)
        {
            var prd = from s in db.PRODUCTs where s.ID_PRO == id select s;
            return View(prd.Single());
        }



    }
}