using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SplitlaneTracker.Server
{
    #region Ignore
    [System.ComponentModel.DesignerCategory("")]
    public class asdff { }
    #endregion
    public partial class GUI : Form
    {
        enum Status
        {
            Initalising,
            Available,
            Ready,
            Running,
            Error,
            Complete
        }
    }
}
