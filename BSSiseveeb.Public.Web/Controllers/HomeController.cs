using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            var employees = EmployeeRepository.AsDto().Where(x => x.Birthdate.Month == DateTime.Now.Month && x.Birthdate.Day == DateTime.Now.Day).ToList();
            var vacations = new List<string>();
            var repoVacations = VacationRepository.AsDto()
                                    .Where(x => x.StartDate.Month == DateTime.Now.Month || x.EndDate.Month == DateTime.Now.Month)
                                    .Where(x => x.Status == VacationStatus.Approved)
                                    .OrderBy(x => x.StartDate).ToList();

            foreach (var vacation in repoVacations)
            {
                var employee = EmployeeRepository.AsDto().Single(x => x.Id == vacation.EmployeeId).Name;
                vacations.Add(employee + " " + vacation.StartDate.ToString("d") + " - " + vacation.EndDate.ToString("d"));
            }

            return View(new IndexViewModel() {Employees = employees, Vacations = vacations });
        }

        [AuthorizeLevel(AccessRights.Standard)]
        public ActionResult Workers()
        {
            return View(new WorkersViewModel() {Employees = EmployeeRepository.AsDto().Where(x => x.ContractEnd == null || x.ContractEnd > DateTime.Now).ToList()});
        }

        [AuthorizeLevel(AccessRights.Standard)]
        public ActionResult Requests()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Standard)]
        public ActionResult Calendar()
        {
            return View();
        }

    }
}