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
            SuspendLayout();
            // 
            // StatusIcon
            // 
            StatusIcon.Icon = (Icon)resources.GetObject("StatusIcon.Icon");
            StatusIcon.Text = "StatusIcon";
            StatusIcon.Visible = true;
            StatusIcon.MouseDoubleClick += StatusIcon_MouseDoubleClick;
            // 
            // ServerGui
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "ServerGui";
            Text = "Form1";
            FormClosing += ServerGui_FormClosing;
            FormClosed += ServerGui_FormClosed;
            ResumeLayout(false);
        }

        #endregion

        private NotifyIcon StatusIcon;
    }
}