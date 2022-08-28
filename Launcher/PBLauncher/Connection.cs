using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PointBlank.Launcher
{
    public partial class Connection : Form
    {
        private bool Flag = true;
        public WebClient Web = new WebClient();

        public Connection()
        {
            InitializeComponent();
            try
            {
                this.Web.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Web_DownloadCompleted);
                this.Label.Text = "กำลังเปิดโปรแกรม...";
            }
            catch
            {
                this.Label.Text = "การเชื่อมต่อล้มเหลว...";
                if (MessageBox.Show("การเชื่อมต่อกับเซิร์ฟเวอร์ล้มเหลว.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    base.Close();
                    base.Dispose();
                }
                this.Logger("# PBLauncher Status - " + "การเชื่อมต่อกับเซิร์ฟเวอร์ล้มเหลว.");
                this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            }
        }

        private void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;
                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    const string str = "PBLauncher";
                    foreach (Process process in Process.GetProcesses())
                    {
                        if (process.ProcessName.StartsWith(str))
                        {
                            process.Kill();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("กรุณาคลิกขวา Run as administrator! \n\n" + ex.ToString());
                }
            }
        }

        private bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void Connection_Load(object sender, EventArgs e)
        {
            AdminRelauncher();
            Process[] Processes = Process.GetProcesses();
            Process[] Processos = Process.GetProcessesByName("PBLauncher");
            Computer Computer = new Computer();
            if (!Computer.FileSystem.FileExists(Application.StartupPath + "\\PBLauncher.log"))
            {
                new StreamWriter(Application.StartupPath + "\\PBLauncher.log").Close();
            }
            this.Logger("");
            this.Logger("");
            this.Logger("");
            this.Logger("PBLauncher Start - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            this.Logger("## PBLauncher Ver " + Version.getVersion().ToString());

            if (Processos.Length > 1)
            {
                if (MessageBox.Show("ไม่สามารถเปิด PBLauncher ได้สองโปรแกรม", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    base.Close();
                    base.Dispose();
                }
                Flag = false;
            }
            for (int i = 0; i < Processes.Length; i++)
            {
                Process Process = Processes[i];
                bool PointBlank = Operators.CompareString(Process.ProcessName, "PointBlank", false) == 0;
                //bool BC = Operators.CompareString(Process.ProcessName, "BC", false) == 0;
                if (PointBlank)
                {
                    Process.Kill();
                    MessageBox.Show("ปิดเกมส์ก่อนเปิดตัวเข้าเกมส์!!", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                /*if (BC)
                {
                    Process.Kill();
                }*/
            }
            Check();
        }

        private void Web_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                base.ShowInTaskbar = false;
                base.Visible = false;
                MessageBox.Show("เกิดข้อผิดพลาดในการอ่านข้อมูล", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void Logger(string Text)
        {
            string Path = Application.StartupPath + "\\PBLauncher.log";
            DateTime Now = DateTime.Now;
            StreamWriter Writer = new StreamWriter(Path, true);
            Writer.WriteLine(Text);
            Writer.Flush();
            Writer.Close();
        }

        private void Check()
        {
            if (Flag)
            {
                try
                {
                    int num = int.Parse(this.Web.DownloadString(Modul.WEB + "launcher/status/status.txt"));
                    string text = this.Web.DownloadString(Modul.WEB + "launcher/status/text.txt");

                    if (num == 1)
                    {
                        this.Start.RunWorkerAsync();
                    }
                    else
                    {
                        switch (num)
                        {
                            case 0:
                                this.Label.Text = "ไม่สามารถเข้าเกมได้ในขณะนี้...";
                                if (MessageBox.Show("ไม่สามารถเข้าเกมได้ในขณะนี้.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                {
                                    base.Close();
                                    base.Dispose();
                                }
                                this.Logger("# PBLauncher Status - " + "ไม่สามารถเข้าเกมได้ในขณะนี้.");
                                this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                                return;
                            case 2:
                                this.Label.Text = "เซิร์ฟเวอร์ปิดปรับปรุง...";
                                if (MessageBox.Show(text.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                                {
                                    base.Close();
                                    base.Dispose();
                                }
                                this.Logger("# PBLauncher Status - " + text.ToString());
                                this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                                return;
                        }
                    }
                }
                catch
                {
                    this.Label.Text = "ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้...";
                    if (MessageBox.Show("ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {
                        base.Close();
                        base.Dispose();
                    }
                    this.Logger("# PBLauncher Status - " + "ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้.");
                    this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                }
            }
        }

        public void Start_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Label.Text = "กรุณารอซักครู่...";
            Thread.Sleep(900);
        }

        public void Start_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }
    }
}
