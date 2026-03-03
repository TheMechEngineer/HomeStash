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
        public string Name { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float CenterX { get; private set; }
        public float CenterY { get; private set; }
        public int RoomColor { get; private set; }

        private Storage RoomStorage = new Storage();
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return RoomStorage.StoredItems; }
        }

        private Room(string _RoomName, float _Width, float _Height, float _CenterX, float _CenterY, int _RoomColor)
        {
            Name = _RoomName;
            Width = _Width;
            Height = _Height;
            CenterX = _CenterX;
            CenterY = _CenterY;
            RoomColor = _RoomColor;
        }

        internal static bool TryCreate(string _RoomName, float _Width, float _Height, float _CenterX, float _CenterY, int _RoomColor, out Room? _CreatedRoom, out string? _ErrorMessage)
        {
            _CreatedRoom = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (!NameSelfValidation(_RoomName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (!SizeSelfValidation(_Width, _Height, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            if (CreationSuccess)
            {
                _CreatedRoom = new Room(_RoomName, _Width, _Height, _CenterX, _CenterY, _RoomColor);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        private static bool NameSelfValidation(string _RoomName, ref string? _ErrorMessage)
        {
            bool RoomNameValid = true;

            if (string.IsNullOrEmpty(_RoomName))
            {
                _ErrorMessage += "Room Name Must Contain Characters\n";
                RoomNameValid = false;
            }

            return RoomNameValid;
        }

        private static bool SizeSelfValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool RoomSizeValid = true;

            if (_Width <= 0 || _Height <= 0)
            {
                _ErrorMessage += "Width And Height Dimensions Must Be Positive Numbers\n";
                RoomSizeValid = false;
            }

            return RoomSizeValid;
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
