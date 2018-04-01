using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using OnlineShow.Common;
using Common;

namespace OnlineShow.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString,int page =1,int pageSize =10 )
        {
            var dao = new UserDAO();
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.SearchString = searchString;
          
            if (session.GroupID == CommonConstants.ADMIN_GROUP)
            {
                var  model = dao.ListUserPage(searchString, page, pageSize,true);
                return View(model);
            }
            else
            {
                var model = dao.ListUserPage(searchString, page, pageSize);
                return View(model);
            }
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            SetViewBag(true);
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user )
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                // mã hóa   password
                var encryptorME5 = Encroptor.MD5Hash(user.Password);
                user.Password = encryptorME5;

                long id = dao.Insert(user);
                if (id > 0)
                {
                    SetAllExcuting("Thêm  user  thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAllExcuting("Thêm  user không  thành công", "warning");
                    ModelState.AddModelError("", "Thêm  user không thành công ");
                }
            }
            return View("Index");

        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit (int id)
        {
           
            SetViewBag(true);
            var user = new UserDAO().viewDetail(id);
            return View(user);
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                var dao = new UserDAO();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    // mã hóa   password
                    var encryptorME5 = Encroptor.MD5Hash(user.Password);
                    user.Password = encryptorME5;
                }
                user.ModifiedBy = session.UserName;
                var result = dao.UpdateUser(user);
                if (result)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user thành công ");
                }
            }
            return View("Index");

        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new UserDAO().DeleteUser(id);
            return RedirectToAction("Index");
        }
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangStatus(long id)
        {
            var result = new UserDAO().ChangStatus(id);
            return Json(new
            {
                status = result
            });
        }
         public void SetViewBag(bool status)
        {
            var dao = new GroupUserDao();
            ViewBag.GroupID = new SelectList(dao.ListAllGroup(), "ID", "Name");
        }
    }
}