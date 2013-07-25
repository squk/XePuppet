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
    class ButtonList
    {
        private List<Button> myButtons = new List<Button>();

        public ButtonList()
        {
            myButtons.Add(new Button(Keys.Space, XboxAutomationButtonFlags.A_Button));
            myButtons.Add(new Button(Keys.E, XboxAutomationButtonFlags.X_Button));
            myButtons.Add(new Button(Keys.F, XboxAutomationButtonFlags.B_Button));
            myButtons.Add(new Button(Keys.Q, XboxAutomationButtonFlags.Y_Button));

            //myButtons.Add(new Button(Keys., XboxAutomationButtonFlags.Y_Button));

            //myButtons.Add(new Button(Keys.Enter, XboxAutomationButtonFlags.StartButton));
        }

        public List<Button> Buttons
        {
            get { return myButtons; }
        }
    }
}
