using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinStore.DataAcessLayer;

namespace WeiXinStore.Models
{
    public class StoreBussiness
    {
        
        public List<instrument> GetInstrument()
        {
            Store st = new Store();
            return st.Instrumentdata.ToList();
        }
        public List<user> GetUser()
        {
            Store st = new Store();
            return st.Userdata.ToList();
        }
        
    }
}