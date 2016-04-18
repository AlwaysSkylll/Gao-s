using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WeiXinStore.Models;

namespace WeiXinStore.DataAcessLayer
{
    public class SalesERPDAL: DbContext
    {
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<employee>().ToTable("TblEmployee");

        //}
        public SalesERPDAL()
            : base("name=TestString")
        {
        }
        public DbSet<employee> Employees { get; set; }
    }
    
}