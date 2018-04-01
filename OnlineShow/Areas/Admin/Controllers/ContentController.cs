using Model.DAO;
using Model.EF;
using OnlineShow.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
namespace OnlineShow.Areas.Admin.Controllers
{
    public class ContentController :BaseController
    {
        // GET: Admin/Content
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpGet]
        public ActionResult Edit( long id)
        {
            var dao = new ContentDao();
            var content = dao.GetById(id);
            SetViewBag(content.categoryId);
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Content model)
        {
            if (ModelState.IsValid)
            {

            }
            SetViewBag(model.categoryId);
            return View();
        }
        [HttpPost]

        public ActionResult Create(Content model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreateBy = session.UserName;
                new ContentDao().Create(model);
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View();
        }
        public  void SetViewBag( long? selectId = null )
        {
            var dao = new CategoryDAO();
            ViewBag.categoryId = new SelectList(dao.ListCategory(),"ID", "Name",selectId);
        }
    }
}