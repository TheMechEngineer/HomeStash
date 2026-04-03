using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;

namespace FrontEnd.Utilities
{
    /// <summary>
    /// Represents A Single Item In A ComboBox With Display Text And Associated Storage Holder
    /// </summary>
    internal class ComboBoxLineItem
    {
        /// <summary>
        /// The Storage Holder Associated With This ComboBox Item
        /// </summary>
        private IStorageHolder StorageHolder;

        /// <summary>
        /// Backing Field For Tag Property
        /// </summary>
        private IStorageHolder __Tag;

        /// <summary>
        /// Externally Accessible Tag Property Associated With The IStorageHolder
        /// </summary>
        internal IStorageHolder Tag
        {
            get { return __Tag; }
        }

        /// <summary>
        /// Backing Field For DisplayText Property
        /// </summary>
        private string __DisplayText;

        /// <summary>
        /// The Text Displayed In The ComboBox
        /// </summary>
        internal string DisplayText
        {
            get { return __DisplayText; }
        }

        /// <summary>
        /// Initializes A New ComboBoxLineItem With The Provided Storage Holder
        /// </summary>
        /// <param name="_StorageHolder">The Storage Holder To Represent</param>
        internal ComboBoxLineItem(IStorageHolder _StorageHolder)
        {
            this.StorageHolder = _StorageHolder;

            SetTag();
            SetDisplayText();
        }

        /// <summary>
        /// Sets The Tag Value Based On The Storage Holder
        /// </summary>
        private void SetTag()
        {
            this.__Tag = StorageHolder;
        }

        /// <summary>
        /// Sets The Display Text Based On The Type Of Storage Holder
        /// </summary>
        private void SetDisplayText()
        {
            Type SelectionType = StorageHolder.GetType();

            // Use Different Properties Of Different Source Objects, To Source The Display Text
            switch (SelectionType)
            {
                case Type CurrentType when SelectionType == typeof(Building):
                    this.__DisplayText = (this.StorageHolder as Building).Name;
                    break;
                case Type CurrentType when SelectionType == typeof(Room):
                    this.__DisplayText = (this.StorageHolder as Room).Name;
                    break;
                case Type CurrentType when SelectionType == typeof(Container):
                    this.__DisplayText = (this.StorageHolder as Container).Name;
                    break;
            }
        }

        /// <summary>
        /// Returns The Display Text For Rendering In The ComboBox
        /// </summary>
        /// <returns>The Display Text String</returns>
        public override string ToString()
        {
            return __DisplayText;
        }
    }
}