namespace BackEnd.DataContinuity
{
    /// <summary>
    /// Data Transfer Object Representing A Room And Its Associated Data
    /// </summary>
    internal class RoomDTO
    {
        /// <summary>
        /// The Name Of The Room
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The Width Of The Room
        /// </summary>
        public required float Width { get; set; }

        /// <summary>
        /// The Height Of The Room
        /// </summary>
        public required float Height { get; set; }

        /// <summary>
        /// The X Coordinate Of The Room Center
        /// </summary>
        public required float CenterX { get; set; }

        /// <summary>
        /// The Y Coordinate Of The Room Center
        /// </summary>
        public required float CenterY { get; set; }

        /// <summary>
        /// The Color Of The Room Stored As An ARGB Integer
        /// </summary>
        public required int RoomColor { get; set; }

        /// <summary>
        /// The Contents Stored In The Room
        /// </summary>
        public required StorageDTO CurrentStorage { get; set; }
    }
}
