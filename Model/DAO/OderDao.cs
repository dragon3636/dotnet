using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
   public class OderDao
    {
        OnlineShopDbContext db = null;
       public OderDao()
        {
            db = new OnlineShopDbContext();
        }

         public  long InsertOder(Order oder)
        {
            db.Order.Add(oder);
            
            db.SaveChanges();
            return oder.ID;
        }
    }
}
