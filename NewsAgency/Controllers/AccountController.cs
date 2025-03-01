using NewsAgency.Infrustructure;
using NewsAgency.Models;
using NewsAgency.Repositories;
using NewsAgency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace NewsAgency.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController(INewsAgencyService _service, INewsRepository _repository)
        {
            service = _service;
            news_repository = _repository;
        }
        // GET: Account

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginInformation model)
        {

            if (!ModelState.IsValid)
                return View(model);
            bool result = service.Authenticate(model.UserName, model.Password, news_repository);
            if (result == true)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                Session["UserRole"] = service.GetRoleOfUser(model.UserName, news_repository);
                string actionName = GetActionToGoFromLoginPage(model.UserName);
                return RedirectToAction(actionName);
            }

            ViewBag.ErrorMessage = "نام کاربری یا رمز عبور اشتباه است";
            return View(model);

        }


        private string GetActionToGoFromLoginPage(string username)
        {

            string role = service.GetRoleOfUser(username, news_repository);

            switch (role.ToLower())
            {
                case "reporter":
                    return "ProfilePage";
                case "admin":
                    return "AdminPage";
                default:
                    return "ErrorPage";
            }
        }

        [RestrictActionToRole(Roles = new string[] { "reporter" })]
        public ActionResult ProfilePage()
        {

            ProfileViewModel model = new ProfileViewModel();
            model = service.GetProfileData(User.Identity.Name, news_repository);
            return View(model);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ChangePassword()
        {

            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordInformation model)
        {


            if (!ModelState.IsValid)
                return View(model);
            bool result = service.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword, news_repository);
            if (result == true)
            {
                ViewBag.SuccessfulMessage = "رمز با موفقیت تغییر کرد";
                return View(model);
            }

            ViewBag.ErrorMessage = "رمز قبلی اشتباه است";
            return View(model);
        }

        [RestrictActionToRole(Roles = new string[] { "admin" })]

        public ActionResult AdminPage()
        {


            AdminPanelViewModel model = new AdminPanelViewModel();
            model = service.GetAdminProfileData(User.Identity.Name, news_repository);
            return View(model);
        }
    }
}