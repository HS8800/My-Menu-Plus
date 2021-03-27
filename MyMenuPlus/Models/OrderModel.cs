using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMenuPlus.Models
{
    public class OrderModel
    {
        public int id { get; set; }
        public int menuID { get; set; }
        public string transaction { get; set; }
        public int tableNumber { get; set; }
        public string itemsJSON { get; set; }
        public DateTime ordered { get; set; }
    }
}