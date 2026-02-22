using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Adapters
{
    internal class ASelection<T>
    {
        public string DisplayText { get; }
        public T Value { get; }

        public ASelection(string _DisplayText, T _Value)
        {
            DisplayText = _DisplayText;
            Value = _Value;
        }

    }
}
