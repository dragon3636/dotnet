using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;

namespace OnlineShow.Controllers
{
    public class ProductController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            ViewBag.AllProducts = new ProductDao().ListAllProduct();
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }
        public JsonResult ListName(string term)
        {
            var data = new ProductDao().ListName(term);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
         public ActionResult Category(long cateid,int page =1,int pageSize =3)
        {
            ViewBag.Category = new ProductCategoryDao().ViewDetail(cateid);
            var totalRecord = 0;
            var model = new ProductDao().ListByCategoryid(cateid,ref totalRecord,page,pageSize);
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

                int maxPage = 5;
                int totalPage = 0;
            totalPage =(int)Math.Ceiling((double) totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.Fist = 1;

            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }
        public ActionResult Search(string keyword, int page = 1, int pageSize =3)
        {
  
            var totalRecord = 0;
            var model = new ProductDao().Search(keyword, ref totalRecord, page, pageSize);
            ViewBag.Keyword = keyword;
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;
            totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.Fist = 1;

            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }
        public ActionResult Detail(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            ViewBag.RelatedProducts = new ProductDao().ListRelateProducts(id);
            ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryId.Value);
            return View(product);
        }

    }
}