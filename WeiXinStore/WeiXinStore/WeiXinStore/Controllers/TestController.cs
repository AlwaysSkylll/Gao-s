using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeiXinStore.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public string SayHi()
        {
            return "Hello world!";
        }
        public ActionResult GetView()
        {
            return View("Test");
        }

    }
}
