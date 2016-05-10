using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Core.Mappers;
using BSSiseveeb.Data;
using BSSiseveeb.Private.Web.Attributes;
using BSSiseveeb.Private.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace BSSiseveeb.Private.Web.Controllers
{
    public class AdminController : BaseController
    {
        private PasswordHasher _hasher;

        [AuthorizeLevel(AccessRights.Vacations)]
        public ActionResult Vacations()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Requests)]
        public ActionResult Requests()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Users)]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [AuthorizeLevel(AccessRights.Users)]
        public ActionResult EditEmployees()
        {
            return View(new WorkersViewModel() {Employees = EmployeeRepository.AsDto().ToList()});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeApi(AccessRights.Users)]
        public ActionResult SetEmployee(RegistrationModel model)
        {
            _hasher = new PasswordHasher();
            var roleId = RoleManager.Roles.Single(x => x.Name == "User").Id;
            var latestId = EmployeeRepository.AsDto().Max(x => x.Id);
            var employee = new Employee
            {
                Name = model.Name,
                Birthdate = model.BirthDay,
                ContractEnd = model.End,
                ContractStart = model.Start,
                PhoneNumber = model.Phone,
                VacationDays = model.VacationDays,
                Email = model.Email,
                Id = latestId + 1
            };

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                EmployeeId = employee.Id,
                AccessFailedCount = 0,
                PasswordHash = _hasher.HashPassword(model.Password),
                RoleId = roleId,
                Messages = "no",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Employee = employee
            };

            UserManager.Create(user);
            HttpContext.GetOwinContext().Get<BSContext>().SaveChanges();
            UserManager.AddToRole(user.Id, "user");

            return View("EditEmployees", new WorkersViewModel() { Employees = EmployeeRepository.AsDto().ToList() });
        }

        [AuthorizeApi(AccessRights.Users)]
        public ActionResult ViewEmployee(int id)
        {
            var employee = EmployeeRepository.AsDto().First(x => x.Id == id);
            var user = UserManager.FindByEmail(employee.Email);
            var role = RoleManager.FindById(user.RoleId);
            var roles = RoleManager.Roles.Select(x => x.Name).ToList();

            var model = new RegistrationModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Start = employee.ContractStart,
                End = employee.ContractEnd,
                Phone = employee.PhoneNumber,
                VacationDays = employee.VacationDays,
                Username = user.UserName,
                OldRole = role.Name,
                NewRole = role.Name,
                Roles = roles
            };

            return View("EditEmployee", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeApi(AccessRights.Users)]
        public ActionResult EditEmployee(RegistrationModel model)
        {
            var employee = EmployeeRepository.First(x => x.Id == model.Id);
            var roleId = RoleManager.Roles.Single(x => x.Name == model.NewRole).Id;

            employee.VacationDays = model.VacationDays;
            employee.ContractEnd = model.End;
            employee.ContractStart = model.Start;
            employee.Email = model.Email;
            employee.PhoneNumber = model.Phone;
            employee.Name = model.Name;
            employee.Account.Email = model.Email;
            employee.Account.UserName = model.Username;
            employee.Account.RoleId = roleId;


            EmployeeRepository.SaveOrUpdate(employee);
            EmployeeRepository.Commit();

            UserManager.Update(employee.Account);
            HttpContext.GetOwinContext().Get<BSContext>().SaveChanges();

            if (model.NewRole != model.OldRole)
            {
                UserManager.RemoveFromRole(employee.Account.Id, model.OldRole);
                UserManager.AddToRole(employee.Account.Id, model.NewRole);
            }
            
            return View("EditEmployees", new WorkersViewModel() { Employees = EmployeeRepository.AsDto().ToList() });
        }

        [AuthorizeLevel(AccessRights.Requests)]
        [HttpPost]
        public void GeneratePdf(GeneratePdfModel model)
        {
            var status = new RequestStatus();

            switch (model.Status)
            {
                case "Pending":
                    status = RequestStatus.Pending;
                    break;
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
        }
    }
}