﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using client.Classes;
using System.Diagnostics;
using System.IO;
using IWshRuntimeLibrary;

namespace client.User_controls
{
    public partial class ucShortcut : UserControl
    {
        public ProgramShortcut Psc { get; set; }
        public frmMain MotherForm { get; set; }
        public Category ThisCategory { get; set; }

        public ucShortcut()
        {
            InitializeComponent();
        }

        private void ucShortcut_Load(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
            AppHandler a = new AppHandler(Psc.FilePath, Psc.Arguments, Psc.ProcessName, Psc.WindowContainsText);
            if (a.IsAppRunning())
            {
                this.BackColor = Color.ForestGreen;
            }
            else
            {
                this.BackColor = MotherForm.BackColor;
            }
            
            picIcon.BackgroundImage = ThisCategory.loadImageCache(Psc); // Use the local icon cache for the file specified as the icon image
        }

        public void ucShortcut_Click(object sender, EventArgs e)
        {
            frmMain.ActiveForm.WindowState = FormWindowState.Minimized;
            //MessageBox.Show(Psc.FilePath);
            if (Psc.isWindowsApp)
            {
                Process p = new Process() 
                {
                    StartInfo = new ProcessStartInfo()
                    { 
                        UseShellExecute = true, FileName = $@"shell:appsFolder\{Psc.FilePath}" 
                    } 
                };
                p.Start();
            }
            else
            {
                if (Path.GetExtension(Psc.FilePath).ToLower() == ".lnk" && Psc.FilePath == MainPath.exeString)

                {
                    if (Psc.ProcessName != "" && Psc.WindowContainsText != "")
                    {
                        AppHandler a = new AppHandler(Psc.FilePath, Psc.Arguments, Psc.ProcessName, Psc.WindowContainsText);
                        a.ShowApp();
                        a.HideTaskbarButton();
                    }
                    else
                    {
                        MotherForm.OpenFile(Psc.Arguments, Psc.FilePath, MainPath.path);
                    }
                }
                else
                {
                    if (Psc.ProcessName != "" && Psc.WindowContainsText != "")
                    {
                        AppHandler a = new AppHandler(Psc.FilePath, Psc.Arguments, Psc.ProcessName, Psc.WindowContainsText);
                        a.ShowApp();
                        a.HideTaskbarButton();
                    }
                    else
                    {
                        MotherForm.OpenFile(Psc.Arguments, Psc.FilePath, Psc.WorkingDirectory);
                    }
                }
            }
            try
            {
                
            }
            catch { }
        }

        public void ucShortcut_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = MotherForm.HoverColor;
        }

        public void ucShortcut_MouseLeave(object sender, EventArgs e)
        {
            AppHandler a = new AppHandler(Psc.FilePath, Psc.Arguments, Psc.ProcessName, Psc.WindowContainsText);
            if (a.IsAppRunning())
            {
                this.BackColor = Color.ForestGreen;
            }
            else
            {
                this.BackColor = Color.Transparent;
            }
        }
    }
}
