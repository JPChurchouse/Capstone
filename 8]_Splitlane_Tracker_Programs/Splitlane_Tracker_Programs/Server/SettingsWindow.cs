using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SplitlaneTracker.Server
{
  public partial class Settings : Form
  {
    public Settings()
    {
      InitializeComponent();

      textbox_IpAddress.Text = Properties.Settings.Default.MqttBrokerAddress;
    }

    private void button_Commit_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.MqttBrokerAddress = textbox_IpAddress.Text;
      Properties.Settings.Default.Save();
      this.Close();
    }
  }
}
