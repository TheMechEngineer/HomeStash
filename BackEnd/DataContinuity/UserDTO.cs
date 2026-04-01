namespace BackEnd.DataContinuity
{
    /// <summary>
    /// Data Transfer Object Representing A User And Its Associated Data
    /// </summary>
    internal class UserDTO
    {
        /// <summary>
        /// The Username Of The User
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// The List Of Buildings Contained In The User
        /// </summary>
        public required List<BuildingDTO> BuildingList { get; set; }
    }
}
