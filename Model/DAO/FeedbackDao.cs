using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public class FeedbackDao
    {
        private OnlineShopDbContext db = null;
        public FeedbackDao()
        {
            db = new OnlineShopDbContext();
        }
        /// <summary>
        ///  Insert  element Feeback on database
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        public bool Insert(Feedback fb)
        {
            try
            {
                db.Feedback.Add(fb);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
