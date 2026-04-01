namespace BackEnd.DataContinuity
{
    /// <summary>
    /// Data Transfer Object Representing The Root Manager And Its Associated Data
    /// </summary>
    internal class RootManagerDTO
    {
        /// <summary>
        /// List Of Users Contained Within The Root Manager
        /// </summary>
        public required List<UserDTO> UserList { get; set; }
    }
}
