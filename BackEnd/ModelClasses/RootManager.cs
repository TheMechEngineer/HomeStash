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
        private List<User> __UserList { get; set; } = new List<User>();
        private User? __ActiveUser;

        public User? ActiveUser {
            get
            { return __ActiveUser; }
        }

        public IReadOnlyList<User> UserList
        {
            get
            { return __UserList; }
        }

        public bool TryAddUser(string _Username, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool CreationSuccess = true;

            if (__UserList.Any(CurrentUser => CurrentUser.Username == _Username))
            {
                _ErrorMessage = $"{_Username} Already Exists. No Duplicate Usernames.";
                CreationSuccess = false;
            }

            if (CreationSuccess)
            {
                User? _NewUser;

                if (User.TryCreate(_Username, out _NewUser, out _ErrorMessage))
                {
                    __UserList.Add(_NewUser);
                    UserListChanged?.Invoke(); 
                }
                else
                {
                    CreationSuccess = false;
                }

            }

            return CreationSuccess;
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
