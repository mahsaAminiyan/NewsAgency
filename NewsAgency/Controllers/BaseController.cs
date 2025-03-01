using NewsAgency.Repositories;
using NewsAgency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsAgency.Controllers
{
    public class BaseController : Controller
    {
        protected INewsAgencyService service;
        protected INewsRepository news_repository;
        protected string GetUserRole(string username)
        {
            string role = service.GetRoleOfUser(username, news_repository);
            return role;
        }

        protected string GetUserPanel(string role)
        {
            if (role == "reporter")
                return "ReporterPanel";
            else if (role == "admin")
                return "AdminPanel";

            return "UserPanel";
        }

        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            // your code here
            var role = GetUserRole(User.Identity.Name);
            ViewBag.PanelName = GetUserPanel(role);
        }
    }
}