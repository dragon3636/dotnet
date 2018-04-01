using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
     public class MenuDao
    {
        private OnlineShopDbContext db = null;
         public MenuDao()
        {
            db = new OnlineShopDbContext();
        }
         public List<Menu>  ListByGroupID(int typeId)
        {
            return db.Menu.Where(x => x.TypeID == typeId&&x.Status==true).OrderBy(x => x.DesplayOrder).ToList();
        }

    }
}
