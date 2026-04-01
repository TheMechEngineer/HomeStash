using BackEnd.Enumerations;
using BackEnd.ModelClasses;

namespace BackEnd.DataContinuity
{
    /// <summary>
    /// Provides Conversion Logic For Transforming Data Transfer Objects (DTOs)
    /// Into Their Corresponding Live Model Objects
    /// </summary>
    internal static class Converter
    {
        /// <summary>
        /// Converts A RootManagerDTO Into A Fully Populated RootManager Instance.
        /// Reconstructs The Object Hierarchy Including Users, Buildings, Rooms,
        /// And Their Associated Storage Contents
        /// </summary>
        /// <param name="_RootManagerDTO">The Source DTO Containing Serialized System Data</param>
        /// <returns>A Populated RootManager Instance Reflecting The DTO Structure.</returns>
        internal static RootManager ToRootManager(RootManagerDTO _RootManagerDTO)
        {
            RootManager RootManagerInstance = new RootManager();

            foreach (UserDTO CurrentUserDTO in _RootManagerDTO.UserList)
            {
                // Create A User Instance Based On The Current User DTO
                RootManagerInstance.TryAddUser(CurrentUserDTO.Username, out _);

                // Assumes The Newly Added User Is The Last In The List
                User CurrentUser = RootManagerInstance.UserList.Last();

                foreach (BuildingDTO CurrentBuildingDTO in CurrentUserDTO.BuildingList)
                {
                    // Create A Building Instance Based On The Current Building DTO
                    CurrentUser.TryAddBuilding(CurrentBuildingDTO.Name, CurrentBuildingDTO.Width, CurrentBuildingDTO.Height, out _);

                    // Assumes The Newly Added Building Is The Last In The List
                    Building CurrentBuilding = CurrentUser.BuildingList.Last();

                    //Populate The Storage That Is Directly In The Building
                    PopulateNestedStorage(CurrentBuilding.CurrentStorage as Storage, CurrentBuildingDTO.CurrentStorage);

                    foreach (RoomDTO CurrentRoomDTO in CurrentBuildingDTO.RoomList)
                    {
                        // Create A Room Instance Based On The Current Room DTO
                        CurrentBuilding.TryAddRoom(CurrentRoomDTO.Name, CurrentRoomDTO.Width, CurrentRoomDTO.Height, CurrentRoomDTO.CenterX, CurrentRoomDTO.CenterY, CurrentRoomDTO.RoomColor, out _);

                        // Assumes The Newly Added Room Is The Last In The List
                        Room CurrentRoom = CurrentBuilding.RoomList.Last();

                        //Populate The Storage That Is Directly In The Room
                        PopulateNestedStorage(CurrentRoom.CurrentStorage as Storage, CurrentRoomDTO.CurrentStorage);
                    }
                }
            }
            return RootManagerInstance;
        }

        /// <summary>
        /// Recursively Populates A Storage Object Using Data From A StorageDTO.
        /// Handles Both Individual Items And Container Items With Nested Storage
        /// </summary>
        /// <param name="_TargetObject">The Destination Storage Object To Populate</param>
        /// <param name="_SenderObject">The Source DTO Containing Stored Item Data</param>
        private static void PopulateNestedStorage(Storage _TargetObject, StorageDTO _SenderObject)
        {
            foreach (ItemDTO CurrentItemDTO in _SenderObject.StoredItems)
            {
                //Determines If The Current Item DTO Is A Container Or Item. Items Will Have A Null Value For Current Storage 
                if (CurrentItemDTO.CurrentStorage == null)
                {
                    // Add A Simple Item (No Nested Storage)
                    _TargetObject.TryAddIStored(StoredItemType.Item, CurrentItemDTO.Name, CurrentItemDTO.Description, CurrentItemDTO.Value, CurrentItemDTO.Quantity, out _);
                }
                else
                {
                    // Add A Container Item
                    _TargetObject.TryAddIStored(StoredItemType.Container, CurrentItemDTO.Name, CurrentItemDTO.Description, CurrentItemDTO.Value, CurrentItemDTO.Quantity, out _);

                    //Recursively Populate Its Storage
                    PopulateNestedStorage((_TargetObject.StoredItems.Last() as Container).CurrentStorage as Storage, CurrentItemDTO.CurrentStorage);
                }
            }
        }
    }
}
