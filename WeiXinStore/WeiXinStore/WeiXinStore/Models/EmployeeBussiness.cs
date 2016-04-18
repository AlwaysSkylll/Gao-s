using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinStore.DataAcessLayer;

namespace WeiXinStore.Models
{
    public class EmployeeBussiness
    {
        public List<employee> GetEmployees() 
        {
            SalesERPDAL salesDal = new SalesERPDAL();
            return salesDal.Employees.ToList();
        }
    }
}