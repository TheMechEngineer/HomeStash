using BackEnd.Enumerations;
using BackEnd.ModelInterfaces;
using System.Text.Json.Serialization;

namespace BackEnd.ModelClasses
{
    /// <summary>
    /// Represents A Room That Contains Storage,
    /// And Provides Validation And Event Notification For Changes
    /// </summary>
    public class Room : IStorageHolder
    {
        /// <summary>
        /// Pass Through Event For When Stored Items Change
        /// </summary>
        public event Action? StoredItemsChanged
        {
            add { RoomStorage.StoredItemsChanged += value; }
            remove { RoomStorage.StoredItemsChanged -= value; }
        }

        /// <summary>
        /// Pass Through Event For When Stored Item Is Modified
        /// </summary>
        public event Action? StoredItemModified
        {
            add { RoomStorage.StoredItemModified += value; }
            remove { RoomStorage.StoredItemModified -= value; }
        }

        /// <summary>
        /// Event Triggered When The Room Name Changes
        /// </summary>
        public event Action? RoomNameChanged;

        /// <summary>
        /// Event Triggered When The Room Dimensions Change
        /// </summary>
        public event Action? RoomDimensionsChanged;

        /// <summary>
        /// Event Triggered When The Room Color Changes
        /// </summary>
        public event Action? RoomColorChanged;

        /// <summary>
        /// The Name Of The Room
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The Width Of The Room
        /// </summary>
        public float Width { get; private set; }

        /// <summary>
        /// The Height Of The Room
        /// </summary>
        public float Height { get; private set; }

        /// <summary>
        /// The X Coordinate Of The Room Center
        /// </summary>
        public float CenterX { get; private set; }

        /// <summary>
        /// The Y Coordinate Of The Room Center
        /// </summary>
        public float CenterY { get; private set; }

        /// <summary>
        /// The Color Of The Room Stored As An ARGB Integer
        /// </summary>
        public int RoomColor { get; private set; }

        /// <summary>
        /// The Storage Backer For Items Contained Directly Within The Room
        /// </summary>
        private Storage RoomStorage;

        /// <summary>
        /// The Storage For Items Contained Directly Within The Building
        /// </summary>
        public IStorage CurrentStorage
        {
            get
            { return RoomStorage; }
        }

        /// <summary>
        /// The List Of Items Directly Stored In The Room
        /// </summary>
        [JsonIgnore] // Excludes Stored Items From JSON To Prevent Repeat References During Serialization
        public IReadOnlyList<IStored> StoredItems
        {
            get
            { return RoomStorage.StoredItems; }
        }

        /// <summary>
        /// Private Constructor Used For Controlled Creation Of Room Objects
        /// </summary>
        /// <param name="_RoomName">The Name Of The Room</param>
        /// <param name="_Width">The Width Of The Room</param>
        /// <param name="_Height">The Height Of The Room</param>
        /// <param name="_CenterX">The Center X Coordinate Of The Room</param>
        /// <param name="_CenterY">The Center Y Coordinate Of The Room</param>
        /// <param name="_RoomColor">The Color Of The Room</param>
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

        /// <summary>
        /// Attempts To Create A New Room With Validation.
        /// Only Available Source To Create A Room Instance
        /// </summary>
        /// <param name="_RoomName">The Proposed Name Of The Room</param>
        /// <param name="_Width">The Proposed Width Of The Room</param>
        /// <param name="_Height">The Proposed Height Of The Room</param>
        /// <param name="_CenterX">The Proposed Center X Coordinate Of The Room</param>
        /// <param name="_CenterY">The Proposed Center Y Coordinate Of The Room</param>
        /// <param name="_RoomColor">The Proposed Color Of The Room</param>
        /// <param name="_CreatedRoom">The Room Instance Created If Successful</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal static bool TryCreate(string _RoomName, float _Width, float _Height, float _CenterX, float _CenterY, int _RoomColor, out Room? _CreatedRoom, out string? _ErrorMessage)
        {
            _CreatedRoom = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            // Runs Room Name Self-Validation
            if (!NameSelfValidation(_RoomName, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Runs Room Dimension Self-Validation
            if (!SizeSelfValidation(_Width, _Height, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Creates New Room Instance If Room Passes Self-Validation Checks
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

        /// <summary>
        /// Attempts To Modify Room Properties With Validation
        /// </summary>
        /// <param name="_NewRoomName">The Proposed Name Of The Room</param>
        /// <param name="_NewWidth">The Proposed Width Of The Room</param>
        /// <param name="_NewHeight">The Proposed Height Of The Room</param>
        /// <param name="_NewCenterX">The Proposed Center X Coordinate Of The Room</param>
        /// <param name="_NewCenterY">The Proposed Center Y Coordinate Of The Room</param>
        /// <param name="_NewRoomColor">The Proposed Color Of The Room</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal bool TryModify(string _NewRoomName, float _NewWidth, float _NewHeight, float _NewCenterX, float _NewCenterY, int _NewRoomColor, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            bool NameChanged = this.Name != _NewRoomName;
            bool DimensionsChanged = this.Width != _NewWidth || this.Height != _NewHeight || this.CenterX != _NewCenterX || this.CenterY != _NewCenterY;
            bool ColorChanged = this.RoomColor != _NewRoomColor;

            // Checks If Any Fields Have Been Modified
            if (NameChanged || DimensionsChanged || ColorChanged)
            {
                // Runs Room Name Self-Validation If Name Was Changed
                if (NameChanged)
                {
                    if (!NameSelfValidation(_NewRoomName, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }

                // Runs Room Dimension Self-Validation If Dimension Was Changed
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

            // Modifies Room Fields If Room Passes Self-Validation Checks
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

        /// <summary>
        /// Validates Room Name Self Requirements
        /// </summary>
        /// <param name="_RoomName">The Proposed Name To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        private static bool NameSelfValidation(string _RoomName, ref string? _ErrorMessage)
        {
            bool RoomNameValid = true;

            // Validates The Name Entered Was Not Empty
            if (string.IsNullOrEmpty(_RoomName))
            {
                _ErrorMessage += "Self Validation Error: Room Name Must Contain Characters\n";
                RoomNameValid = false;
            }

            return RoomNameValid;
        }

        /// <summary>
        /// Validates Room Dimension Self Requirements
        /// </summary>
        /// <param name="_Width">The Proposed Width To Validate</param>
        /// <param name="_Height">The Proposed Height To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        private static bool SizeSelfValidation(float _Width, float _Height, ref string? _ErrorMessage)
        {
            bool RoomSizeValid = true;

            // Validates The Dimensions Are Positive Numbers
            if (_Width <= 0 || _Height <= 0)
            {
                _ErrorMessage += "Self Validation Error: Width And Height Dimensions Must Be Positive Numbers\n";
                RoomSizeValid = false;
            }

            return RoomSizeValid;
        }

        /// <summary>
        /// Calculates The Total Number Of Items In The Room
        /// </summary>
        public int TotalItemCount()
        {
            return RoomStorage.TotalItemCount();
        }

        /// <summary>
        /// Calculates The Total Value Of Items In The Room
        /// </summary>
        public double TotalItemValue()
        {
            return RoomStorage.TotalItemValue();
        }

        /// <summary>
        /// Attempts To Add A Stored Item To The Room Storage
        /// </summary>
        /// <param name="_StoredType">The Type Of The Proposed IStored</param>
        /// <param name="_StoredName">The Proposed Name Of The IStored</param>
        /// <param name="_Description">The Proposed Description Of The IStored</param>
        /// <param name="_Value">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_Quantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage)
        {
            return RoomStorage.TryAddIStored(_StoredType, _StoredName, _Description, _Value, _Quantity, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Modify A Stored Item In The Room Storage
        /// </summary>
        /// <param name="_IStoredToModify">The IStored Instance To Attempt To Modify</param>
        /// <param name="_NewStoredName">The Proposed Name Of The IStored</param>
        /// <param name="_NewDescription">The Proposed Description Of The IStored</param>
        /// <param name="_NewValue">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_NewQuantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage)
        {
            return RoomStorage.TryModifyIStored(_IStoredToModify, _NewStoredName, _NewDescription, _NewValue, _NewQuantity, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Move A Stored Item From The Current Storage To Another Storage
        /// </summary>
        /// <param name="_ItemToMove">The IStored Instance To Attempt To Move</param>
        /// <param name="_Destination">The Proposed Destination Storage Holder</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage)
        {
            return RoomStorage.TryMoveIStored(_ItemToMove, _Destination, out _ErrorMessage);
        }

        /// <summary>
        /// Attempts To Remove A Stored Item From The Room Storage
        /// </summary>
        /// <param name="_StoredToRemove">The IStored Instance To Attempt To Remove</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage)
        {
            return RoomStorage.TryRemoveIStored(_StoredToRemove, out _ErrorMessage);
        }
    }
}