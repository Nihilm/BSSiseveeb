using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BSSiseveeb.Core;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Public.Web.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace BSSiseveeb.Public.Web.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        public ActionResult Vacations()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Vacations))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            return View(new BaseViewModel() { CurrentUserRole = CurrentUserRole });
        }

        public ActionResult Requests()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Requests))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            return View(new GeneratePdfModel() { CurrentUserRole = CurrentUserRole });
        }

        public ActionResult EditEmployees()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Users))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            return View(new WorkersViewModel() {Employees = EmployeeRepository.AsDto().ToList(), CurrentUserRole = CurrentUserRole});
        }

        public ActionResult ViewEmployee(string id)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Users))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }

            var employee = EmployeeRepository.First(x => x.Id == id);
            var role = RoleRepository.First(x => x.Id == employee.RoleId);
            var roles = RoleRepository.Select(x => x.Name).ToList();

            var model = new RegistrationModel
            {
                Id = employee.Id,
                Start = employee.ContractStart,
                End = employee.ContractEnd,
                VacationDays = employee.VacationDays,
                OldRole = role.Name,
                NewRole = role.Name,
                Roles = roles,
                CurrentUserRole = CurrentUserRole
            };

            return View("EditEmployee", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmployee(RegistrationModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Users))
            {
                return View("Error");
            }

            var employee = EmployeeRepository.First(x => x.Id == model.Id);
            var role = RoleRepository.First(x => x.Name == model.NewRole);

            employee.VacationDays = model.VacationDays;
            employee.ContractEnd = model.End;
            employee.ContractStart = model.Start;
            employee.PhoneNumber = model.Phone;
            employee.Role = role;

            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();

            return View("EditEmployees", new WorkersViewModel() { Employees = EmployeeRepository.AsDto().ToList(), CurrentUserRole = CurrentUserRole });
        }

        [HttpPost]
        public ActionResult GeneratePdf(GeneratePdfModel model)
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Vacations))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            if(model.Start == new DateTime() || model.End == new DateTime())
            {
                return View("Requests", new GeneratePdfModel() { CurrentUserRole = CurrentUserRole });
            }

            var status = new RequestStatus();

            switch (model.Status)
            {
                case "Confirmed":
                    status = RequestStatus.Confirmed;
                    break;
                case "Declined":
                    status = RequestStatus.Declined;
                    break;
                default:
                    status = RequestStatus.Pending;
                    break;
            }

            var requests = RequestRepository.AsDto()
                .Where(x => x.TimeStamp < model.End && x.TimeStamp > model.Start && x.Status == status);
            var employees = EmployeeRepository.AsDto();
            var start = model.Start.ToString("d");
            var end = model.End.ToString("d");

            var document = new Document();
            document.Info.Title = $"{status} requests from {start} to {end}";

            var style = document.Styles["Normal"];
            style.Font.Name = "Verdana";
            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style = document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;
            style = document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);

            var section = document.AddSection();

            var paragraph = section.AddParagraph();
            paragraph.Style = "Reference";
            paragraph.AddFormattedText($"{status} requests from {start} to {end}", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddDateField("dd.MM.yyyy");

            var table = section.AddTable();
            table.Style = "Table";
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            var column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            var row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("Töötarve");
            row.Cells[1].AddParagraph("Lisa info");
            row.Cells[2].AddParagraph("Taotluse kuupäev");
            row.Cells[3].AddParagraph("Taotleja");

            table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

            foreach (var request in requests)
            {
                var itemrow = table.AddRow();
                var employee = employees.Single(x => x.Id == request.EmployeeId).Name;
                itemrow.TopPadding = 1.5;
                itemrow.Cells[0].AddParagraph(request.Req);
                itemrow.Cells[1].AddParagraph(request.Description);
                itemrow.Cells[2].AddParagraph(request.TimeStamp.ToString("d"));
                itemrow.Cells[3].AddParagraph(employee);
                table.SetEdge(0, table.Rows.Count - 1, 4, 1, Edge.Box, BorderStyle.Single, 0.75);
            }

            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "requests.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;

            renderer.RenderDocument();
            var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream, false);
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            stream.Close();
            Response.End();

            return View("Requests", new GeneratePdfModel() { CurrentUserRole = CurrentUserRole });
        }

        

        [HttpGet]
        public async Task<ActionResult> SyncUsers()
        {
            if (!CurrentUser.Role.Rights.HasFlag(AccessRights.Users))
            {
                return View("Error", new BaseViewModel() { CurrentUserRole = CurrentUserRole });
            }
            string tenantID = ClaimsPrincipal.Current.FindFirst(AppClaims.TenantId).Value;

            Uri servicePointUri = new Uri(graphResourceID);
            Uri serviceRoot = new Uri(servicePointUri, tenantID);
            ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                  async () => await GetTokenForApplication());

            var result = await activeDirectoryClient.Users.ExecuteAsync();
            IEnumerable<IUser> users = result.CurrentPage.ToList();

            var myId = CurrentUserId;
            var defaultRole = RoleRepository.Single(x => x.Name == "User");
            var currentEmployees = EmployeeRepository.ToList();

            var appUsers = users.Where(x => x.GivenName != null).Select(x => new Employee()
            {
                Id = x.ObjectId,
                Email = x.Mail,
                Role = defaultRole,
                Name = x.DisplayName,
                PhoneNumber = x.Mobile,
                IsInitialized = false,
                VacationMessages = false,
                RequestMessages = false,
                MonthlyBirthdayMessages = false,
                DailyBirthdayMessages = false,
                VacationDays = 28
            });

            foreach(var user in appUsers)
            {
                if (currentEmployees.Contains(user))
                {
                    var tempUser = currentEmployees.Single(x => x.Id == user.Id);
                    tempUser.Email = user.Email;
                    tempUser.Name = user.Name;
                    tempUser.PhoneNumber = user.PhoneNumber;
                    EmployeeRepository.SaveOrUpdate(tempUser);
                }
                else
                {
                    EmployeeRepository.AddIfNew(user);
                }
            }

            EmployeeRepository.Commit();

            return View("EditEmployees", new WorkersViewModel() { Employees = EmployeeRepository.AsDto().ToList(), CurrentUserRole = CurrentUserRole });
        }
    }
}