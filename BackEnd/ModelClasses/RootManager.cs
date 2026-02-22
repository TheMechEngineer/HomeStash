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
            {
                return __ActiveUser;
            }

            set
            {
                __ActiveUser = value;
                ActiveUserChanged?.Invoke();
            }
        }

        public IReadOnlyList<User> UserList
        {
            get
            {
                return __UserList;
            }
        }

        public void AddUser(User _UserToAdd)
        {
            __UserList.Add(_UserToAdd);
            UserListChanged?.Invoke();
        }

        public void RemoveUser(User _UserToRemove)
        {
            __UserList.Remove(_UserToRemove);
            UserListChanged?.Invoke();
        }
    }
}
