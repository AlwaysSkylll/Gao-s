using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXinStore.Models;
using WeiXinStore.DataAcessLayer;

namespace WeiXinStore.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        public user Register(user u)
        {
            Store db = new Store();
            DateTime dt;
            DateTime dt2 = System.DateTime.Now;
            //string dtstring =System.DateTime.Now.ToString("yyyy-MM-dd hh：mm：ss");
            //dt = Convert.ToDateTime(dtstring);
            dt = DateTime.Parse(Convert.ToDateTime(dt2).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            u.CreateTime = dt;
            db.Userdata.Add(u);
            db.SaveChanges();

            return u;
        }

    }
}
