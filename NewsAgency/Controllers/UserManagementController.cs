using NewsAgency.Infrustructure;
using NewsAgency.Models;
using NewsAgency.Repositories;
using NewsAgency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
namespace NewsAgency.Controllers
{
    [RestrictActionToRole(Roles = new string[] { "admin" })]
    [Authorize]
    public class UserManagementController : BaseController
    {
        public UserManagementController(INewsAgencyService _service, INewsRepository _repository)
        {
            service = _service;
            news_repository = _repository;
        }
        public ActionResult ManageUsers(int page = 1, string username = "")
        {
            ViewBag.Username = username;

            var model = service.GetUserList(page, news_repository, username);
            return View(model);
        }


        public ActionResult EditUser(string username)
        {

            var model = service.GetUserList(1, news_repository, username);
            if (model == null || model.Users == null || model.Users.Count() == 0)
            {
                return RedirectToAction("ManageUsers");
            }
            return View(model.Users.FirstOrDefault());
        }
        [HttpPost]
        public ActionResult EditUser(UserInfoModel model)
        {

            if (!ModelState.IsValid)
                return View(model);
            var result = service.EditUser(model.Username, model.Name, model.Family, model.Password, news_repository);
            if (result == false)
            {
                ViewBag.ErrorMessage = "ویرایش با مشکل مواجع شد";
                return View(model);
            }
            ViewBag.SuccessfulMessage = "ویرایش با موفقیت انجام شد";
            return View(model);
        }


        public ActionResult NewUser()
        {

            return View(new UserInfoModel());
        }

        [HttpPost]
        public ActionResult NewUser(UserInfoModel model)
        {


            if (!ModelState.IsValid)
                return View(model);

            var role_of_user = service.GetRoleOfUser(model.Username, news_repository);
            if (role_of_user != "unknown")
            {
                ViewBag.ErrorMessage = "نام کاربری تکراری است";
                return View(model);
            }

            var result = service.AddUser(model.Username, model.Password, model.Name, model.Family, model.Role, news_repository);
            if (result == -1)
            {
                ViewBag.ErrorMessage = "در ذخیره سازی کاربر مشکلی رخ داده است. لطفا مجددا تلاش نمایید";
                return View(model);
            }


            ViewBag.SuccessfulMessage = "کاربر با موفقیت افزوده شد";
            return View(model);

        }
    }
}