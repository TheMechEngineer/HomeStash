using BackEnd.Enumerations;
using BackEnd.ModelClasses;
using BackEnd.Utilities;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackEnd.DataContinuity
{
    public static class DataContinuityController
    {
        public static RootManager StartupSmallPopulate()
        {
            RootManager ReturnItem = new RootManager();


            ReturnItem.TryAddUser("Caleb", out _); // _ Is a special placeholder meant to discard unwanted output

            ReturnItem.TryChangeActiveUser(ReturnItem.UserList[0], out _);

            ReturnItem.ActiveUser.TryAddBuilding("Home", 15, 10, out _);

            ReturnItem.ActiveUser.TryChangeActiveBuilding(ReturnItem.ActiveUser.BuildingList[0], out _);

            ReturnItem.ActiveUser.ActiveBuilding.TryAddRoom("Room1", 3, 1, 4.5f, 2, Color.Red.ToArgb(), out _);

            ReturnItem.ActiveUser.ActiveBuilding.TryAddIStored(StoredItemType.Item, "Item1", "Test Description 1", 1, 2, out _);
            ReturnItem.ActiveUser.ActiveBuilding.TryAddIStored(StoredItemType.Item, "Item2", "Test Description 2", 1, 2, out _);
            ReturnItem.ActiveUser.ActiveBuilding.TryAddIStored(StoredItemType.Container, "Container1", "Container1 Description", 0, 1, out _);

            ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Item, "Item3", "Test Description 3", 1, 2, out _);
            ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Item, "Item4", "Test Description 4", 1, 2, out _);

            ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Container, "Container2", "Item Storage", 0, 1, out _);
            ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Container, "Container3", "First Container", 1, 2, out _);

            (ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].StoredItems[2] as Container).TryAddIStored(StoredItemType.Item, "Container Item 1", "First Container Item", 3, 18, out _);
            (ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].StoredItems[2] as Container).TryAddIStored(StoredItemType.Item, "Container Item 2", "Second Container Item", 1, 117, out _);
            (ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].StoredItems[2] as Container).TryAddIStored(StoredItemType.Container, "Container 4", "4 Container", 3, 5, out _);
            ((ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].StoredItems[2] as Container).StoredItems[2] as Container).TryAddIStored(StoredItemType.Container, "Sub Container Item 1", "Second Container", 15, 4, out _);

            return ReturnItem;
        }

        public static RootManager StartupLargePopulate()
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
            ReturnItem.ActiveUser.ActiveBuilding.RoomList[2].TryAddIStored(StoredItemType.Container, "New Container", "Item Storage", 0, 1, out _);

            ReturnItem.ActiveUser.ActiveBuilding.RoomList[3].TryAddIStored(StoredItemType.Container, "Container 1", "First Container", 1, 2, out _);

            (ReturnItem.ActiveUser.ActiveBuilding.RoomList[3].StoredItems[0] as Container).TryAddIStored(StoredItemType.Item, "Container Item 1", "First Container Item", 3, 18, out _);
            (ReturnItem.ActiveUser.ActiveBuilding.RoomList[3].StoredItems[0] as Container).TryAddIStored(StoredItemType.Item, "Container Item 2", "Second Container Item", 1, 117, out _);
            (ReturnItem.ActiveUser.ActiveBuilding.RoomList[3].StoredItems[0] as Container).TryAddIStored(StoredItemType.Container, "Container 2", "Second Container", 3, 5, out _);
            ((ReturnItem.ActiveUser.ActiveBuilding.RoomList[3].StoredItems[0] as Container).StoredItems[2] as Container).TryAddIStored(StoredItemType.Container, "Sub Container Item 1", "Second Container", 15, 4, out _);

            for (int i = 0; i < 50; i++)
            {
                ReturnItem.ActiveUser.ActiveBuilding.RoomList[0].TryAddIStored(StoredItemType.Item, $"Item{i+50}", $"Test Description {i + 50}", 1, i, out _);
            }

            return ReturnItem;
        }

        public static RootManager StartupDataContinuity()
        {
            string StorageFile = "HomeStashData.json";
            RootManager LiveRootManager;

            if (File.Exists(StorageFile))
            {
                RootManagerDTO StoredRootManager = JsonSerializer.Deserialize<RootManagerDTO>(File.ReadAllText(StorageFile));

                LiveRootManager = Converter.ToRootManager(StoredRootManager);
            }
            else
            {
                LiveRootManager = new RootManager();
            }

            return LiveRootManager;
        }

        public static void ShutdownDataContinuity(RootManager _ProgramInstance)
        {
            string StorageFile = "HomeStashData.json";
            string JSONString = JsonSerializer.Serialize(_ProgramInstance);
            File.WriteAllText(StorageFile, JSONString);
        }

    }
}
