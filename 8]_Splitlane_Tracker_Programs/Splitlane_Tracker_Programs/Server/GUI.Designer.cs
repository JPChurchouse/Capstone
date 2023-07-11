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
      labl_DetectorStatus = new Label();
      tableLayoutPanel1 = new TableLayoutPanel();
      label2 = new Label();
      label1 = new Label();
      label3 = new Label();
      labl_Title = new Label();
      pictureBox1 = new PictureBox();
      tableLayoutPanel2 = new TableLayoutPanel();
      tableLayoutPanel4 = new TableLayoutPanel();
      txtbx_Detection = new TextBox();
      tableLayoutPanel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
      tableLayoutPanel2.SuspendLayout();
      tableLayoutPanel4.SuspendLayout();
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
      labl_ServerStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      labl_ServerStatus.AutoSize = true;
      labl_ServerStatus.BackColor = Color.Transparent;
      labl_ServerStatus.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
      labl_ServerStatus.ForeColor = Color.White;
      labl_ServerStatus.Location = new Point(211, 12);
      labl_ServerStatus.Margin = new Padding(6);
      labl_ServerStatus.Name = "labl_ServerStatus";
      labl_ServerStatus.Size = new Size(188, 57);
      labl_ServerStatus.TabIndex = 0;
      labl_ServerStatus.Text = "Unavail.";
      labl_ServerStatus.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // labl_RaceStatus
      // 
      labl_RaceStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      labl_RaceStatus.AutoSize = true;
      labl_RaceStatus.BackColor = Color.Transparent;
      labl_RaceStatus.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
      labl_RaceStatus.ForeColor = Color.White;
      labl_RaceStatus.Location = new Point(211, 150);
      labl_RaceStatus.Margin = new Padding(6);
      labl_RaceStatus.Name = "labl_RaceStatus";
      labl_RaceStatus.Size = new Size(188, 60);
      labl_RaceStatus.TabIndex = 1;
      labl_RaceStatus.Text = "Unavail.";
      labl_RaceStatus.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // txtbx_Output
      // 
      txtbx_Output.BackColor = Color.Black;
      txtbx_Output.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
      txtbx_Output.ForeColor = Color.White;
      txtbx_Output.Location = new Point(4, 244);
      txtbx_Output.Margin = new Padding(4);
      txtbx_Output.Multiline = true;
      txtbx_Output.Name = "txtbx_Output";
      txtbx_Output.ReadOnly = true;
      txtbx_Output.Size = new Size(418, 350);
      txtbx_Output.TabIndex = 0;
      txtbx_Output.TabStop = false;
      // 
      // labl_DetectorStatus
      // 
      labl_DetectorStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      labl_DetectorStatus.AutoSize = true;
      labl_DetectorStatus.BackColor = Color.Transparent;
      labl_DetectorStatus.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
      labl_DetectorStatus.ForeColor = Color.White;
      labl_DetectorStatus.Location = new Point(211, 81);
      labl_DetectorStatus.Margin = new Padding(6);
      labl_DetectorStatus.Name = "labl_DetectorStatus";
      labl_DetectorStatus.Size = new Size(188, 57);
      labl_DetectorStatus.TabIndex = 2;
      labl_DetectorStatus.Text = "Unavail.";
      labl_DetectorStatus.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // tableLayoutPanel1
      // 
      tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      tableLayoutPanel1.BackColor = Color.Transparent;
      tableLayoutPanel1.ColumnCount = 2;
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
      tableLayoutPanel1.Controls.Add(label1, 0, 2);
      tableLayoutPanel1.Controls.Add(label2, 0, 0);
      tableLayoutPanel1.Controls.Add(label3, 0, 1);
      tableLayoutPanel1.Controls.Add(labl_DetectorStatus, 1, 1);
      tableLayoutPanel1.Controls.Add(labl_RaceStatus, 1, 2);
      tableLayoutPanel1.Controls.Add(labl_ServerStatus, 1, 0);
      tableLayoutPanel1.ForeColor = Color.White;
      tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
      tableLayoutPanel1.Location = new Point(437, 9);
      tableLayoutPanel1.Margin = new Padding(9);
      tableLayoutPanel1.Name = "tableLayoutPanel1";
      tableLayoutPanel1.Padding = new Padding(6);
      tableLayoutPanel1.RowCount = 3;
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
      tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
      tableLayoutPanel1.Size = new Size(411, 222);
      tableLayoutPanel1.TabIndex = 3;
      // 
      // label2
      // 
      label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      label2.AutoSize = true;
      label2.BackColor = Color.Transparent;
      label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
      label2.ForeColor = Color.White;
      label2.Location = new Point(12, 12);
      label2.Margin = new Padding(6);
      label2.Name = "label2";
      label2.Size = new Size(187, 57);
      label2.TabIndex = 4;
      label2.Text = "Server:";
      label2.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // label1
      // 
      label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      label1.AutoSize = true;
      label1.BackColor = Color.Transparent;
      label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
      label1.ForeColor = Color.White;
      label1.Location = new Point(12, 150);
      label1.Margin = new Padding(6);
      label1.Name = "label1";
      label1.Size = new Size(187, 60);
      label1.TabIndex = 3;
      label1.Text = "Race:";
      label1.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // label3
      // 
      label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      label3.AutoSize = true;
      label3.BackColor = Color.Transparent;
      label3.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
      label3.ForeColor = Color.White;
      label3.Location = new Point(12, 81);
      label3.Margin = new Padding(6);
      label3.Name = "label3";
      label3.Size = new Size(187, 57);
      label3.TabIndex = 5;
      label3.Text = "Detection:";
      label3.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // labl_Title
      // 
      labl_Title.BackColor = Color.Transparent;
      labl_Title.Font = new Font("Segoe UI Semibold", 17F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
      labl_Title.ForeColor = Color.White;
      labl_Title.Location = new Point(4, 160);
      labl_Title.Margin = new Padding(4, 0, 4, 0);
      labl_Title.Name = "labl_Title";
      labl_Title.Size = new Size(411, 57);
      labl_Title.TabIndex = 4;
      labl_Title.Text = "Splitlane Tracking System";
      labl_Title.TextAlign = ContentAlignment.MiddleCenter;
      labl_Title.Click += labl_Title_Click;
      // 
      // pictureBox1
      // 
      pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      pictureBox1.BackColor = Color.Transparent;
      pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
      pictureBox1.Image = Properties.Resources.Logo_XKarts_Main;
      pictureBox1.Location = new Point(4, 4);
      pictureBox1.Margin = new Padding(4);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(412, 152);
      pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
      pictureBox1.TabIndex = 5;
      pictureBox1.TabStop = false;
      pictureBox1.Click += pictureBox1_Click;
      // 
      // tableLayoutPanel2
      // 
      tableLayoutPanel2.ColumnCount = 2;
      tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
      tableLayoutPanel2.Controls.Add(tableLayoutPanel4, 0, 0);
      tableLayoutPanel2.Controls.Add(tableLayoutPanel1, 1, 0);
      tableLayoutPanel2.Controls.Add(txtbx_Output, 0, 1);
      tableLayoutPanel2.Controls.Add(txtbx_Detection, 1, 1);
      tableLayoutPanel2.Location = new Point(0, 0);
      tableLayoutPanel2.Margin = new Padding(4);
      tableLayoutPanel2.Name = "tableLayoutPanel2";
      tableLayoutPanel2.RowCount = 2;
      tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
      tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
      tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
      tableLayoutPanel2.Size = new Size(857, 600);
      tableLayoutPanel2.TabIndex = 6;
      // 
      // tableLayoutPanel4
      // 
      tableLayoutPanel4.ColumnCount = 1;
      tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
      tableLayoutPanel4.Controls.Add(pictureBox1, 0, 0);
      tableLayoutPanel4.Controls.Add(labl_Title, 0, 1);
      tableLayoutPanel4.Location = new Point(4, 4);
      tableLayoutPanel4.Margin = new Padding(4);
      tableLayoutPanel4.Name = "tableLayoutPanel4";
      tableLayoutPanel4.RowCount = 2;
      tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
      tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
      tableLayoutPanel4.Size = new Size(420, 229);
      tableLayoutPanel4.TabIndex = 7;
      // 
      // txtbx_Detection
      // 
      txtbx_Detection.BackColor = Color.Black;
      txtbx_Detection.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
      txtbx_Detection.ForeColor = Color.White;
      txtbx_Detection.Location = new Point(432, 244);
      txtbx_Detection.Margin = new Padding(4);
      txtbx_Detection.Multiline = true;
      txtbx_Detection.Name = "txtbx_Detection";
      txtbx_Detection.ReadOnly = true;
      txtbx_Detection.Size = new Size(418, 350);
      txtbx_Detection.TabIndex = 8;
      txtbx_Detection.TabStop = false;
      // 
      // GUI
      // 
      AutoScaleDimensions = new SizeF(10F, 25F);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      BackgroundImageLayout = ImageLayout.Zoom;
      ClientSize = new Size(856, 600);
      Controls.Add(tableLayoutPanel2);
      DoubleBuffered = true;
      FormBorderStyle = FormBorderStyle.FixedToolWindow;
      Icon = (Icon)resources.GetObject("$this.Icon");
      Margin = new Padding(4);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "GUI";
      SizeGripStyle = SizeGripStyle.Hide;
      Text = "Splitlane Tracker Server";
      FormClosing += ServerGui_FormClosing;
      FormClosed += ServerGui_FormClosed;
      tableLayoutPanel1.ResumeLayout(false);
      tableLayoutPanel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
      tableLayoutPanel2.ResumeLayout(false);
      tableLayoutPanel2.PerformLayout();
      tableLayoutPanel4.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private NotifyIcon StatusIcon;
    private Label labl_ServerStatus;
    private Label labl_RaceStatus;
    private TextBox txtbx_Output;
    private Label labl_DetectorStatus;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label2;
    private Label label3;
    private Label label1;
    private Label labl_Title;
    private PictureBox pictureBox1;
    private TableLayoutPanel tableLayoutPanel2;
    private TableLayoutPanel tableLayoutPanel4;
    private TextBox txtbx_Detection;
  }
}