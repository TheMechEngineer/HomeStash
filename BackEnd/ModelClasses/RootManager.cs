using System.Text.Json.Serialization;

namespace BackEnd.ModelClasses
{
    /// <summary>
    /// Represents The Root Manager That Contains Users,
    /// And Provides Validation And Event Notification For Changes
    /// </summary>
    public class RootManager
    {
        /// <summary>
        /// Event Triggered When The Active User Changes
        /// </summary>
        public event Action? ActiveUserChanged;

        /// <summary>
        /// Event Triggered When The User List Changes
        /// </summary>
        public event Action? UserListChanged;

        /// <summary>
        /// The Current Active User Backing Field For The Root Manager
        /// </summary>
        private User? __ActiveUser;

        /// <summary>
        /// The Current Active User For The Root Manager
        /// </summary>
        [JsonIgnore] // Excludes Active User From JSON As This Is Not Needed For Long Term Storage
        public User? ActiveUser
        {
            get
            { return __ActiveUser; }
        }

        /// <summary>
        /// Internal List Of Users For The Root Manager
        /// </summary>
        private List<User> __UserList = new List<User>();

        /// <summary>
        /// The Read-Only List Of Users For The Root Manager
        /// </summary>
        public IReadOnlyList<User> UserList
        {
            get
            { return __UserList.AsReadOnly(); }
        }

        /// <summary>
        /// Default Constructor For RootManager
        /// </summary>
        public RootManager()
        { }

        /// <summary>
        /// Attempts To Add A User To The Root Manager With Validation
        /// </summary>
        /// <param name="_Username">The Proposed Name Of The User</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryAddUser(string _Username, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool AddUserSuccess = true;

            // Runs Username System-Validation
            if (!UsernameSystemValidation(_Username, ref _ErrorMessage))
            {
                AddUserSuccess = false;
            }

            // If User Passes System-Validation Attempt To Create The User
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

        /// <summary>
        /// Attempts To Modify A User With Validation
        /// </summary>
        /// <param name="_UserToModify">The User Instance To Attempt To Modify</param>
        /// <param name="_NewUsername">The Proposed Name Of The User</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryModifyUser(User _UserToModify, string _NewUsername, out string? _ErrorMessage)
        {
            _ErrorMessage = null;
            bool ModifyUserSuccess = true;

            bool UsernameChanged = _UserToModify.Username != _NewUsername;

            // Checks If Any Fields Have Been Modified
            if (UsernameChanged) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
            {
                // Runs Username System-Validation If Username Was Changed
                if (UsernameChanged) //This Is Redundant In This Class, But Follows The Structure In The Other Classes Where It Makes Sense
                {
                    if (!UsernameSystemValidation(_NewUsername, ref _ErrorMessage))
                    {
                        ModifyUserSuccess = false;
                    }
                }

                // If User Passes System-Validation Attempt To Modify The User
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

        /// <summary>
        /// Attempts To Remove A User From The Root Manager
        /// </summary>
        /// <param name="_UserToRemove">The User Instance To Attempt To Remove</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryRemoveUser(User _UserToRemove, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            // Validates The User To Remove Exists Within The Root Manager
            if (!__UserList.Contains(_UserToRemove))
            {
                _ErrorMessage = "User To Remove Must Exist In The User List";
                return false;
            }

            // If User To Remove Is The Active User, Clear Active User
            if (_UserToRemove == __ActiveUser)
            {
                __ActiveUser = null;
                ActiveUserChanged?.Invoke();
            }

            // Removes User And Triggers Event
            __UserList.Remove(_UserToRemove);
            UserListChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Attempts To Change The Active User
        /// </summary>
        /// <param name="_NewActiveUser">The Proposed New Active User</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryChangeActiveUser(User _NewActiveUser, out string? _ErrorMessage)
        {
            _ErrorMessage = null;

            // Validates The New Active User Exists Within The Root Manager
            if (!__UserList.Contains(_NewActiveUser))
            {
                _ErrorMessage = "New Active User Must Exist In The User List";
                return false;
            }

            __ActiveUser = _NewActiveUser;
            ActiveUserChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Validates Username System Requirements
        /// </summary>
        /// <param name="_Username">The Proposed UserName To Validate</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        private bool UsernameSystemValidation(string _Username, ref string? _ErrorMessage)
        {
            bool SystemValid = true;

            // Validates The Username Entered Is Not A Duplicate
            if (__UserList.Any(CurrentUser => CurrentUser.Username == _Username))
            {
                _ErrorMessage += $"System Validation Error: {_Username} Already Exists. No Duplicate Usernames.\n";
                SystemValid = false;
            }

            return SystemValid;
        }
    }
}