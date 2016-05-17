// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace BSSiseveeb.Public.Web.Controllers
{
    public partial class AdminController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AdminController() { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AdminController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ViewEmployee()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ViewEmployee);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult EditEmployee()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditEmployee);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult GeneratePdf()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GeneratePdf);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AdminController Actions { get { return MVC.Admin; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Vacations = "Vacations";
            public readonly string Requests = "Requests";
            public readonly string EditEmployees = "EditEmployees";
            public readonly string ViewEmployee = "ViewEmployee";
            public readonly string EditEmployee = "EditEmployee";
            public readonly string GeneratePdf = "GeneratePdf";
            public readonly string SyncUsers = "SyncUsers";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Vacations = "Vacations";
            public const string Requests = "Requests";
            public const string EditEmployees = "EditEmployees";
            public const string ViewEmployee = "ViewEmployee";
            public const string EditEmployee = "EditEmployee";
            public const string GeneratePdf = "GeneratePdf";
            public const string SyncUsers = "SyncUsers";
        }


        static readonly ActionParamsClass_ViewEmployee s_params_ViewEmployee = new ActionParamsClass_ViewEmployee();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ViewEmployee ViewEmployeeParams { get { return s_params_ViewEmployee; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ViewEmployee
        {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_EditEmployee s_params_EditEmployee = new ActionParamsClass_EditEmployee();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_EditEmployee EditEmployeeParams { get { return s_params_EditEmployee; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_EditEmployee
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_GeneratePdf s_params_GeneratePdf = new ActionParamsClass_GeneratePdf();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GeneratePdf GeneratePdfParams { get { return s_params_GeneratePdf; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GeneratePdf
        {
            public readonly string model = "model";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string EditEmployee = "EditEmployee";
                public readonly string EditEmployees = "EditEmployees";
                public readonly string Requests = "Requests";
                public readonly string Vacations = "Vacations";
            }
            public readonly string EditEmployee = "~/Views/Admin/EditEmployee.cshtml";
            public readonly string EditEmployees = "~/Views/Admin/EditEmployees.cshtml";
            public readonly string Requests = "~/Views/Admin/Requests.cshtml";
            public readonly string Vacations = "~/Views/Admin/Vacations.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AdminController : BSSiseveeb.Public.Web.Controllers.AdminController
    {
        public T4MVC_AdminController() : base(Dummy.Instance) { }

        [NonAction]
        partial void VacationsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Vacations()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Vacations);
            VacationsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void RequestsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Requests()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Requests);
            RequestsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void EditEmployeesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult EditEmployees()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditEmployees);
            EditEmployeesOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ViewEmployeeOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string id);

        [NonAction]
        public override System.Web.Mvc.ActionResult ViewEmployee(string id)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ViewEmployee);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            ViewEmployeeOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void EditEmployeeOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BSSiseveeb.Public.Web.Models.RegistrationModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult EditEmployee(BSSiseveeb.Public.Web.Models.RegistrationModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditEmployee);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            EditEmployeeOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void GeneratePdfOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BSSiseveeb.Public.Web.Models.GeneratePdfModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult GeneratePdf(BSSiseveeb.Public.Web.Models.GeneratePdfModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GeneratePdf);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            GeneratePdfOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void SyncUsersOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> SyncUsers()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SyncUsers);
            SyncUsersOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult(callInfo as System.Web.Mvc.ActionResult);
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
