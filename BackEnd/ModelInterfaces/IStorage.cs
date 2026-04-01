using BackEnd.Enumerations;
using System.Text.Json.Serialization;

namespace BackEnd.ModelInterfaces
{
    /// <summary>
    /// Interface Defining Storage Behavior Storage Objects
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// The List Of Items Stored In The Storage
        /// </summary>
        [JsonInclude]
        internal IReadOnlyList<IStored> StoredItems { get; }

        /// <summary>
        /// Attempts To Add A New Item Or Container To The Storage
        /// </summary>
        /// <param name="_IStoredType">The Type Of The Proposed IStored</param>
        /// <param name="_StoredName">The Proposed Name Of The IStored</param>
        /// <param name="_Description">The Proposed Description Of The IStored</param>
        /// <param name="_Value">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_Quantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal bool TryAddIStored(StoredItemType _IStoredType, string _StoredName, string _Description, double _Value, int _Quantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Modify An Existing Item Or Container In The Storage
        /// </summary>
        /// <param name="_IStoredToModify">The IStored Instance To Attempt To Modify</param>
        /// <param name="_NewStoredName">The Proposed Name Of The IStored</param>
        /// <param name="_NewDescription">The Proposed Description Of The IStored</param>
        /// <param name="_NewValue">The Proposed Monetary Value Of The IStored</param>
        /// <param name="_NewQuantity">The Proposed Quantity Of The IStored</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal bool TryModifyIStored(IStored _IStoredToModify, string _NewStoredName, string _NewDescription, double _NewValue, int _NewQuantity, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Move An Item Or Container To Another Storage Holder
        /// </summary>
        /// <param name="_IStoredToMove">The IStored Instance To Attempt To Move</param>
        /// <param name="_Destination">The Proposed Destination Storage Holder</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal bool TryMoveIStored(IStored _IStoredToMove, IStorageHolder _Destination, out string? _ErrorMessage);

        /// <summary>
        /// Attempts To Remove An Item Or Container From The Storage
        /// </summary>
        /// <param name="_IStoredToRemove">The IStored Instance To Attempt To Remove</param>
        /// <param name="_ErrorMessage">The Error Message If Unsuccessful</param>
        /// <returns></returns>
        internal bool TryRemoveIStored(IStored _IStoredToRemove, out string? _ErrorMessage);

        /// <summary>
        /// Returns The Total Count Of Items In The Storage, Including Nested Containers
        /// </summary>
        internal int TotalItemCount();

        /// <summary>
        /// Returns The Total Value Of Items In The Storage, Including Nested Containers
        /// </summary>
        internal double TotalItemValue();
    }
}
