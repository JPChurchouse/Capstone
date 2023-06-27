namespace SplitlaneTracker.Server
{
    partial class GUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            StatusIcon = new NotifyIcon(components);
            labl_ServerStatus = new Label();
            labl_RaceStatus = new Label();
            txtbx_Output = new TextBox();
            SuspendLayout();
            // 
            // StatusIcon
            // 
            StatusIcon.BalloonTipText = "Splitlane Tracker Server";
            StatusIcon.BalloonTipTitle = "Initalising...";
            StatusIcon.Icon = (Icon)resources.GetObject("StatusIcon.Icon");
            StatusIcon.Text = "Initalising...";
            StatusIcon.Visible = true;
            StatusIcon.MouseDoubleClick += StatusIcon_MouseDoubleClick;
            // 
            // labl_ServerStatus
            // 
            labl_ServerStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labl_ServerStatus.AutoSize = true;
            labl_ServerStatus.BackColor = Color.Transparent;
            labl_ServerStatus.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            labl_ServerStatus.ForeColor = Color.White;
            labl_ServerStatus.Location = new Point(13, 13);
            labl_ServerStatus.Margin = new Padding(4);
            labl_ServerStatus.Name = "labl_ServerStatus";
            labl_ServerStatus.Size = new Size(139, 25);
            labl_ServerStatus.TabIndex = 0;
            labl_ServerStatus.Text = "Server: Online";
            // 
            // labl_RaceStatus
            // 
            labl_RaceStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labl_RaceStatus.AutoSize = true;
            labl_RaceStatus.BackColor = Color.Transparent;
            labl_RaceStatus.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            labl_RaceStatus.ForeColor = Color.White;
            labl_RaceStatus.Location = new Point(13, 46);
            labl_RaceStatus.Margin = new Padding(4);
            labl_RaceStatus.Name = "labl_RaceStatus";
            labl_RaceStatus.Size = new Size(166, 25);
            labl_RaceStatus.TabIndex = 1;
            labl_RaceStatus.Text = "Race: Unavailable";
            // 
            // txtbx_Output
            // 
            txtbx_Output.BackColor = Color.Black;
            txtbx_Output.Dock = DockStyle.Bottom;
            txtbx_Output.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtbx_Output.ForeColor = Color.White;
            txtbx_Output.Location = new Point(0, 78);
            txtbx_Output.Multiline = true;
            txtbx_Output.Name = "txtbx_Output";
            txtbx_Output.ReadOnly = true;
            txtbx_Output.Size = new Size(264, 213);
            txtbx_Output.TabIndex = 0;
            txtbx_Output.TabStop = false;
            // 
            // GUI
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(264, 291);
            Controls.Add(txtbx_Output);
            Controls.Add(labl_RaceStatus);
            Controls.Add(labl_ServerStatus);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GUI";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Splitlane Tracker Server";
            FormClosing += ServerGui_FormClosing;
            FormClosed += ServerGui_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon StatusIcon;
        private Label labl_ServerStatus;
        private Label labl_RaceStatus;
        private TextBox txtbx_Output;
    }
}