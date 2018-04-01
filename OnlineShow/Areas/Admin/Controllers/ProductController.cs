using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using OnlineShow.Common;
using Common;
namespace OnlineShow.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        [HasCredential(RoleID = "VIEW_PRODUCT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
            ViewBag.SearchString = searchString;
            var model = dao.ListProductPage(searchString, page, pageSize);
            SetViewBag();
            return View(model);
        }
        [HttpGet]
        [HasCredential(RoleID = "CREATE_PRODUCT")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [HasCredential(RoleID = "CREATE_PRODUCT")]

        public ActionResult Create(Product product )
        {
          
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                product.CreateBy = session.UserName;
                long id = dao.Insert(product);
                if (id > 0)
                {
                    SetAllExcuting("Thêm  sản phẩm  thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    SetAllExcuting("Thêm sản phẩm thất bại", "warning");
                    ModelState.AddModelError("", "Thêm sản phẩm thất bại");
                }
            }
            return View("Index");
        }
        [HasCredential(RoleID = "EDIT_PRODUCT")]
        public ActionResult Edit( long id)
        {
            var dao = new ProductDao();
            var model = dao.ViewDetail(id);
            SetViewBag();
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        [HasCredential(RoleID = "EDIT_PRODUCT")]

        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
              
                var result = dao.UpdateProduct(product);
                if (result)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin sản phẩm thành công ");
                }
            }
            return View("Index");
        }
        [HasCredential(RoleID = "DELETE_PRODUCT")]

        public ActionResult Delete(int id)
        {
            new ProductDao().DeleteProduct(id);
            return RedirectToAction("Index");
        }
        [HasCredential(RoleID = "EDIT_PRODUCT")]

        public JsonResult ChangStatus(long id)
        {
            var result = new ProductDao().ChangStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public void SetViewBag(long? selectId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.LisCategory(), "ID", "Name", selectId);
        }
    }
}