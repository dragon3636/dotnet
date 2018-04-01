using BotDetect.Web.Mvc;
using Model.DAO;
using Model.EF;
using OnlineShow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShow.Common;
using Facebook;
using System.Configuration;
using System.Xml.Linq;
using Common;
namespace OnlineShow.Controllers

{
    public class UserController : Controller
    {

        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()

        {
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get  the  user 's  information ,like  email,
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middle_name = me.middle_name;
                string last_name = me.last_name;
                var user = new User();
                user.UserName = userName;
                user.Email = email;
                user.GroupID = CommonConstants.MEMBER_GROUP;
                user.Status = true;
                user.Name = firstname + " " + middle_name + " " + last_name;
                user.CreateDate = DateTime.Now;
                var resultInsert = new UserDAO().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var session = new UserLogin();
                    session.UserID = user.ID;
                    session.UserName = user.UserName;
                    session.GroupID = user.GroupID;
                    Session.Add(CommonConstants.USER_SESSION, session);

                }
            }
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var result = dao.Login(Encroptor.MD5Hash(model.Password), model.UserName);
                if (result == 1)
                {
                    var user = dao.GetbyID(model.UserName);
                    var session = new UserLogin();
                    session.UserID = user.ID;
                    session.UserName = user.UserName;
                    session.GroupID = user.GroupID;
                    Session.Add(CommonConstants.USER_SESSION, session);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản  bị khóa. ");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhâp thất bại.");

                }
            }
            return View(model);
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapchar", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");

                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Password = Encroptor.MD5Hash(model.Password);
                    user.GroupID = CommonConstants.MEMBER_GROUP;
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    if (!string.IsNullOrEmpty(model.ProvinceID))
                    {
                        user.ProvinceID = int.Parse(model.ProvinceID) ;
                    }
                    else if (!string.IsNullOrEmpty(model.DistrictID))
                    {
                        user.DistrictID = int.Parse(model.DistrictID);
                    }
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreateDate = DateTime.Now;
                    // trạng thái kích hoạt ->email xác nhận đăng ký
                    user.Status = true; // = true khi  vào email kcíh hoạt thư xác nhận

                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            return View(model);
        }
        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~\assets\client\data\Provinces_Data.xml"));
            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }

        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~\assets\client\data\Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item")
                .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = provinceID;

                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
    }
}