namespace BackEnd.DataContinuity
{
    /// <summary>
    /// Data Transfer Object Representing A Building And Its Associated Data.
    /// </summary>
    internal class BuildingDTO
    {
        /// <summary>
        /// The Name Of The Building
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The Width Of The Building
        /// </summary>
        public required float Width { get; set; }

        /// <summary>
        /// The Height Of The Building
        /// </summary>
        public required float Height { get; set; }

        /// <summary>
        /// The Contents Stored In The Building
        /// </summary>
        public required StorageDTO CurrentStorage { get; set; }

        /// <summary>
        /// List Of Rooms Contained Within The Building. Null If No Room Data Is Provided
        /// </summary>
        public List<RoomDTO>? RoomList { get; set; }
    }
}
