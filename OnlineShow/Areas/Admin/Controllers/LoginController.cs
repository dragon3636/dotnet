using Model.DAO;
using OnlineShow.Areas.Admin.Models;
using OnlineShow.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
namespace OnlineShow.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
          
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var result = dao.Login(Encroptor.MD5Hash(model.PassWord), model.UserName,0);
                
                if (result ==1)
                {
                    var user = dao.GetbyID(model.UserName);
                    var session = new UserLogin();
                    session.UserID = user.ID;
                    session.UserName = user.UserName;
                    session.GroupID = user.GroupID;
                    var listCredential = dao.GetListCredentail(model.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredential);
                    Session.Add(CommonConstants.USER_SESSION,session);
                    return RedirectToAction("Index","Home");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản  bị khóa. ");
                }
                else if( result ==-2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản ko được cấp quyền");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhâp thất bại.");

                }
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/admin/login");
        }
    }
}