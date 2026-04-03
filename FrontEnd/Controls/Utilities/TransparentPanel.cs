using System.ComponentModel;

namespace FrontEnd.Controls.Utilities
{
    // Source - https://stackoverflow.com/a/32402532
    // Posted by Reza Aghaei, modified by community. See post 'Timeline' for change history
    // Retrieved 2026-03-02, License - CC BY-SA 4.0

    /// <summary>
    /// Custom Panel That Supports Adjustable Transparency By Overlaying A Semi-Transparent Background
    /// </summary>
    internal class TransparentPanel : Panel
    {
        /// <summary>
        /// Extended Window Style Flag That Enables Transparent Rendering Behavior
        /// </summary>
        private const int WS_EX_TRANSPARENT = 0x20;

        /// <summary>
        /// Initializes The TransparentPanel Control
        /// </summary>
        public TransparentPanel()
        {
            SetStyle(ControlStyles.Opaque, true);
        }

        /// <summary>
        /// Backing Field For Opacity Property
        /// </summary>
        private int opacity = 50;

        /// <summary>
        /// Gets Or Sets The Opacity Level Of The Panel As A Percentage From 0 To 100
        /// </summary>
        [DefaultValue(50)]
        public int Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("value must be between 0 and 100");
                opacity = value;
            }
        }

        /// <summary>
        /// Gets The CreateParams With Transparency Enabled For The Control
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;
                return cp;
            }
        }

        /// <summary>
        /// Handles Paint Event To Render A Semi-Transparent Background
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new SolidBrush(Color.FromArgb(opacity * 255 / 100, BackColor)))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
            base.OnPaint(e);
        }
    }
}