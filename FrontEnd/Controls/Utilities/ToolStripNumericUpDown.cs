using System.Windows.Forms.Design;

namespace FrontEnd.Controls.Utilities
{
    // Specifies That This Control Can Only Be Added To A ToolStrip In The Designer
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]

    /// <summary>
    /// ToolStrip Control That Hosts A NumericUpDown Control For Use Within A ToolStrip
    /// </summary>
    public class ToolStripNumericUpDown : ToolStripControlHost
    {
        /// <summary>
        /// Event Triggered When The NumericUpDown Value Changes
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Initializes The ToolStripNumericUpDown Control Using Numeric Up Down As A Base
        /// </summary>
        public ToolStripNumericUpDown() : base(new NumericUpDown()) { }

        /// <summary>
        /// Gets The Hosted NumericUpDown Control
        /// </summary>
        public NumericUpDown NumericUpDownControl
        {
            get
            {
                return Control as NumericUpDown;
            }
        }

        /// <summary>
        /// Gets Or Sets The Current Value Of The NumericUpDown Control
        /// </summary>
        public decimal Value
        {
            get { return NumericUpDownControl.Value; }
            set { NumericUpDownControl.Value = value; }
        }

        /// <summary>
        /// Gets Or Sets The Minimum Allowed Value Of The NumericUpDown Control
        /// </summary>
        public decimal Minimum
        {
            get { return NumericUpDownControl.Minimum; }
            set { NumericUpDownControl.Minimum = value; }
        }

        /// <summary>
        /// Gets Or Sets The Maximum Allowed Value Of The NumericUpDown Control
        /// </summary>
        public decimal Maximum
        {
            get { return NumericUpDownControl.Maximum; }
            set { NumericUpDownControl.Maximum = value; }
        }


        /// <summary>
        /// Subscribes To Control Events When The Hosted Control Is Initialized
        /// </summary>
        /// <param name="CurrentControl">The Control Being Subscribed To</param>
        protected override void OnSubscribeControlEvents(Control CurrentControl)
        {
            // Call the base so the base events are connected.
            base.OnSubscribeControlEvents(CurrentControl);

            // Add the event.
            NumericUpDownControl.ValueChanged += NumericUpDown_ValueChanged;
        }

        /// <summary>
        /// Unsubscribes From Control Events When The Hosted Control Is Disposed
        /// </summary>
        /// <param name="CurrentControl"></param>
        protected override void OnUnsubscribeControlEvents(Control CurrentControl)
        {
            // Call the base method so the basic events are unsubscribed.
            base.OnUnsubscribeControlEvents(CurrentControl);

            // Remove the event.
            NumericUpDownControl.ValueChanged -= NumericUpDown_ValueChanged;
        }
        /// <summary>
        /// Handles NumericUpDown Value Changed Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}