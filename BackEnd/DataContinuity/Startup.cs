using BackEnd.ModelClasses;
using BackEnd.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.DataContinuity
{
    public static class Startup
    {
        public static List<Item> TempItemStatup()
        {
            List<Item> ReturnList = new List<Item>();

            ReturnList.Add(new Item { ID = IDManager.GetNextID(), Name = "TV", Value = 1200 });
            ReturnList.Add(new Item { ID = IDManager.GetNextID(), Name = "Laptop", Value = 1800 });

            return ReturnList;
        }

        public static RootManager TempItemStatup2()
        {
            RootManager ReturnItem = new RootManager();

            ReturnItem.AddUser(new User { UserName = "Bill" });
            ReturnItem.AddUser(new User { UserName = "Ted" });
            ReturnItem.AddUser(new User { UserName = "John" });
            ReturnItem.AddUser(new User { UserName = "Caleb" });

            ReturnItem.UserList[3].AddBuilding(new Building { Name = "Home", Height = 3, Width = 3 });
            ReturnItem.UserList[3].AddBuilding(new Building { Name = "1000", Height = 1000, Width = 1000 });

            return ReturnItem;
        }

    }
}
