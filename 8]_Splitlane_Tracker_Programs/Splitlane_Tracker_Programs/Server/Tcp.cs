using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Server
{
    #region Ignore
    [System.ComponentModel.DesignerCategory("")]
    public class qwerty { }
    #endregion
    public partial class GUI : Form
    {
        private static Services.Tcp.TcpServer TcpServer =
            new Services.Tcp.TcpServer(
                "localhost",
                8080,
                Environment.CurrentDirectory +
                "\\RaceDisplayPage.html", log);

        private void UpdateDisplays()
        {
            string info = myRace.GetDisplayInfoAsJson();
            TcpServer.Send(info);
        }
    }
}
