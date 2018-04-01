using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
namespace OnlineShow.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDao().GetActionContact();

            return View(model);
        }
        [HttpPost]
        public JsonResult Send(string name, string mobile, string address, string email, string content)
        {
            var feedback = new Feedback();
            feedback.Name = name;
            feedback.Phone = mobile;
            feedback.Address = address;
            feedback.Email = email;
            feedback.Content = content;
            feedback.CreateDate = DateTime.Now;
            var result = new FeedbackDao().Insert(feedback);
            if (result)
            {
                return Json(new
                {
                    status = true
                });
                // gửi email   phản hồi tới  admin
                // gữi  thư  xác nhận  đã gửi email cho khách  hàng  
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }
    }
}