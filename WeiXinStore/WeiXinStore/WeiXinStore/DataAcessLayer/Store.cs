using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WeiXinStore.Models;

namespace WeiXinStore.DataAcessLayer
{
    public class Store:DbContext 
    {
        public Store():base("name=StoreString")
        {
        }
        public DbSet<instrument> Instrumentdata { get; set; }
        public DbSet<user> Userdata { get; set; }
    }
}