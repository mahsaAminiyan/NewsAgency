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
    [Authorize]
    public class NewsManagementController : BaseController
    {
        public NewsManagementController(INewsAgencyService _service, INewsRepository _repository)
        {
            service = _service;
            news_repository = _repository;
        }

        [RestrictActionToRole(Roles = new string[] { "reporter" })]

        public ActionResult SendNews()
        {

            ViewBag.VisibleSuccessAlert = "none";
            ViewBag.VisibleFailedAlert = "none";
            NewsModel model = new NewsModel();
            var categories = service.GetAllNewsCategories(news_repository);
            model.Categories = new SelectList(categories);
            return View(model);
        }


        [RestrictActionToRole(Roles = new string[] { "reporter" })]
        [HttpPost]
        public ActionResult SendNews(NewsModel model, HttpPostedFileBase NewsImage = null)
        {


            ViewBag.VisibleSuccessAlert = "none";
            ViewBag.VisibleFailedAlert = "none";

            var categories = service.GetAllNewsCategories(news_repository);
            model.Categories = new SelectList(categories);

            if (!ModelState.IsValid)
                return View(model);

            if (NewsImage == null)
            {
                ViewBag.VisibleFailedAlert = "block";
                ViewBag.ErrorMessage = "عکسی ارسال نشده است";
                return View(model);
            }
            if (!NewsImage.ContentType.ToLower().Contains("jpg") && !NewsImage.ContentType.ToLower().Contains("jpeg"))
            {
                ViewBag.VisibleFailedAlert = "block";
                ViewBag.ErrorMessage = "فرمت عکس باید jpg باشد";
                return View(model);
            }

            bool result = service.SaveNews(User.Identity.Name, model.Title, model.Summary, model.MainContent, model.Category, NewsImage, news_repository);
            if (result == true)
            {
                model.Title = "";
                model.MainContent = "";
                model.Summary = "";
                ViewBag.SuccessfulMessage = "خبر با موفقیت ثبت شد و منتظر تایید مدیر می باشد.";
                ViewBag.VisibleSuccessAlert = "block";
                return View(model);
            }
            ViewBag.VisibleFailedAlert = "block";
            ViewBag.ErrorMessage = "در ثبت خبر مشکلی رخ داده است. لطفا مجددا تلاش نمایید.";
            return View(model);
        }

        [RestrictActionToRole(Roles = new string[] { "reporter" })]
        public ActionResult ShowNewsWithStatus(int status, string username = "", int page = 1)
        {

            var model = service.GetNewsWithStatus(page, (NewsStatusEnum)status, username, news_repository);
            return View(model);
        }
        [RestrictActionToRole(Roles = new string[] { "admin" })]

        public ActionResult ConfirmNews(int page = 1)
        {

            var model = service.GetNewsWithStatus(page, NewsStatusEnum.Waiting, "", news_repository);
            return View(model);
        }

        [RestrictActionToRole(Roles = new string[] { "admin" })]
        public ActionResult ConfirmNewsWithId(int Id)
        {

            var result = service.SetNewsStatus(Id, NewsStatusEnum.Confirmed, news_repository);
            if (result == false)
            {

            }

            return RedirectToAction("ConfirmNews");
        }

        [RestrictActionToRole(Roles = new string[] { "admin" })]
        public ActionResult RejectNewsWithId(int Id)
        {

            var result = service.SetNewsStatus(Id, NewsStatusEnum.Rejected, news_repository);
            if (result == false)
            {

            }

            return RedirectToAction("ConfirmNews");
        }


        [RestrictActionToRole(Roles = new string[] { "admin" })]
        public ActionResult ShowNewsForConfirm(int Id)
        {

            var model = service.GetNewsWithId(Id, news_repository);
            return View(model);
        }

        [RestrictActionToRole(Roles = new string[] { "admin" })]

        public ActionResult ManageNews(int page = 1, string title = "")
        {
            ViewBag.title = title;

            var model = service.GetNewsList(page, news_repository, title);
            return View(model);
        }

        [RestrictActionToRole(Roles = new string[] { "admin" })]
        public ActionResult ChangeShowValueOfNewsWithId(int Id, string title = "")
        {
            var result = service.ChangeNewsShowValue(Id, news_repository);
            if (result == false)
            {

            }

            return RedirectToAction("ManageNews", new { page = 1, title = title });
        }
    }
}