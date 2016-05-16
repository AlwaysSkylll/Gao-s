using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXinStore.Models;

namespace WeiXinStore.Controllers
{
    public class UserLogController : Controller
    {
        //
        // GET: /UserLog/

        public ActionResult Index()
        {
            return View();
        }
        public void LogQuit()
        {
            Session.Remove("UserName");
            Session.Remove("UserId");
        }
        public void LogValidation()
        {
            StoreEntities st = new StoreEntities();
            List<users> users = st.users.ToList();
            string phonenum = Request.Form["phonenum"];
            string password = Request.Form["password"];
            if (users.Count == 0)
            {
                Response.Write("0");
                Response.End();
            }
            else
            {
                foreach (users u in users)
                {
                    if (phonenum == u.Phone && password == u.Password)
                    {
                        var UserName = u.Name;
                        Session["UserName"] = UserName;
                        Session["UserId"] = u.UserId;
                        Response.Write("1");
                        Response.End();
                    }
                }
            }
            Response.Write("0");
            Response.End();
        }
    }
}
