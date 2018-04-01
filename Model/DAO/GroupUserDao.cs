using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
     public class GroupUserDao
    {
        private OnlineShopDbContext db = null;
        public GroupUserDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<UserGroup> ListAllGroup()
        {
            return db.UserGroups.Where(x => x.ID != null).ToList();
        }
    }
}
