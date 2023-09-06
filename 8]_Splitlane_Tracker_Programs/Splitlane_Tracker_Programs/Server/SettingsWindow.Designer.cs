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
      label2 = new Label();
      tb_LapsLeft = new TextBox();
      tb_LapsRight = new TextBox();
      label3 = new Label();
      tb_LapsTotal = new TextBox();
      label4 = new Label();
      SuspendLayout();
      // 
      // button_Commit
      // 
      button_Commit.Location = new Point(155, 130);
      button_Commit.Name = "button_Commit";
      button_Commit.Size = new Size(75, 23);
      button_Commit.TabIndex = 0;
      button_Commit.Text = "Commit";
      button_Commit.UseVisualStyleBackColor = true;
      button_Commit.Click += button_Commit_Click;
      // 
      // textbox_IpAddress
      // 
      textbox_IpAddress.Location = new Point(92, 6);
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
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(12, 40);
      label2.Name = "label2";
      label2.Size = new Size(62, 17);
      label2.TabIndex = 3;
      label2.Text = "Laps.Left:";
      // 
      // tb_LapsLeft
      // 
      tb_LapsLeft.Location = new Point(92, 37);
      tb_LapsLeft.Name = "tb_LapsLeft";
      tb_LapsLeft.Size = new Size(100, 25);
      tb_LapsLeft.TabIndex = 4;
      // 
      // tb_LapsRight
      // 
      tb_LapsRight.Location = new Point(92, 68);
      tb_LapsRight.Name = "tb_LapsRight";
      tb_LapsRight.Size = new Size(100, 25);
      tb_LapsRight.TabIndex = 6;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(12, 71);
      label3.Name = "label3";
      label3.Size = new Size(71, 17);
      label3.TabIndex = 5;
      label3.Text = "Laps.Right:";
      // 
      // tb_LapsTotal
      // 
      tb_LapsTotal.Location = new Point(92, 99);
      tb_LapsTotal.Name = "tb_LapsTotal";
      tb_LapsTotal.Size = new Size(100, 25);
      tb_LapsTotal.TabIndex = 8;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(12, 102);
      label4.Name = "label4";
      label4.Size = new Size(69, 17);
      label4.TabIndex = 7;
      label4.Text = "Laps.Total:";
      // 
      // Settings
      // 
      AcceptButton = button_Commit;
      AutoScaleDimensions = new SizeF(7F, 17F);
      AutoScaleMode = AutoScaleMode.Font;
      CancelButton = button_Commit;
      ClientSize = new Size(247, 165);
      ControlBox = false;
      Controls.Add(tb_LapsTotal);
      Controls.Add(label4);
      Controls.Add(tb_LapsRight);
      Controls.Add(label3);
      Controls.Add(tb_LapsLeft);
      Controls.Add(label2);
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
    private Label label2;
    private TextBox tb_LapsLeft;
    private TextBox tb_LapsRight;
    private Label label3;
    private TextBox tb_LapsTotal;
    private Label label4;
  }
}