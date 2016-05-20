using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BSSiseveeb.Public.Web.Controllers
{
    [Authorize]
    public partial class HomeController : BaseController
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!CurrentUser.IsInitialized)
                {
                    return View(MVC.Home.Views.Initial, new ChangeAccountSettingsViewModel { Phone = CurrentUser.PhoneNumber });
                }

                var employees = EmployeeRepository
                                    .Where(x => x.Birthdate.HasValue)
                                    .Where(x => x.Birthdate.Value.Month == DateTime.Now.Month && x.Birthdate.Value.Day == DateTime.Now.Day)
                                    .AsDto();

                var vacations = VacationRepository
                                        .Where(x => x.StartDate.Month == DateTime.Now.Month || x.EndDate.Month == DateTime.Now.Month)
                                        .Where(x => x.Status == VacationStatus.Approved)
                                        .OrderBy(x => x.StartDate)
                                        .Select(x => new EmployeeVacationModel()
                                        {
                                            EmployeeName = x.Employee.Name,
                                            StartDate = x.StartDate,
                                            EndDate = x.EndDate
                                        })
                                        .ToList();

                return View(new IndexViewModel() { Employees = employees, Vacations = vacations });
            }
            else
            {
                return View(MVC.Home.Views.Public);
            }
        }

        [AuthorizeLevel(AccessRights.Standard)]
        public virtual ActionResult Workers()
        {
            return View(new WorkersViewModel() { Employees = EmployeeRepository.Where(x => x.ContractEnd == null || x.ContractEnd > DateTime.Now).AsDto() });
        }

        [AuthorizeLevel(AccessRights.Standard)]
        public virtual ActionResult Requests()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Standard)]
        public virtual ActionResult Calendar()
        {
            return View();
        }
    }
}