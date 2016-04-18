using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiXinStore.Models;
using WeiXinStore.ViewsModels;
namespace WeiXinStore.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Test/

        public string SayHi()
        {
            return "Hello world!";
        }
        public ActionResult Index()
        {
            ListViewModel empList = new ListViewModel();
            EmployeeBussiness empBal = new EmployeeBussiness();
            List<employee> employees = empBal.GetEmployees();
            List<EmployeeViewModel> empViewModels = new List<EmployeeViewModel>();
            foreach (employee emp in employees)
            {
                EmployeeViewModel empViewModel=new EmployeeViewModel();
                empViewModel.EmployeeName = emp.FirstName + "" + emp.LastName;
                empViewModel.Salary = emp.Salary.ToString("");
                if (emp.Salary > 15000)
                {
                    empViewModel.SalaryColor = "Yellow";
                }
                else 
                {
                    empViewModel.SalaryColor = "Green";
                }
                empViewModels.Add(empViewModel);
            }
            empList.Employees = empViewModels;
            return View("Index",empList);
        }
        
    }
}
