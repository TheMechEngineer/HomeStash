using BackEnd.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Room : IStorage
    {
        public string Name { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public int RoomColor { get; set; }

        private Storage RoomStorage = new Storage();
        public IReadOnlyList<IStored> StoredItems
        {
            get
            {
                return RoomStorage.StoredItems;
            }
        }

        public int TotalItemCount()
        {
            return RoomStorage.TotalItemCount();
        }
        public double TotalItemValue()
        {
            return RoomStorage.TotalItemValue();
        }
        public void AddItem(IStored _ItemToAdd)
        {
            RoomStorage.AddItem(_ItemToAdd);
        }

        public void RemoveItem(IStored _ItemToRemove)
        {
            RoomStorage.RemoveItem(_ItemToRemove);
        }
        public void MoveItem(IStored _ItemToMove, IStorage _Destination)
        {
            RoomStorage.MoveItem(_ItemToMove, _Destination);
        }

    }
}
