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

        private List<Building> __BuildingList = new List<Building>();

        private Building? __ActiveBuilding;

        public Building? ActiveBuilding
        {
            get
            { return __ActiveBuilding; }
        }

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

        internal bool TryModifyName(string _NewUsername, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifySuccess = true;

            if (!UsernameSelfValidation(_NewUsername, ref _ErrorMessage))
            {
                ModifySuccess = false;
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
                _ErrorMessage += "Username Must Contain Characters\n";
                UsernameValid = false;
            }

            return UsernameValid;
        }

        public bool TryAddBuilding(string _BuildingName, float _Width, float _Height, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddBuildingSuccess = true;

            if (!NewBuildingSystemValidation(_BuildingName, ref _ErrorMessage))
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

        private bool NewBuildingSystemValidation(string _BuildingName, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            if (__BuildingList.Any(CurrentBuilding => CurrentBuilding.Name == _BuildingName))
            {
                _ErrorMessage += $"{_BuildingName} Already Exists. No Duplicate Building Names.\n";
                SystemValid = false;
            }

            return SystemValid;
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

        public void CopyBuilding(Building _BuildingToCopy)
        {
            //stub
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
    }
}
