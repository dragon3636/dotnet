using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace OnlineShow.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index( string searchString,int page=1,int pageSize =10)

        {
            var dao = new CategoryDAO();
            var model = dao.ListAllPaging(searchString,page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category entity)
        {
            
          
            if (ModelState.IsValid)
            {
                var dao = new CategoryDAO();
                entity.CreateDate = DateTime.Now;
                var total = 0;
                total = dao.TotalCategory();
                entity.DisplayOrder = total ++;
                var CurrentCulture = Session[CommonConstants.CurrentCulture];
                entity.Language = CurrentCulture.ToString();
                long id = dao.InsertCategory(entity);
                if (id > 0)
                {
                    SetAllExcuting(StaticResource.Resources.InsertCategorySuccess, "success");
                    return RedirectToAction("Index", "Category");

                }
                else
                {
                    SetAllExcuting(StaticResource.Resources.InsertCategoryFailed, "warning");
                    ModelState.AddModelError("", "Thêm  category thất bại ");
                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var category = new CategoryDAO().viewDetail(id);
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(Category entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryDAO();
                var result = dao.Update(entity);
                if (result)
                {
                 return    RedirectToAction("Index", "Category");
                }
                else
                {
                    ModelState.AddModelError("", " Câp nhật   category thất bại ");
                }
            }
            return View("Index");
        }
         [HttpDelete]
         public ActionResult Delete(int id)
        {
            new CategoryDAO().DeleteCategory(id);
            return RedirectToAction("Index");
        }
    }
}