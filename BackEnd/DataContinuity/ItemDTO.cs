namespace BackEnd.DataContinuity
{
    internal class ItemDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required double Value { get; set; }
        public required int Quantity { get; set; }
        public StorageDTO? CurrentStorage { get; set; }
    }
}
