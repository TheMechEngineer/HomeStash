using BackEnd.ModelClasses;
using BackEnd.ModelInterfaces;

namespace FrontEnd.Utilities
{
    internal class ComboBoxLineItem
    {
        private IStorageHolder StorageHolder;

        private IStorageHolder __Tag;
        internal IStorageHolder Tag
        {
            get { return __Tag; }
        }

        private string __DisplayText;
        internal string DisplayText
        {
            get { return __DisplayText; }
        }

        internal ComboBoxLineItem(IStorageHolder _StorageHolder)
        {
            this.StorageHolder = _StorageHolder;

            SetTag();
            SetDisplayText();
        }

        private void SetTag()
        {
            this.__Tag = StorageHolder;
        }

        private void SetDisplayText()
        {
            Type SelectionType = StorageHolder.GetType();

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

        public override string ToString()
        {
            return __DisplayText;
        }
    }
}