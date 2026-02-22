using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModelClasses
{
    public class RootManager
    {
        private List<User> __UserList { get; set; } = new List<User>();

        public User ActiveUser { get; set; }

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
        }

        public void RemoveUser(User _UserToRemove)
        {
            __UserList.Remove(_UserToRemove);
        }
    }
}
