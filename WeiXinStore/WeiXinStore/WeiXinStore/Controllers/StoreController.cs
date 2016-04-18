using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXinStore.ViewsModels;
using WeiXinStore.Models;
using WeiXinStore.DataAcessLayer;

namespace WeiXinStore.Controllers
{
    public class StoreController : Controller
    {
        //
        // GET: /Store/

        public ActionResult Index()
        {
            StoreBussiness store = new StoreBussiness();
            //store.GetInstrument();
            //store.GetUser();
            return View("Index");
        }
        public ActionResult About()
        {
            return View("About");
        }
        public ActionResult Cover()
        {
            return View("Cover");
        }


    }
}
