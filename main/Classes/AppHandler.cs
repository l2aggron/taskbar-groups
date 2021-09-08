using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace client.Classes
{
    class AppHandler
    {
        private Process Process;
        private string ProcessName;
        private string TitleContainsText;

        [DllImport("user32.dll")]
        private static extern bool SwitchToThisWindow(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_EXSTYLE = -0x14;
        private const int WS_EX_TOOLWINDOW = 0x0080;

        public AppHandler(string pathToExecutable, string arguments, string processName, string titleContainsText)
        {
            Process = new Process();
            Process.StartInfo.FileName = pathToExecutable;
            if (!string.IsNullOrEmpty(arguments)) { Process.StartInfo.Arguments = arguments; }

            TitleContainsText = titleContainsText;
            ProcessName = processName;
        }

        public void ShowApp()
        {
            bool iSwindowFound = false;
            Array processesFound = Process.GetProcessesByName(ProcessName);
            if (processesFound.Length > 0)
            {
                foreach (Process process in processesFound)
                {
                    if (!iSwindowFound && process.MainWindowTitle.Contains(TitleContainsText))
                    {
                        //MessageBox.Show(ProcessName + TitleContainsText);
                        SwitchToThisWindow(process.MainWindowHandle, 1);
                        iSwindowFound = true;
                    }
                }
            }
            if (!iSwindowFound)
            {
                Process.Start();
                //HideTaskbarButton();
            }
        }

        public void HideTaskbarButton()
        {
            bool iSwindowFound = false;
            for (int i = 0; i < 100; i++)
            {
                Array processesFound = Process.GetProcessesByName(ProcessName);
                if (processesFound.Length > 0)
                {
                    foreach (Process process in processesFound)
                    {
                        if (process.MainWindowTitle.Contains(TitleContainsText))
                        {
                            SetWindowLong(process.MainWindowHandle, GWL_EXSTYLE, GetWindowLong(process.MainWindowHandle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
                            iSwindowFound = true;
                            break;
                        }
                    }
                }
                if (iSwindowFound) { break;}
                Thread.Sleep(100);
            }
        }

        public bool IsAppRunning()
        {
            Array processesFound = Process.GetProcessesByName(ProcessName);
            if (processesFound.Length > 0)
            {
                foreach (Process process in processesFound)
                {
                    if (process.MainWindowTitle.Contains(TitleContainsText))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void ShowTaskbarButton()
        {
            bool iSwindowFound = false;
            for (int i = 0; i < 100; i++)
            {
                Array processesFound = Process.GetProcessesByName(ProcessName);
                if (processesFound.Length > 0)
                {
                    foreach (Process process in processesFound)
                    {
                        if (process.MainWindowTitle.Contains(TitleContainsText))
                        {
                            SetWindowLong(process.MainWindowHandle, GWL_EXSTYLE, 0x272); //TODO replace 0x272 with bitwise calculation like in HideTaskbarButton
                            iSwindowFound = true;
                            break;
                        }
                    }
                }
                if (iSwindowFound) { break; }
                Thread.Sleep(100);
            }
        }
    }
}
