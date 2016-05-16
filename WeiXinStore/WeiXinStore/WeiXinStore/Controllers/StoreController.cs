using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using WeiXinStore.Models;
using System.Web.Script.Serialization;

namespace WeiXinStore.Controllers
{
    public class  StoreController : Controller
    {
        //
        // GET: /Store/
        public RedirectResult Oauth()
        {
            return Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx0860979218cc17a5&redirect_uri=http://1hh4914440.iask.in&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect");

        }
        //商城首页
        public ActionResult Index()
        {
            StoreEntities store = new StoreEntities();
            ProductListViewModel listview = new ProductListViewModel();
            listview.instrumentList = store.instruments.ToList();
            return View("Index", listview);
        }

        //关于页面
        public ActionResult About()
        {
            return View("About");
        }

        //封面页
        public ActionResult Cover()
        {
            return View("Cover");
        }

        //产品详情
        public ActionResult ProductDetail()
        {
            return View("ProductDetail");
        }

        //得到购物车内容
        public void GetCart()
        {
            //如果用户登录执行
            if (Session["UserId"] != null)
            {
                StoreEntities st = new StoreEntities();
                instruments instru = new instruments();
                shopcart cart = new shopcart();
                users us = new users();  
                List<CartItem> cilist = new List<CartItem>();
                ProductListViewModel listView = new ProductListViewModel();
                //获取当前用户的购物车
                int uId = (int)Session["UserId"];
                us = st.users.Find(uId);
                cart = st.shopcart.Find(us.CartId);
                string CartJsonString = cart.CartJson;
                List<Cartjson> cjList = new JavaScriptSerializer().Deserialize<List<Cartjson>>(CartJsonString);
                if (cart != null && CartJsonString != "[]")
                {
                    
                    foreach (Cartjson jj in cjList)
                    {
                        //获取购物车内所有商品对象并加入到JSON字符串中
                        CartItem ci = new CartItem();
                        instru = st.instruments.Find(jj.id);
                        ci.product = instru.InstrumentName;
                        ci.productId = instru.InstrumentId;
                        ci.num = jj.num;
                        ci.price = instru.Instrumentprice;
                        cilist.Add(ci);
                    }
                    cilist[0].totalPrice = cart.CountPrice;
                    Response.Write(new JavaScriptSerializer().Serialize(cilist));
                    Response.End();
                }        
            }
            Response.Write("0");
        }

        //添加购物车内容
        public void AddToCart()
        {
            
            DateTime dt;
            DateTime dt2 = System.DateTime.Now;          
            StoreEntities st = new StoreEntities();
            users us = new users();
            shopcart cart = new shopcart();
            instruments instru = new instruments();
            int itemId = int.Parse(Request.Form["cartItemId"]);
            dt = DateTime.Parse(Convert.ToDateTime(dt2).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            Cartjson cj = new Cartjson();
            cj.id=itemId;
            cj.num=1;
            instru = st.instruments.Find(itemId);
            CartItem ci= new CartItem();
            ci.productId = instru.InstrumentId;
            ci.product = instru.InstrumentName;
            ci.num = 1;
            ci.price = instru.Instrumentprice;     
            //用户未登录
            if (Session["UserId"] == null )
            {
                Response.Write("0");
            }
             //用户已登录
            else
            {
                int uId = (int)Session["UserId"];
                us = st.users.Find(uId);
                //用户已有购物车数据，修改
                if (us.CartId != null)
                {
                    int flag = 0;
                    cart = st.shopcart.Find(us.CartId);
                    string CartJsonString = cart.CartJson;
                    cart.CountPrice += ci.price;
                    ci.totalPrice = cart.CountPrice;
                    var ciJson = new JavaScriptSerializer().Serialize(ci);
                    //数据库已有的json对象
                    List<Cartjson> cjlist = new JavaScriptSerializer().Deserialize<List<Cartjson>>(CartJsonString);
                    foreach (Cartjson jj in cjlist)
                    {
                        if (ci.productId == jj.id)
                        {
                            jj.num++;
                            cart.CartJson = new JavaScriptSerializer().Serialize(cjlist);
                            Response.Write(ci.productId + "|" + cart.CountPrice);
                            flag++;
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        //若没有该产品追加一个json数组
                        cjlist.Add(cj);
                        cart.CartJson = new JavaScriptSerializer().Serialize(cjlist);
                        Response.Write(ciJson);
                    }
                    cart.CountNum++;
                    st.SaveChanges();
                }
                //若用户还没有购物车数据行，新增一条
                else
                {
                    ci.totalPrice = instru.Instrumentprice;
                    var ciJson = new JavaScriptSerializer().Serialize(ci);
                    List<Cartjson> cjlist = new List<Cartjson>();
                    cjlist.Add(cj);
                    cart.UserId = uId;
                    cart.CartJson = new JavaScriptSerializer().Serialize(cjlist);
                    cart.CountNum = 1;
                    cart.CountPrice = instru.Instrumentprice;
                    cart.CreateTime = dt;
                    cart.CartId = DateTime.Now.Year * (new Random().Next(99)) + (new Random().Next(31)) * DateTime.Now.Month + (new Random().Next(61)) * DateTime.Now.Day * DateTime.Now.Millisecond;
                    st.shopcart.Add(cart);
                    us.CartId = cart.CartId;
                    st.users.Find(uId).CartId = cart.CartId;
                    st.SaveChanges();
                    Response.Write(ciJson);
                }
            }
        }
        
        //移除购物车内容
        public void RemoveCartItem() {
            shopcart cart = new shopcart();
            StoreEntities st = new StoreEntities();
            users us = new users();
            instruments instru = new instruments();
            int itemId = int.Parse(Request.Form["cartItemId"]);
            int uId = (int)Session["UserId"];
            instru = st.instruments.Find(itemId);
            us = st.users.Find(uId);
            cart = st.shopcart.Find(us.CartId);
            string CartJsonString = cart.CartJson;
            double? PriceChange = 0;
            List<Cartjson> cjlist = new JavaScriptSerializer().Deserialize<List<Cartjson>>(CartJsonString);
            foreach(Cartjson jj in cjlist)
            {
                if (jj.id == itemId)
                {
                    var num = jj.num;
                    PriceChange=num*instru.Instrumentprice;
                    cart.CountPrice-=PriceChange;
                    cart.CountNum -= num;
                    cjlist.Remove(jj);
                    if (cart.CountNum == 0)
                        cart.CountPrice = 0;
                    cart.CartJson = new JavaScriptSerializer().Serialize(cjlist);
                    Response.Write(cart.CountPrice+"|"+cart.CountNum);
                    st.SaveChanges();
                    break;
                }
            }
        }
    }
}
