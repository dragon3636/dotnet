 using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShow.Models
{
    [Serializable]
    public class CarItem
    {
        public   Product Product { get; set; }
        public int Quantity { get; set; }
        
    }
}