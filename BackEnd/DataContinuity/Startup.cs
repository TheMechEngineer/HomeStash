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


            ReturnItem.TryAddUser("Bill", out _); // _ Is a special placeholder meant to discard unwanted output
            ReturnItem.TryAddUser("Ted", out _);
            ReturnItem.TryAddUser("John", out _);
            ReturnItem.TryAddUser("Caleb", out _);

            ReturnItem.TryChangeActiveUser(ReturnItem.UserList[3], out _);

            ReturnItem.ActiveUser.TryAddBuilding("Home", 15, 10, out _ );
            ReturnItem.ActiveUser.TryAddBuilding("1000", 1000, 1000, out _ );

            return ReturnItem;
        }

    }
}
