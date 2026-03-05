using BackEnd.Enumerations;
using BackEnd.ModelClasses;
using BackEnd.Utilities;

using System;
using System.Collections.Generic;
using System.Drawing;
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

            //ReturnList.Add(new Item { ID = IDManager.GetNextID(), Name = "TV", Value = 1200 });
            //ReturnList.Add(new Item { ID = IDManager.GetNextID(), Name = "Laptop", Value = 1800 });

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

            ReturnItem.ActiveUser.TryChangeActiveBuilding(ReturnItem.ActiveUser.BuildingList[0], out _);

            ReturnItem.ActiveUser.ActiveBuilding.TryAddRoom("Room1",3,1,4.5f,2,Color.Red.ToArgb(), out _);
            ReturnItem.ActiveUser.ActiveBuilding.TryAddRoom("Room2",2,3,4.5f,4,Color.Green.ToArgb(), out _);
            ReturnItem.ActiveUser.ActiveBuilding.TryAddRoom("Room3",5,5,12,7,Color.Teal.ToArgb(), out _);
            ReturnItem.ActiveUser.ActiveBuilding.TryAddRoom("Room4",12,1,7.5f,1,Color.GreenYellow.ToArgb(), out _);

            ReturnItem.ActiveUser.ActiveBuilding.TryAddIStored(StoredItemType.Item, "Item1", "Test Description 1", 1, 2, out _);

            ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Item, "Item2", "Test Description 2", 0, 2, out _);
            ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Item, "Item3", "Test Description 3", 1, 1, out _);
            ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Item, "Item4", "Test Description 4", 5, 2, out _);

            ReturnItem.ActiveUser.ActiveBuilding.RoomList[1].TryAddIStored(StoredItemType.Item, "Item5", "Test Description 5", 20, 3, out _);
            ReturnItem.ActiveUser.ActiveBuilding.RoomList[1].TryAddIStored(StoredItemType.Item, "Item6", "Test Description 6", 1, 50, out _);

            ReturnItem.ActiveUser.ActiveBuilding.RoomList[2].TryAddIStored(StoredItemType.Item, "Item7", "Test Description 7", 100, 1, out _);

            return ReturnItem;
        }

    }
}
