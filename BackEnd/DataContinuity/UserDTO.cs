using BackEnd.ModelClasses;

namespace BackEnd.DataContinuity
{
    internal class UserDTO
    {
        public required string Username {  get; set; }
        public required List<BuildingDTO> BuildingList {  get; set; }
    }
}
