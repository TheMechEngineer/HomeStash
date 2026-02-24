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

            set
            {
                __ActiveUser = value;
                ActiveUserChanged?.Invoke();
            }
        }

        public IReadOnlyList<User> UserList
        {
            get
            { return __UserList; }
        }

        public bool TryAddUser(string _Username, out string? _ErrorMessage)
        {
            if(__UserList.Any(CurrentUser => CurrentUser.UserName == _Username))
            {
                _ErrorMessage = $"{_Username} Already Exists. No Duplicate Usernames.";
                return false;
            }

            User? NewUser;

            if(!User.TryCreate(_Username,out NewUser,out _ErrorMessage))
            {
                return false;
            }

            __UserList.Add(NewUser);
            UserListChanged?.Invoke();
            return true;
        }

        public void RemoveUser(User _UserToRemove)
        {
            __UserList.Remove(_UserToRemove);
            UserListChanged?.Invoke();
        }
    }
}
