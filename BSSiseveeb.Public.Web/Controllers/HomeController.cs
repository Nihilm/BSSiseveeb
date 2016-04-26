using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Data.Repositories;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using Microsoft.AspNet.Identity.Owin;


namespace BSSiseveeb.Public.Web.Controllers
{
    public class HomeController : Controller
    {
        public IEmployeeRepository EmployeeRepository { get; set; }
        

        public ActionResult Index()
        {
            var employees = EmployeeRepository.Where(x => x.Birthdate.Month == DateTime.Now.Month && x.Birthdate.Day == DateTime.Now.Day).ToList();
            return View(new IndexViewModel() {Employees = employees });
        }

        [AuthorizeLevel(AccessRights.Level1)]
        public ActionResult Workers()
        {
            return View(new WorkersViewModel() {Employees = EmployeeRepository.ToList()});
        }

        [AuthorizeLevel(AccessRights.Level1)]
        public ActionResult Requests()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Level1)]
        public ActionResult Calendar()
        {
            return View();
        }

    }
}