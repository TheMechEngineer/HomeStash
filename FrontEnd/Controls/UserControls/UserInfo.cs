using BackEnd.ModelClasses;
using FrontEnd.Utilities;

namespace FrontEnd.UserControls
{
    /// <summary>
    /// UserControl That Handles Display And Interaction For Adding Or Modifying A User
    /// </summary>
    internal partial class UserInfo : UserControl
    {
        /// <summary>
        /// Event Triggered When The Confirm Button Is Clicked
        /// </summary>
        internal event Action<FormType, User?, UserInfo, string>? ConfirmClicked;

        /// <summary>
        /// Event Triggered When The Cancel Button Is Clicked
        /// </summary>
        internal event Action<UserInfo>? CancelClicked;

        /// <summary>
        /// The User Object Being Modified, Null If Adding A New User
        /// </summary>
        private User? CurrentUser;

        /// <summary>
        /// Indicates Whether The Form Is In Add Or Modify Mode
        /// </summary>
        private FormType CurrentFormType;

        /// <summary>
        /// Initializes The UserInfo Control In Add Mode
        /// </summary>
        internal UserInfo()
        {
            InitializeComponent();

            CurrentFormType = FormType.Add;
            InitializeVisuals();
        }

        /// <summary>
        /// Initializes The UserInfo Control In Modify Mode With The Provided User
        /// </summary>
        /// <param name="_UserToModify">The User Object To Modify</param>
        internal UserInfo(User _UserToModify)
        {
            InitializeComponent();

            CurrentFormType = FormType.Modify;
            CurrentUser = _UserToModify;

            InitializeVisuals();
        }

        /// <summary>
        /// Initializes Visual Elements Of The UserInfo Control Based On Form Mode
        /// </summary>
        private void InitializeVisuals()
        {
            if (CurrentFormType == FormType.Add)
            {
                lblTitle.Text = "Add New User";
            }
            else if (CurrentFormType == FormType.Modify)
            {
                lblTitle.Text = "Modify User";
                txtUserNameInput.Text = CurrentUser.Username;
            }
        }

        /// <summary>
        /// Handles UserInfo Load Event For Visualization That Requires Load First To Be Accurate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserInfo_Load(object sender, EventArgs e)
        {
            SizeForm();

            txtUserNameInput.Focus();
        }

        /// <summary>
        /// Sizes And Positions Controls Within The Form
        /// </summary>
        private void SizeForm()
        {

            int Gap = 25;
            int SmallGap = 10;
            int ButtonHeight = 50;

            int LabelTextSizeDiff = txtUserNameInput.Height - lblUserName.Height;
            int MaxLabelSize = new int[] { lblUserName.Width }.Max();
            int TextBoxWidth = this.ClientSize.Width - 2 * Gap - MaxLabelSize;

            // Right Align All Labels That Are Not The Title Label. Based On The Longest Label
            foreach (Label CurrentLabel in this.Controls.OfType<Label>())
            {
                if (CurrentLabel == lblTitle)
                {
                    continue;
                }
                else
                {
                    CurrentLabel.Left = Gap + (MaxLabelSize - CurrentLabel.Width);
                }
            }

            // Center Title Label
            lblTitle.Left = this.ClientSize.Width / 2 - lblTitle.Width / 2;
            lblTitle.Top = SmallGap;

            // Position Username Label And TextBox
            lblUserName.Top = lblTitle.Bottom + SmallGap + LabelTextSizeDiff / 2;

            txtUserNameInput.Width = TextBoxWidth;
            txtUserNameInput.Left = this.ClientSize.Width - Gap - TextBoxWidth;
            txtUserNameInput.Top = lblTitle.Bottom + SmallGap;

            // Position Confirm Button
            btnConfirm.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnConfirm.Left = Gap;
            btnConfirm.Top = txtUserNameInput.Bottom + Gap;

            // Position Cancel Button
            btnCancel.Width = (this.ClientSize.Width - 2 * Gap - SmallGap) / 2;
            btnCancel.Left = this.ClientSize.Width - Gap - btnCancel.Width;
            btnCancel.Top = txtUserNameInput.Bottom + Gap;

            // Adjust User Control Height Based On Sized And Positioned Controls
            this.ClientSize = new Size(this.ClientSize.Width, btnConfirm.Bottom + Gap);
        }

        /// <summary>
        /// Handles Confirm Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ConfirmClicked?.Invoke(CurrentFormType, CurrentUser, this, txtUserNameInput.Text);
        }

        /// <summary>
        /// Handles Cancel Button Click Event
        /// </summary>
        /// <param name="sender">The Event Source</param>
        /// <param name="e">Event Arguments</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this);
        }
    }
}