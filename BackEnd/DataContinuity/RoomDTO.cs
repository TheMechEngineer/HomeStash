namespace BackEnd.DataContinuity
{
    internal class RoomDTO
    {
        public required string Name { get; set; }
        public required float Width { get; set; }
        public required float Height { get; set; }
        public required float CenterX { get; set; }
        public required float CenterY { get; set; }
        public required int RoomColor { get; set; }
        public required StorageDTO CurrentStorage { get; set; }
    }
}
