using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using WeiXinStore.Models;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;
using Senparc.Weixin.MP;
namespace WeiXinStore.Controllers
{
    public class  StoreController : Controller
    {
        
        // GET: /Store/
        //获取微信授权code
        public void GetCode()
        {
            string appid = "wx0860979218cc17a5";
            string redirect_url = "http://1hh4914440.iask.in/Store/Cover";
            string scope = "snsapi_userinfo";
            string state = "Yungao";
            Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=" + redirect_url + "&response_type=code&scope=" + scope + "&state=" + state + "#wechat_redirect");
        }
        //根据code获取access_token，获取用户信息
        public void CodeToToken()
        {
            string code =Request.Form["code"];
            string appid = "wx0860979218cc17a5";
            string appsecret = "d4624c36b6795d1d99dcf0547af5443d";
            string url="https://api.weixin.qq.com/sns/oauth2/access_token?appid="+appid+"&secret="+appsecret+"&code="+code+"&grant_type=authorization_code ";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream postData = response.GetResponseStream();
            StreamReader respoStreamReader = new StreamReader(postData, Encoding.Default);
            string alldata = respoStreamReader.ReadToEnd();
            accessToken acess = new JavaScriptSerializer().Deserialize<accessToken>(alldata);
            respoStreamReader.Close();
            respoStreamReader.Dispose();
            postData.Close();
            string token = acess.access_token;
            string openid = acess.openid;
            if (token != "" && openid != ""){
                StoreEntities store = new StoreEntities();
                users user = new users();
                int mark = 0;
                DateTime dt;
                DateTime dt2 = System.DateTime.Now;
                dt = DateTime.Parse(Convert.ToDateTime(dt2).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + token + "&openid=" + openid + "&lang=zh_CN ";
                request = (HttpWebRequest)WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                postData = response.GetResponseStream();
                respoStreamReader = new StreamReader(postData, Encoding.UTF8);
                alldata = respoStreamReader.ReadToEnd();
                wxUserInfo userInfo = new JavaScriptSerializer().Deserialize<wxUserInfo>(alldata);
                foreach(users u in store.users) {
                    if (u.OpenId == userInfo.openid)
                    {
                        Session["UserName"] = u.Name;
                        Session["UserId"] = u.UserId;
                        u.headimgurl = userInfo.headimgurl;
                        //store.SaveChanges();
                        mark = 1;
                    }
                    if (mark == 1)
                        break;
                }
                if (mark == 0)
                {
                    Session["UserName"] = userInfo.nickname;
                    user.CreateTime = dt;
                    user.Name = userInfo.nickname;
                    user.Sex = userInfo.sex;
                    user.headimgurl = userInfo.headimgurl;
                    user.OpenId = userInfo.openid;
                    store.users.Add(user);
                    Response.Write(alldata);
                    Response.End();
                }
                store.SaveChanges();
            }
            
        }

       // public bool CheckSignature()
       //{
       //     string signature = Request.Form["signature"].ToString();
       //     string timestamp = Request.Form["timestamp"].ToString();
       //     string nonce = Request.Form["nonce"].ToString();
       //     string[] ArrTmp = { "sadsadsad", timestamp, nonce };
       //     Array.Sort(ArrTmp);　　 //字典排序  
       //     string tmpStr = string.Join("", ArrTmp);
       //     //tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
       //     //tmpStr = tmpStr.ToLower();
       //     byte[] pwBytes = Encoding.UTF8.GetBytes(tmpStr);
       //     byte[] hash = SHA1.Create().ComputeHash(pwBytes);
       //     StringBuilder hex = new StringBuilder();
       //     for (int i = 0; i < hash.Length; i++) hex.Append(hash[i].ToString("X2"));
       //     if (hex.ToString() == signature)
       //     {
       //         return true;
       //     }
       //     else
       //     {
       //         return false;
       //     }
       // }

        //商城首页
        public ActionResult Index()
        {
            ModelState.Clear();
            //微信用户登录状态
            if (Session["UserName"] != null && Session["UserId"] == null)
            {
                StoreEntities st = new StoreEntities();
                List<users> users = st.users.ToList();
                int mark = 0;
                foreach (users u in users)
                {
                    if (u.Name == Session["UserName"].ToString())
                    {
                        Session["UserId"] = u.UserId;
                        mark++;
                    }
                    if (mark == 1)
                        break;
                }
            }
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
            
            return View("");
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
                if(us.CartId!=null)
                {
                    string CartJsonString = cart.CartJson;
                    List<Cartjson> cjList = new JavaScriptSerializer().Deserialize<List<Cartjson>>(CartJsonString);
                    if (cart.CartJson != null)
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
                            ci.goodImg = instru.ProductImg;
                            cilist.Add(ci);
                        }
                        cilist[0].totalPrice = cart.CountPrice;
                        Response.Write(new JavaScriptSerializer().Serialize(cilist));
                        Response.End();
                    }
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
                    { 
                        cart.CountPrice = 0;
                        cart.CartJson = null;
                    }
                    else
                    cart.CartJson = new JavaScriptSerializer().Serialize(cjlist);
                    Response.Write(cart.CountPrice+"|"+cart.CountNum);
                    st.SaveChanges();
                    break;
                }
            }
        }

        //订单页
        public ActionResult Order()
        {
            return View("");
        }

        //个人中心
        public ActionResult UserCenter() {
            StoreEntities st = new StoreEntities();
            users user = st.users.Find((int)Session["UserId"]);
            return View("",user);
        }
        //新建地址
        public void NewAddress(address addr) {
            StoreEntities st = new StoreEntities();
            addr.UserId = (int)Session["UserId"];
            st.address.Add(addr);
            st.SaveChanges();
        }
        //获取地址
        public JsonResult GetAddress() {
            StoreEntities st = new StoreEntities();
            List<address> addrs = new List<address>();
            address addr = new address();
            foreach (address address in st.address)
            {
                if ((int)Session["UserId"] == address.UserId)
                    addrs.Add(address);
            }
            return Json(addrs,JsonRequestBehavior.AllowGet);
        }
        //地址管理页面
        public ActionResult AddressManage() {
            return View("");
        }
        //传递地址到订单页面
        public void GoToPay() {
            int addrid = int.Parse(Request.Form["addrid"]);
            Session["addrid"] = addrid;
        }
        //确认支付
        public ActionResult WxPayConfirm() {
            int userid=(int)Session["UserId"];
            StoreEntities st = new StoreEntities();
            shopcart cart = new shopcart();
            users user = new users();
            order ord = new order();
            int addrId = (int)Session["addrid"];
            DateTime dt2 = System.DateTime.Now;       
            user = st.users.Find(userid);
            cart = st.shopcart.Find(user.CartId);
            if (user.CartId == null)
            {
                return View("Index");
            }
            else 
            { 
                ord.OrderId = DateTime.Now.Year * (new Random().Next(99)) + (new Random().Next(31)) * DateTime.Now.Month + (new Random().Next(61)) * DateTime.Now.Day * DateTime.Now.Millisecond;
                ord.OrderPrice = cart.CountPrice;
                ord.OrderStatus = 0;
                ord.CartId = cart.CartId;
                ord.UserId = cart.UserId;
                ord.AddressId = addrId;
                ord.CreateTime=DateTime.Parse(Convert.ToDateTime(dt2).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                user.CartId = null;
                st.order.Add(ord);
                st.SaveChanges();
                return View("", ord);
            }
        }
        //删除指定id地址信息
        public void DeleteAddress() { 
            int addrId=int.Parse(Request.Form["addrid"]);
            StoreEntities st = new StoreEntities();
            address addr = new address();
            int userId=(int)Session["UserId"];
            addr = st.address.Where(a=>a.UserId==userId).First(a=>a.AddressId==addrId);
            st.address.Remove(addr);
            st.SaveChanges();
            Response.Write(addrId);
        }
    }
}
