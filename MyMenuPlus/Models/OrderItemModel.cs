using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMenuPlus.Models
{
    public class OrderItemModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal pricePerUnit { get; set; }
        public int qty { get; set; }

    }
}