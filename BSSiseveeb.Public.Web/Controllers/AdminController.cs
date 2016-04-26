﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSSiseveeb.Core.Contracts.Repositories;
using BSSiseveeb.Core.Domain;
using BSSiseveeb.Public.Web.Attributes;
using BSSiseveeb.Public.Web.Models;

namespace BSSiseveeb.Public.Web.Controllers
{
    public class AdminController : Controller
    {
        [AuthorizeLevel(AccessRights.Level5)]
        public ActionResult Index()
        {
            return View();
        }

    }
}