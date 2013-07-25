using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XDevkit;
using HaloDevelopmentExtender;
using HaloReach3d;
using System.Runtime.InteropServices;
using Gma.UserActivityMonitor;
using ExtensionMethods;

namespace XboxRemote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        XboxManager Xbox_Manager;
        XboxConsole Xbox_Console;
        IXboxAutomation Xbox_Automation;

        Keys w = Keys.W;
        Keys a = Keys.A;
        Keys s = Keys.S;
        Keys d = Keys.D;

        MouseButtons rightTrigger = MouseButtons.Left;
        MouseButtons leftTrigger = MouseButtons.Right;


        private void Form1_Load(object sender, EventArgs e)
        {
            Xbox_Manager = (XboxManager)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342")));
            xdkNameBox.Text = Xbox_Manager.DefaultConsole;
            
        }

        Timer timer1 = new Timer();

        private void monitorButton_Click(object sender, EventArgs e)
        {
            Xbox_Console = Xbox_Manager.OpenConsole(xdkNameBox.Text);
            Xbox_Automation = Xbox_Console.XboxAutomation;
            Xbox_Automation.BindController(0, 0);
            Xbox_Automation.ConnectController(0);

            
            timer1.Tick += new EventHandler(setGamepad);
            timer1.Interval = 10; // in miliseconds
            timer1.Start();

            HookManager.MouseUp += HookManager_MouseUp;
            HookManager.MouseDown += HookManager_MouseDown;
            HookManager.KeyDown += HookManager_KeyDown;
            HookManager.KeyUp += HookManager_KeyUp;
            HookManager.MouseMoveExt += HookManager_MouseMove;
        }

        private void setGamepad(object sender, EventArgs e)
        {
            if (Cursor.Position.X < 100 || Cursor.Position.X > 200 || Cursor.Position.Y < 100 || Cursor.Position.Y > 200) // Box left
            {
                Cursor.Position = new Point(150, 150);
            }
            Xbox_Automation.SetGamepadState(0, ref gamepad);
        }

        XBOX_AUTOMATION_GAMEPAD gamepad = new XBOX_AUTOMATION_GAMEPAD();

        private void settingsButton_Click(object sender, EventArgs e)
        {
            Xbox_Automation.SetGamepadState(0, ref gamepad);
        }

        //##################################################################
        #region Event handlers of particular events

        ButtonList buttons = new ButtonList();

        private void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (Button button in buttons.Buttons)
            {
                if (e.KeyCode == button.Key)
                {
                    gamepad.Buttons = button.XboxButton;
                    textBoxLog.AppendText(string.Format("KeyDown - {0}\n", button.XboxButton.ToString()));
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                HookManager.MouseUp -= HookManager_MouseUp;
                HookManager.MouseDown -= HookManager_MouseDown;
                HookManager.KeyDown -= HookManager_KeyDown;
                HookManager.KeyUp -= HookManager_KeyUp;
                HookManager.MouseMoveExt -= HookManager_MouseMove;
                timer1.Stop();
            }
            if (e.KeyCode == w)
            {
                gamepad.LeftThumbY = int.MaxValue;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }
            if (e.KeyCode == a)
            {
                gamepad.LeftThumbX = int.MinValue;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }
            if (e.KeyCode == s)
            {
                gamepad.LeftThumbY = int.MinValue;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }
            if (e.KeyCode == d)
            {
                gamepad.LeftThumbX = int.MaxValue;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }
            textBoxLog.ScrollToCaret();
            Xbox_Automation.SetGamepadState(0, ref gamepad);
        }

        private void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (Button button in buttons.Buttons)
            {
                if (e.KeyCode == button.Key)
                {
                    gamepad.Buttons = new XBOX_AUTOMATION_GAMEPAD().Buttons;
                    textBoxLog.AppendText(string.Format("KeyUp - {0}\n", button.XboxButton.ToString()));
                }
            }

            if (e.KeyCode == w)
            {
                gamepad.LeftThumbY = 0;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }
            if (e.KeyCode == a)
            {
                gamepad.LeftThumbX = 0;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }
            if (e.KeyCode == s)
            {
                gamepad.LeftThumbY = 0;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }
            if (e.KeyCode == d)
            {
                gamepad.LeftThumbX = 0;
                textBoxLog.AppendText(string.Format("KeyDown - {0}\n", e.KeyCode));
            }

            textBoxLog.AppendText(string.Format("KeyUp - {0}\n", e.KeyCode));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBoxLog.AppendText(string.Format("KeyPress - {0}\n", e.KeyChar));
            textBoxLog.ScrollToCaret();
        }

        long startTime = DateTime.Now.Ticks;
        Point startPoint = new Point(0, 0);

        Point lastPos = new Point(0, 0);

        private void HookManager_MouseMove(object sender, MouseEventExtArgs e)
        {
            this.Focus();
            var endTime = DateTime.Now.Ticks;
            var endPoint = e.Location;

            var pixelDistance = Math.Sqrt(Math.Abs(startPoint.X - endPoint.X) ^ 2 + Math.Abs(startPoint.Y - endPoint.Y) ^ 2);
            var durationInSec = (endTime - startTime) / 10000000.0; // 1 ms = 10^4 Ticks
            double speed = 1;
            if (durationInSec != 0 && pixelDistance != 0)
                speed = pixelDistance / durationInSec;
            else
                speed = 1;
            // do something with the calculated speed
            startTime = endTime;
            startPoint = endPoint;

            int dx = e.X - lastPos.X;
            int dy = e.Y - lastPos.Y;

            Point dir = new Point();
           // if (Math.Abs(dx) > Math.Abs(dy))
            dir.X = (dx > 0) ? 1 : -1;
            dir.Y = (dy > 0) ? -1 : 1;
            if (e.Y == lastPos.Y)
            {
                dir.Y = 0;
            }
            if (e.X == lastPos.X)
            {
                dir.X = 0;
            }
            //else
                //direction = (dy > 0) ? "Down" : "Up";
            gamepad.RightThumbX = (dir.X * (int)speed * 100);
            gamepad.RightThumbY = (dir.Y * (int)speed * 100);

            //labelMousePosition.Text = dir.ToString();
            labelMousePosition.Text = string.Format("x={0:0000}; y={1:0000};", gamepad.RightThumbX, gamepad.RightThumbY);
            textBoxLog.AppendText("(" + dir.X + ", " + dir.Y + ") " + speed + "\n");

            lastPos = e.Location;
        }

        

        int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private void HookManager_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxLog.AppendText(string.Format("MouseClick - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == rightTrigger)
            {
                gamepad.RightTrigger = 0;
                textBoxLog.AppendText("MouseUp - RightTrigger\n");
            }
            if (e.Button == leftTrigger)
            {
                gamepad.LeftTrigger = 0;
                textBoxLog.AppendText("MouseUp - LeftTrigger\n");
            }
            //textBoxLog.AppendText(string.Format("MouseUp - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
            Xbox_Automation.SetGamepadState(0, ref gamepad);
        }


        private void HookManager_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == rightTrigger)
            {
                gamepad.RightTrigger = 1023;
                textBoxLog.AppendText("MouseDown - RightTrigger\n");
            }
            if (e.Button == leftTrigger)
            {
                gamepad.LeftTrigger = 1023;
                textBoxLog.AppendText("MouseDown - LeftTrigger\n");
            }
           // textBoxLog.AppendText(string.Format("MouseDown - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
            Xbox_Automation.SetGamepadState(1, ref gamepad);
        }


        private void HookManager_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxLog.AppendText(string.Format("MouseDoubleClick - {0}\n", e.Button));
            textBoxLog.ScrollToCaret();
        }


        private void HookManager_MouseWheel(object sender, MouseEventArgs e)
        {
            //labelWheel.Text = string.Format("Wheel={0:000}", e.Delta);
        }

        #endregion

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Xbox_Automation.BindController(0, 0);
            Xbox_Automation.UnbindController(0);
        }
    }
}
