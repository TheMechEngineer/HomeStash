using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class RootManager
    {
        public event Action? ActiveUserChanged;
        public event Action? UserListChanged;
        private List<User> __UserList = new List<User>();
        private User? __ActiveUser;

        public User? ActiveUser {
            get
            { return __ActiveUser; }
        }

        public IReadOnlyList<User> UserList
        {
            get
            { return __UserList.AsReadOnly(); }
        }

        public bool TryAddUser(string _Username, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddUserSuccess = true;

            if (!UsernameSystemValidation(_Username, ref _ErrorMessage))
            {
                AddUserSuccess = false;
            }

            if (AddUserSuccess)
            {
                User? _NewUser;

                if (User.TryCreate(_Username, out _NewUser, out _ErrorMessage))
                {
                    __UserList.Add(_NewUser);
                    UserListChanged?.Invoke(); 
                }
                else
                {
                    AddUserSuccess = false;
                }

            }

            if (!AddUserSuccess)
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return AddUserSuccess;
        }

        public bool TryModifyUser(User _UserToModify, string _NewUsername, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyUserSuccess = true;

            if (_UserToModify.Username != _NewUsername) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
            {
                if (_UserToModify.Username != _NewUsername) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
                {
                    if (!UsernameSystemValidation(_NewUsername, ref _ErrorMessage))
                    {
                        ModifyUserSuccess = false;
                    }
                }

                if (ModifyUserSuccess)
                {
                    if (_UserToModify.TryModify(_NewUsername, out _ErrorMessage))
                    {
                        UserListChanged?.Invoke();
                    }
                    else
                    {
                        ModifyUserSuccess = false;
                    }
                }
            }
            else
            {
                ModifyUserSuccess = false;
                _ErrorMessage += $"No User Fields Have Been Modified\n";
            }

            if (!ModifyUserSuccess)
            {
                _ErrorMessage = _ErrorMessage?.TrimEnd();
            }

            return ModifyUserSuccess;
        }

        private bool UsernameSystemValidation(string _Username, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            if (__UserList.Any(CurrentUser => CurrentUser.Username == _Username))
            {
                _ErrorMessage += $"{_Username} Already Exists. No Duplicate Usernames.\n";
                SystemValid = false;
            }

            return SystemValid;
        }

        public bool TryRemoveUser(User _UserToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (!__UserList.Contains(_UserToRemove))
            {
                _ErrorMessage = "User To Remove Must Exist In The User List";
                return false;
            }

            if (_UserToRemove == __ActiveUser) { 
                __ActiveUser = null;
                ActiveUserChanged?.Invoke();
            }

            __UserList.Remove(_UserToRemove);
            UserListChanged?.Invoke();
            return true;
        }

        public bool TryChangeActiveUser(User _NewActiveUser, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            if (!__UserList.Contains(_NewActiveUser))
            {
                _ErrorMessage = "New Active User Must Exist In The User List";
                return false;
            }

            __ActiveUser = _NewActiveUser;
            ActiveUserChanged?.Invoke();
            return true;
        }
    }
}
