using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace FrontEnd.Utilities
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripNumericUpDown : ToolStripControlHost
    {
        public event EventHandler ValueChanged;

        public ToolStripNumericUpDown() : base(new NumericUpDown()) {}

        public NumericUpDown NumericUpDownControl
        {
            get
            {
                return this.Control as NumericUpDown;
            }
        }

        public decimal Value
        {
            get { return NumericUpDownControl.Value; }
            set { NumericUpDownControl.Value = value; }
        }

        public decimal Minimum
        {
            get { return NumericUpDownControl.Minimum; }
            set { NumericUpDownControl.Minimum = value; }
        }

        public decimal Maximum
        {
            get { return NumericUpDownControl.Maximum; }
            set { NumericUpDownControl.Maximum = value; }
        }

        protected override void OnSubscribeControlEvents(Control CurrentControl)
        {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(CurrentControl);

            // Add the event.
            this.NumericUpDownControl.ValueChanged += NumericUpDown_ValueChanged;
        }

        protected override void OnUnsubscribeControlEvents(Control CurrentControl)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(CurrentControl);

            // Remove the event.
            this.NumericUpDownControl.ValueChanged -= NumericUpDown_ValueChanged;
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }

}
