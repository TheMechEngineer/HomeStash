using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class User
    {
        public event Action? ActiveBuildingChanged;
        public event Action? BuildingListChanged;

        public string Username { get; private set; }

        private Building? __ActiveBuilding;
        public Building? ActiveBuilding
        {
            get
            { return __ActiveBuilding; }
        }

        private List<Building> __BuildingList = new List<Building>();
        public IReadOnlyList<Building> BuildingList
        {
            get
            { return __BuildingList.AsReadOnly(); }
        }

        private User(string _Username)
        {
            this.Username = _Username;
        }

        internal static bool TryCreate(string _Username, out User? _CreatedUser, out string? _ErrorMessage)
        {
            _CreatedUser = null;
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (!UsernameSelfValidation(_Username, ref _ErrorMessage))
            {
                CreationSuccess = false;
            }

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

        internal bool TryModify(string _NewUsername, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            if (this.Username != _NewUsername) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
            {
                if (this.Username != _NewUsername) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
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

        private static bool UsernameSelfValidation(string _Username, ref string? _ErrorMessage)
        {
            bool UsernameValid = true;

            if (string.IsNullOrEmpty(_Username))
            {
                _ErrorMessage += "Self Validation Error: Username Must Contain Characters\n";
                UsernameValid = false;
            }

            return UsernameValid;
        }

        public bool TryAddBuilding(string _BuildingName, float _Width, float _Height, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddBuildingSuccess = true;

            if (!BuildingNameSystemValidation(_BuildingName, ref _ErrorMessage))
            {
                AddBuildingSuccess = false;
            }

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

        public bool TryModifyBuilding(Building _BuildingToModify, string _NewBuildingName, float _NewWidth, float _NewHeight, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyBuildingSuccess = true;

            if (_BuildingToModify.Name != _NewBuildingName || _BuildingToModify.Width != _NewWidth || _BuildingToModify.Height != _NewHeight)
            {
                if (_BuildingToModify.Name != _NewBuildingName)
                {
                    if (!BuildingNameSystemValidation(_NewBuildingName, ref _ErrorMessage))
                    {
                        ModifyBuildingSuccess = false;
                    }
                }

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

        public bool TryRemoveBuilding(Building _BuildingToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (!__BuildingList.Contains(_BuildingToRemove))
            {
                _ErrorMessage = "Building To Be Removed Must Exist In The Building List";
                return false;
            }

            if (_BuildingToRemove == __ActiveBuilding)
            {
                __ActiveBuilding = null;
                ActiveBuildingChanged?.Invoke();
            }

            __BuildingList.Remove(_BuildingToRemove);
            BuildingListChanged?.Invoke();
            return true;
        }

        public bool TryChangeActiveBuilding(Building _NewActiveBuilding, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (!__BuildingList.Contains(_NewActiveBuilding))
            {
                _ErrorMessage = "New Active Building Must Exist In The Building List";
                return false;
            }

            __ActiveBuilding = _NewActiveBuilding;
            ActiveBuildingChanged?.Invoke();
            return true;
        }
        public void CopyBuilding(Building _BuildingToCopy)
        {
            //stub
        }

        private bool BuildingNameSystemValidation(string _BuildingName, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            if (__BuildingList.Any(CurrentBuilding => CurrentBuilding.Name == _BuildingName))
            {
                _ErrorMessage += $"System Validation Error: {_BuildingName} Already Exists. No Duplicate Building Names.\n";
                SystemValid = false;
            }

            return SystemValid;
        }
    }
}
