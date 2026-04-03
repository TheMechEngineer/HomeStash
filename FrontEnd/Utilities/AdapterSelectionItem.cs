namespace FrontEnd.Adapters
{
    /// <summary>
    /// Represents A Single Item In A Selection List With Display Text And Associated Value
    /// </summary>
    internal class AdapterSelectionItem
    {
        /// <summary>
        /// Gets The Text Displayed For This Selection Item
        /// </summary>
        internal string DisplayText { get; }

        /// <summary>
        /// Gets The Underlying Object Associated With This Selection Item
        /// </summary>
        internal object Value { get; }

        /// <summary>
        /// Initializes A New AdapterSelectionItem With Display Text And Value
        /// </summary>
        /// <param name="_DisplayText">The Text To Display In The Selection List</param>
        /// <param name="_Value">The Underlying Value Associated With The Display Text</param>
        internal AdapterSelectionItem(string _DisplayText, object _Value)
        {
            DisplayText = _DisplayText;
            Value = _Value;
        }
    }
}