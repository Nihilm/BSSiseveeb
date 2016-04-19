using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Data.Repositories;

namespace BSSiseveeb.Public.Web.Controllers
{
    public class HomeController : Controller
    {
        public IRequestRepository RequestRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IVacationRepository VacationRepository { get; set; }

        

        public ActionResult Index()
        {


            return View();
        }

        public ActionResult Workers()
        {

            return View();
        }

        public ActionResult Requests()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View();
        }
    }
}