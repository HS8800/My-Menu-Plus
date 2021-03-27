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
    public class OrderDisplayHub : Hub
    {



        private static List<WebSocketClientModel> WebSocketClients = new List<WebSocketClientModel>();


        public void OrderComplete(string orderID)//Needs Security
        {
            Debug.WriteLine(orderID);

            
            //Clients.All.order(1,3,"Items Example");
        }

        public void Order(int id, int tableNumber, string items) {
            Clients.All.order(id, tableNumber, items);
        }
        public void LoginToDisplay(string key, string connectionID) {

            var displayLogin = AccountHelper.displayLogin(key);

            if (displayLogin.success)
            {

                if (WebSocketClients.FindIndex(item => item.connectionID == connectionID) == 0)
                {
                    WebSocketClientModel client = new WebSocketClientModel();
                    client.menuID = displayLogin.menuID;
                    client.connectionID = connectionID;
                    WebSocketClients.Add(client);
         
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