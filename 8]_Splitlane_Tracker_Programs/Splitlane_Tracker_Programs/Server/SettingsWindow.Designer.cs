namespace SplitlaneTracker.Server
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_Commit = new Button();
            textbox_IpAddress = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // button_Commit
            // 
            button_Commit.Location = new Point(75, 60);
            button_Commit.Name = "button_Commit";
            button_Commit.Size = new Size(75, 23);
            button_Commit.TabIndex = 0;
            button_Commit.Text = "Commit";
            button_Commit.UseVisualStyleBackColor = true;
            button_Commit.Click += button_Commit_Click;
            // 
            // textbox_IpAddress
            // 
            textbox_IpAddress.Location = new Point(12, 29);
            textbox_IpAddress.Name = "textbox_IpAddress";
            textbox_IpAddress.Size = new Size(138, 25);
            textbox_IpAddress.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(73, 17);
            label1.TabIndex = 2;
            label1.Text = "IP Address:";
            // 
            // Settings
            // 
            AcceptButton = button_Commit;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = button_Commit;
            ClientSize = new Size(162, 89);
            ControlBox = false;
            Controls.Add(label1);
            Controls.Add(textbox_IpAddress);
            Controls.Add(button_Commit);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Settings";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Settings";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_Commit;
        private TextBox textbox_IpAddress;
        private Label label1;
    }
}