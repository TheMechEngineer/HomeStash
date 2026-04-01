namespace BackEnd.DataContinuity
{
    /// <summary>
    /// Data Transfer Object Representing Storage And Its Associated Data
    /// </summary>
    internal class StorageDTO
    {
        /// <summary>
        /// The List Of Items Contained In The Storage
        /// </summary>
        public required List<ItemDTO> StoredItems { get; set; }
    }
}
