using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDevkit;
using HaloDevelopmentExtender;
using HaloReach3d;
using System.Runtime.InteropServices;
using Gma.UserActivityMonitor;
using System.Windows.Forms;

namespace XboxRemote
{
    class Button
    {
        public Keys Key;
        public XboxAutomationButtonFlags XboxButton;

        public Button(Keys key, XboxAutomationButtonFlags button)
        {
            Key = key;
            XboxButton = button;
        }
    }
}
