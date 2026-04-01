using BackEnd.Enumerations;

namespace BackEnd.ModelInterfaces
{
    /// <summary>
    /// Interface Representing An Entity That Has Storage
    /// </summary>
    public interface IStorageHolder
    {
        /// <summary>
        /// The Name Of The Storage Holder
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// The Storage Associated With The Holder
        /// </summary>
        internal IStorage CurrentStorage { get; }

        /// <summary>
        /// The List Of Items Directly Stored By The Holder
        /// </summary>
        public IReadOnlyList<IStored> StoredItems { get; }

        /// <summary>
        /// Attempts To Add A New Item Or Container To The Holder
        /// </summary>
        /// <param name="_StoredType">The Type Of The Proposed IStored</param>
        /// <param name="_StoredName">The Proposed Name Of The IStored</param>
        /// <param name="_Description">The Proposed Description Of The IStored</param>
        /// <param name="_Value">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_Quantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryAddIStored(StoredItemType _StoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Modify An Existing Item Or Container Of The Holder
        /// </summary>
        /// <param name="_IStoredToModify">The IStored Instance To Attempt To Modify</param>
        /// <param name="_NewStoredName">The Proposed Name Of The IStored</param>
        /// <param name="_NewDescription">The Proposed Description Of The IStored</param>
        /// <param name="_NewValue">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_NewQuantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Move An Item Or Container To Another Storage Holder
        /// </summary>
        /// <param name="_ItemToMove">The IStored Instance To Attempt To Move</param>
        /// <param name="_Destination">The Proposed Destination Storage Holder</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryMoveIStored(IStored _ItemToMove, IStorageHolder _Destination, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Remove An Item Or Container From The Holder
        /// </summary>
        /// <param name="_StoredToRemove">The IStored Instance To Attempt To Remove</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        public bool TryRemoveIStored(IStored _StoredToRemove, out string? _ErrorMessage);

        /// <summary>
        /// Returns The Total Count Of Items In The Holder, Including Nested Containers
        /// </summary>
        public int TotalItemCount();

        /// <summary>
        /// Returns The Total Value Of Items In The Holder, Including Nested Containers
        /// </summary>
        public double TotalItemValue();
    }
}
