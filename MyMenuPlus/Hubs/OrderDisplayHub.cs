using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MyMenuPlus
{
    public class OrderDisplayHub : Hub
    {
        public void OrderComplete(string order)//Needs Security
        {
            Debug.WriteLine(order);

            
            //Clients.All.order(1,3,"Items Example");
        }

        public void Order(int id, int tableNumber, string items) {
            Clients.All.order(id, tableNumber, items);
        }
       

    }
}