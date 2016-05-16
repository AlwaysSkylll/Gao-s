using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXinStore.Models;

namespace WeiXinStore.Controllers
{
    public class UserRegisterController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        public string Register(users u)
        {
            StoreEntities db = new StoreEntities();
            DateTime dt;
            DateTime dt2 = System.DateTime.Now;
            dt = DateTime.Parse(Convert.ToDateTime(dt2).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            u.CreateTime = dt;
            db.users.Add(u);
            db.SaveChanges();
            return ("注册成功，请尝试登陆您的账号："+u.Name);
        }
        public void IsExist()
        {
            //querystring 不能判断值
            //string phone = Request.QueryString["phonenum"];
            var phone = Request.Form["phonenum"];
            StoreEntities st = new StoreEntities();
            List<users> users = st.users.ToList();
            if (users.Count==0)
            {
                Response.Write("1");
                Response.End();
            }
            else
            {
                foreach(users usr in users){
                    if (phone == usr.Phone)
                    {
                        Response.Write("0");
                        Response.End();
                        break;
                    }           
                 }
            }
            Response.Write("1");
            Response.End();         
        }
    }
}
