using System;
using System.Collections.Generic;
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
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            var employees = EmployeeRepository.Where(x => x.Birthdate.Month == DateTime.Now.Month && x.Birthdate.Day == DateTime.Now.Day).ToList();
            var vacations = new List<string>();
            var repoVacations = VacationRepository
                                    .Where(x => x.StartDate.Month == DateTime.Now.Month || x.EndDate.Month == DateTime.Now.Month)
                                    .Where(x => x.Status == VacationStatus.Approved)
                                    .OrderBy(x => x.StartDate).ToList();

            foreach (var vacation in repoVacations)
            {
                var employee = EmployeeRepository.Single(x => x.Id == vacation.EmployeeId).Name;
                vacations.Add(employee + " " + vacation.StartDate.ToString("d") + " - " + vacation.EndDate.ToString("d"));
            }

            return View(new IndexViewModel() {Employees = employees, Vacations = vacations });
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