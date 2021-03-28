using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MyMenuPlus.Helpers;
using MyMenuPlus.Models;
using MySql.Data.MySqlClient;

namespace MyMenuPlus
{
    public class OrderDisplayClients{
        public static List<WebSocketClientModel> WebSocketClients = new List<WebSocketClientModel>();
    }

    public class OrderDisplayHub : Hub
    {
        

        public void OrderComplete(string orderID)//Needs Security
        {
            Debug.WriteLine(orderID);

            
            //Clients.All.order(1,3,"Items Example");
        }


        public void CompleteOrder(int orderID,string connectionID) {

            var ConnectionIndex = OrderDisplayClients.WebSocketClients.FindIndex(client => client.connectionID == connectionID);
            if (ConnectionIndex == -1)
            {//Client not logged in
                return;
            }

            MenuContentHelper.updateOrderStatus(orderID, 1, OrderDisplayClients.WebSocketClients[ConnectionIndex].menuID);
            

        }


        public void LoadOrders(string connectionID) {

            var ConnectionIndex = OrderDisplayClients.WebSocketClients.FindIndex(client => client.connectionID == connectionID);
            if (ConnectionIndex == -1) {//Client not logged in
                return;
            }

            Clients.Client(connectionID).orders(MenuContentHelper.LoadOrders(OrderDisplayClients.WebSocketClients[ConnectionIndex].menuID));

        }

        public void LoginToDisplay(string key, string connectionID) {

            var displayLogin = AccountHelper.displayLogin(key);

            if (displayLogin.success)
            {

   
                if (OrderDisplayClients.WebSocketClients.FindIndex(item => item.connectionID == connectionID) == -1)
                {
                    WebSocketClientModel client = new WebSocketClientModel();
                    client.menuID = displayLogin.menuID;
                    client.connectionID = connectionID;
                    OrderDisplayClients.WebSocketClients.Add(client);
         
                    Clients.Client(connectionID).login("Logged in");
                }
                else
                {
                    Clients.Client(connectionID).login("Already logged in");
                }

            }
            else {
                Clients.Client(connectionID).login("Login Failed");
            }
      
        }
       

    }
}