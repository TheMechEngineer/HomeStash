using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Enumerations;
using BackEnd.ModelClasses;

namespace BackEnd.DataContinuity
{
    internal static class Converter
    {
        internal static RootManager ToRootManager(RootManagerDTO _RootManagerDTO)
        {
            RootManager RootManagerInstance = new RootManager();

            foreach (UserDTO CurrentUserDTO in _RootManagerDTO.UserList)
            {
                RootManagerInstance.TryAddUser(CurrentUserDTO.Username, out _);

                User CurrentUser = RootManagerInstance.UserList.Last();

                foreach (BuildingDTO CurrentBuildingDTO in CurrentUserDTO.BuildingList)
                {
                    CurrentUser.TryAddBuilding(CurrentBuildingDTO.Name, CurrentBuildingDTO.Width, CurrentBuildingDTO.Height, out _);

                    Building CurrentBuilding = CurrentUser.BuildingList.Last();

                    PopulateNestedStorage(CurrentBuilding.CurrentStorage as Storage, CurrentBuildingDTO.CurrentStorage);

                    foreach (RoomDTO CurrentRoomDTO in CurrentBuildingDTO.RoomList)
                    {
                        CurrentBuilding.TryAddRoom(CurrentRoomDTO.Name, CurrentRoomDTO.Width, CurrentRoomDTO.Height, CurrentRoomDTO.CenterX, CurrentRoomDTO.CenterY, CurrentRoomDTO.RoomColor, out _);

                        Room CurrentRoom = CurrentBuilding.RoomList.Last();

                        PopulateNestedStorage(CurrentRoom.CurrentStorage as Storage, CurrentRoomDTO.CurrentStorage);
                    }
                }            
            }
            return RootManagerInstance;
        }

        private static void PopulateNestedStorage(Storage _TargetObject, StorageDTO _SenderObject)
        {
            foreach (ItemDTO CurrentItemDTO in _SenderObject.StoredItems)
            {
                if (CurrentItemDTO.CurrentStorage == null)
                {
                    _TargetObject.TryAddIStored(StoredItemType.Item, CurrentItemDTO.Name, CurrentItemDTO.Description, CurrentItemDTO.Value, CurrentItemDTO.Quantity, out _);
                }
                else
                {
                    _TargetObject.TryAddIStored(StoredItemType.Container, CurrentItemDTO.Name, CurrentItemDTO.Description, CurrentItemDTO.Value, CurrentItemDTO.Quantity, out _);

                    PopulateNestedStorage((_TargetObject.StoredItems.Last() as Container).CurrentStorage as Storage, CurrentItemDTO.CurrentStorage);
                }
            }
        }
    }
}
