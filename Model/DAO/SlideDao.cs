﻿
using Model.EF;
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
   public class SlideDao
    {
        OnlineShopDbContext db = null;
        public SlideDao()
        {
            db = new OnlineShopDbContext();
        }
         public List<Slide> ListAll()
        {
            return db.Slide.Where(x => x.Status == true).OrderByDescending(y => y.CreateDate).ToList();

        }
    }
}
