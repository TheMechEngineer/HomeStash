namespace BackEnd.DataContinuity
{
    /// <summary>
    /// Data Transfer Object Representing An Item And Its Associated Data
    /// </summary>
    internal class ItemDTO
    {
        /// <summary>
        /// The Name Of The Item
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The Description Of The Item
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The Monetary Value Of The Item
        /// </summary>
        public required double Value { get; set; }

        /// <summary>
        /// The Quantity Of The Item.
        /// </summary>
        public required int Quantity { get; set; }

        /// <summary>
        /// The Contents Stored In The Item (If It Is A Container)
        /// </summary>
        public StorageDTO? CurrentStorage { get; set; }
    }
}
