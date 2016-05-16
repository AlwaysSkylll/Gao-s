using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXinStore.Models;

namespace WeiXinStore.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetProduct()
        {
            StoreEntities st = new StoreEntities();
            List<instruments> instrus = st.instruments.ToList();
            string instruId = Request.Form["itemId"];
            instruments product = new instruments();
            foreach (instruments instru in instrus)
            {
                if (instru.InstrumentId.ToString() == instruId && instru.InstrumentNum>0)
                {
                    product = instru;
                    return Json(product);
                }
            }
            return Json("0");
        }
    }
}
