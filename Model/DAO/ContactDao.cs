using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
   public class ContactDao
    {
        private OnlineShopDbContext db = null;
        public ContactDao()
        {
            db = new OnlineShopDbContext();
        }
        public Contact GetActionContact()
        {
            return  db.Contact.Single(x => x.Status == true);

        }
    }
}
