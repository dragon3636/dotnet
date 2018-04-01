using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OderDetailDao
    {
        private OnlineShopDbContext db = null;
        public OderDetailDao()
        {
            db = new OnlineShopDbContext();
        }
        public  bool Insert(OderDetail  details)
        {
            try
            {
                    db.OderDetail.Add(details);
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
