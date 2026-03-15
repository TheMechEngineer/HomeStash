using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class Room : IStorageHolder
    {
        public event Action? StoredItemsChanged
        {
            add { RoomStorage.StoredItemsChanged += value; }
            remove { RoomStorage.StoredItemsChanged -= value; }
        }

        public event Action? StoredItemModified
        {
            add { RoomStorage.StoredItemModified += value; }
            remove { RoomStorage.StoredItemModified -= value; }
        }

        public event Action? RoomNameChanged;
        public event Action? RoomDimensionsChanged;
        public event Action? RoomColorChanged;

        public string Name { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float CenterX { get; private set; }
        public float CenterY { get; private set; }
        public int RoomColor { get; private set; }

        private Storage RoomStorage;
        public IStorage CurrentStorage
        {
            get
            { return RoomStorage; }
        }
        [JsonIgnore]
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

            RoomStorage = new Storage(this);
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

        internal bool TryModify(string _NewRoomName, float _NewWidth, float _NewHeight, float _NewCenterX, float _NewCenterY, int _NewRoomColor, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            bool NameChanged = this.Name != _NewRoomName;
            bool DimensionsChanged = this.Width != _NewWidth || this.Height != _NewHeight || this.CenterX != _NewCenterX || this.CenterY != _NewCenterY;
            bool ColorChanged = this.RoomColor != _NewRoomColor;

            if (NameChanged || DimensionsChanged || ColorChanged)
            {
                if (NameChanged)
                {
                    if (!NameSelfValidation(_NewRoomName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                if (DimensionsChanged)
                {
                    if (!SizeSelfValidation(_NewWidth, _NewHeight, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }
            }
            else
            {
                ModifySuccess = false;
                _ErrorMessage += $"No Room Fields Have Been Modified\n";
            }

            if (ModifySuccess)
            {
                this.Name = _NewRoomName;
                this.Width = _NewWidth;
                this.Height = _NewHeight;
                this.CenterX = _NewCenterX;
                this.CenterY = _NewCenterY;
                this.RoomColor = _NewRoomColor;

                if (NameChanged)
                {
                    this.RoomNameChanged?.Invoke();
                }

                if (DimensionsChanged)
                {
                    this.RoomDimensionsChanged?.Invoke();
                }

                if (ColorChanged)
                {
                    this.RoomColorChanged?.Invoke();
                }
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifySuccess;
        }

        private static bool NameSelfValidation(string _RoomName, ref string? _ErrorMessage)
        {
            bool RoomNameValid = true;

            if (string.IsNullOrEmpty(_RoomName))
            {
                _ErrorMessage += "Self Validation Error: Room Name Must Contain Characters\n";
                RoomNameValid = false;
            }

            return RoomNameValid;
        }

        private static bool SizeSelfValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool RoomSizeValid = true;

            if (_Width <= 0 || _Height <= 0)
            {
                _ErrorMessage += "Self Validation Error: Width And Height Dimensions Must Be Positive Numbers\n";
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

        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            return RoomStorage.TryAddIStored(_StoredType, _StoredName, _Description, _Value, _Quantity, out _ErrorMessage);
        }

        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            return RoomStorage.TryModifyIStored(_IStoredToModify, _NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
        }

        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage)
        {
            return RoomStorage.TryMoveIStored(_ItemToMove, _Destination, out _ErrorMessage);
        }

        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage)
        {
            return RoomStorage.TryRemoveIStored(_StoredToRemove, out _ErrorMessage);
        }

    }
}
