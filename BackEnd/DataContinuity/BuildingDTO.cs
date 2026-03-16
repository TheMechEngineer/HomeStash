using BackEnd.ModelClasses;

namespace BackEnd.DataContinuity
{
    internal class BuildingDTO
    {
        public required string Name { get; set; }
        public required float Width { get; set; }
        public required float Height { get; set; }
        public required StorageDTO CurrentStorage { get; set; }
        public List<RoomDTO>? RoomList {  get; set; }
    }
}
