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
    [Authorize]
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            var employees = EmployeeRepository
                                .Where(x => x.Birthdate.HasValue)
                                .Where(x => x.Birthdate.Value.Month == DateTime.Now.Month && x.Birthdate.Value.Day == DateTime.Now.Day)
                                .AsDto();

            var vacations = new List<string>();
            var repoVacations = VacationRepository
                                    .Where(x => x.StartDate.Month == DateTime.Now.Month || x.EndDate.Month == DateTime.Now.Month)
                                    .Where(x => x.Status == VacationStatus.Approved)
                                    .OrderBy(x => x.StartDate)
                                    .AsDto();

            foreach (var vacation in repoVacations)
            {
                var employee = EmployeeRepository.AsDto().Single(x => x.Id == vacation.EmployeeId).Name;
                vacations.Add(employee + " " + vacation.StartDate.ToString("d") + " - " + vacation.EndDate.ToString("d"));
            }

            return View(new IndexViewModel() { Employees = employees.ToList(), Vacations = vacations, CurrentUserRole = CurrentUserRole });
        }

        public ActionResult Workers()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Standard))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            return View(new WorkersViewModel() { Employees = EmployeeRepository.AsDto().Where(x => x.ContractEnd == null || x.ContractEnd > DateTime.Now).ToList(), CurrentUserRole = CurrentUserRole });
        }

        public ActionResult Requests()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Standard))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            return View(new BaseViewModel() { CurrentUserRole = CurrentUserRole });
        }

        public ActionResult Calendar()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Standard))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            return View(new BaseViewModel() { CurrentUserRole = CurrentUserRole });
        }
    }
}