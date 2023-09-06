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
      tb_LapsLeft.Text = Properties.Settings.Default.Laps_Left.ToString();
      tb_LapsRight.Text = Properties.Settings.Default.Laps_Right.ToString();
      tb_LapsTotal.Text = Properties.Settings.Default.Laps_Total.ToString();
    }

    private void button_Commit_Click(object sender, EventArgs e)
    {
      int L = Properties.Settings.Default.Laps_Left;
      int R = Properties.Settings.Default.Laps_Right;
      int T = Properties.Settings.Default.Laps_Total;

      if (Int32.TryParse(tb_LapsLeft.Text,  out int l) && l >= 0) L = l;
      if (Int32.TryParse(tb_LapsRight.Text, out int r) && r >= 0) R = r;
      if (Int32.TryParse(tb_LapsTotal.Text, out int t) && t >= 0) T = t; // A zero lap race? Really? 🤔
      T = T > L + R ? T : L + R;

      Properties.Settings.Default.Laps_Left  = L;
      Properties.Settings.Default.Laps_Right = R;
      Properties.Settings.Default.Laps_Total = T;

      Properties.Settings.Default.MqttBrokerAddress = textbox_IpAddress.Text;

      Properties.Settings.Default.Save();
      this.Close();
    }
  }
}
