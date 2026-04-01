using System.Text.Json.Serialization;

namespace BackEnd.ModelClasses
{
    /// <summary>
    /// Represents A User That Contains Buildings,
    /// And Provides Validation And Event Notification For Changes
    /// </summary>
    public class User
    {
        /// <summary>
        /// Event Triggered When The Active Building Changes
        /// </summary>
        public event Action? ActiveBuildingChanged;

        /// <summary>
        /// Event Triggered When The Building List Changes
        /// </summary>
        public event Action? BuildingListChanged;

        /// <summary>
        /// The Username Of The User
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The Current Active Building Backing Field For The User
        /// </summary>
        private Building? __ActiveBuilding;

        /// <summary>
        /// The Current Active Building For The User
        /// </summary>
        [JsonIgnore] // Excludes Active Building From JSON As This Is Not Needed For Long Term Storage
        public Building? ActiveBuilding
        {
            get
            { return __ActiveBuilding; }
        }

        /// <summary>
        /// Internal List Of Buildings For The User
        /// </summary>
        private List<Building> __BuildingList = new List<Building>();

        /// <summary>
        /// The Read-Only List Of Buildings For The User
        /// </summary>
        public IReadOnlyList<Building> BuildingList
        {
            get
            { return __BuildingList.AsReadOnly(); }
        }

        /// <summary>
        /// Private Constructor Used For Controlled Creation Of User Objects
        /// </summary>
        private User(string _Username)
        {
            this.Username = _Username;
        }

        /// <summary>
        /// Attempts To Create A New User With Validation.
        /// Only Available Source To Create A User Instance
        /// </summary>
        internal static bool TryCreate(string _Username, out User? _CreatedUser, out string? _ErrorMessage)
        {
            _CreatedUser = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            // Runs Username Self-Validation
            if (!UsernameSelfValidation(_Username, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

            // Creates New User Instance If User Passes Self-Validation Checks
            if (CreationSuccess)
            {
                _CreatedUser = new User(_Username);
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return CreationSuccess;
        }

        /// <summary>
        /// Attempts To Modify User Properties With Validation
        /// </summary>
        internal bool TryModify(string _NewUsername, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            bool UsernameChanged = this.Username != _NewUsername;

            // Checks If Any Fields Have Been Modified
            if (UsernameChanged) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
            {
                // Runs Username Self-Validation If Username Was Changed
                if (UsernameChanged) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
                {
                    if (!UsernameSelfValidation(_NewUsername, ref _ErrorMessage))
                    {
                        ModifySuccess = false;
                    }
                }
            }
            else
            {
                ModifySuccess = false;
                _ErrorMessage += $"No User Fields Have Been Modified\n";
            }

            // Modifies User Fields If User Passes Self-Validation Checks
            if (ModifySuccess)
            {
                this.Username = _NewUsername;
            }
            else
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifySuccess;
        }

        /// <summary>
        /// Validates Username Self Requirements
        /// </summary>
        private static bool UsernameSelfValidation(string _Username, ref string? _ErrorMessage)
        {
            bool UsernameValid = true;

            // Validates The Username Entered Was Not Empty
            if (string.IsNullOrEmpty(_Username))
            {
                _ErrorMessage += "Self Validation Error: Username Must Contain Characters\n";
                UsernameValid = false;
            }

            return UsernameValid;
        }

        /// <summary>
        /// Attempts To Add A Building To The User With Validation
        /// </summary>
        public bool TryAddBuilding(string _BuildingName, float _Width, float _Height, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddBuildingSuccess = true;

            // Runs Building Name System-Validation
            if (!BuildingNameSystemValidation(_BuildingName, ref _ErrorMessage))
            {
                AddBuildingSuccess = false;
            }

            // If Building Passes System-Validation Attempt To Create The Building
            if (AddBuildingSuccess)
            {
                Building? NewBuilding;

                if (Building.TryCreate(_BuildingName, _Width, _Height, out NewBuilding, out _ErrorMessage))
                {
                    __BuildingList.Add(NewBuilding);
                    BuildingListChanged?.Invoke();
                }
                else
                {
                    AddBuildingSuccess = false;
                }
            }

            if (!AddBuildingSuccess)
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return AddBuildingSuccess;
        }

        /// <summary>
        /// Attempts To Modify A Building With Validation
        /// </summary>
        public bool TryModifyBuilding(Building _BuildingToModify, string _NewBuildingName, float _NewWidth, float _NewHeight, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyBuildingSuccess = true;

            bool NameChanged = _BuildingToModify.Name != _NewBuildingName;
            bool DimensionsChanged = _BuildingToModify.Width != _NewWidth || _BuildingToModify.Height != _NewHeight;

            // Checks If Any Fields Have Been Modified
            if (NameChanged || DimensionsChanged)
            {
                // Runs Building Name System-Validation If Name Was Changed
                if (NameChanged)
                {
                    if (!BuildingNameSystemValidation(_NewBuildingName, ref _ErrorMessage))
                    {
                        ModifyBuildingSuccess = false;
                    }
                }

                // If Building Passes System-Validation Attempt To Modify The Building
                if (ModifyBuildingSuccess)
                {
                    if (_BuildingToModify.TryModify(_NewBuildingName, _NewWidth, _NewHeight, out _ErrorMessage))
                    {
                        BuildingListChanged?.Invoke();
                    }
                    else
                    {
                        ModifyBuildingSuccess = false;
                    }
                }
            }
            else
            {
                ModifyBuildingSuccess = false;
                _ErrorMessage += $"No Building Fields Have Been Modified\n";
            }

            if (!ModifyBuildingSuccess)
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifyBuildingSuccess;
        }

        /// <summary>
        /// Attempts To Remove A Building From The User
        /// </summary>
        public bool TryRemoveBuilding(Building _BuildingToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            // Validates The Building To Remove Exists Within The User
            if (!__BuildingList.Contains(_BuildingToRemove))
            {
                _ErrorMessage = "Building To Be Removed Must Exist In The Building List";
                return false;
            }

            // If Building To Remove Is The Active Building, Clear Active Building
            if (_BuildingToRemove == __ActiveBuilding)
            {
                __ActiveBuilding = null;
                ActiveBuildingChanged?.Invoke();
            }

            // Removes Building And Triggers Event
            __BuildingList.Remove(_BuildingToRemove);
            BuildingListChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Attempts To Change The Active Building For The User
        /// </summary>
        public bool TryChangeActiveBuilding(Building _NewActiveBuilding, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            // Validates The New Active Building Exists Within The User
            if (!__BuildingList.Contains(_NewActiveBuilding))
            {
                _ErrorMessage = "New Active Building Must Exist In The Building List";
                return false;
            }

            __ActiveBuilding = _NewActiveBuilding;
            ActiveBuildingChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Placeholder For Future Building Copy Functionality
        /// </summary>
        public void CopyBuilding(Building _BuildingToCopy)
        {
            //stub
        }

        /// <summary>
        /// Validates Building Name System Requirements
        /// </summary>
        private bool BuildingNameSystemValidation(string _BuildingName, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            // Validates The Name Entered Is Not A Duplicate
            if (__BuildingList.Any(CurrentBuilding => CurrentBuilding.Name == _BuildingName))
            {
                _ErrorMessage += $"System Validation Error: {_BuildingName} Already Exists. No Duplicate Building Names.\n";
                SystemValid = false;
            }

            return SystemValid;
        }
    }
}