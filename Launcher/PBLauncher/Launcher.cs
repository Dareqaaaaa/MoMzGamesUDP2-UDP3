using Ionic.Zip;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.FileIO;
using PointBlank.DLL;
using PointBlank.DLL.Modul;
using PointBlank.DLL.Xml;
using PointBlank.Launcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace PointBlank.Launcher
{
    public partial class Launcher : Form
    {
        public Connection Connection;
        public WebClient GameUpdate;
        public Computer Computer;
        public WebClient Web;
        private AuthModul AuthModul;
        private bool Value = true;
        string login = "", name = "", token = "";
        int rank, exp_;
        CookieContainer Cookie = null;

        long GetFileSize(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                return new FileInfo(FilePath).Length;
            }
            return 0;
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string dllName);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        delegate int MessageBoxDelegate(IntPtr hwnd,
            [MarshalAs(UnmanagedType.LPWStr)]string text,
            [MarshalAs(UnmanagedType.LPWStr)]string caption,
            int type);

        public Launcher(Connection Connection, AuthModul AuthModul)
        {
            this.GameUpdate = new WebClient();
            this.Computer = new Computer();
            this.Web = new WebClient();
            this.Connection = Connection;
            this.AuthModul = AuthModul;
            InitializeComponent();
            this.GameUpdate.DownloadFileCompleted += new AsyncCompletedEventHandler(this.GameUpdate_DownloadCompleted);
            this.GameUpdate.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.GameUpdate_DownloadProgressChanged);
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

        public void Launcher_Load(object sender, EventArgs e)
        {
            Cookie = new CookieContainer(); // important
            this.WebBrowser.Visible = true;
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (Value == false)
            {
                Environment.Exit(0);
            }
            else
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult Result = MessageBox.Show("ต้องการจะปิดโปรแกรมใช่หรือไม่?", Modul.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (Result == DialogResult.Yes)
                    {
                        this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                        Environment.Exit(0);
                    }
                    else
                    {
                        e.Cancel = Value;
                    }
                }
                else
                {
                    e.Cancel = Value;
                }
            }
        }

        private void Launcher_MouseDown(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                this.NewPoint.X = base.Left - Control.MousePosition.X;
                this.NewPoint.Y = base.Top - Control.MousePosition.Y;
            }
        }

        private void Launcher_MouseMove(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                base.Left = this.NewPoint.X + Control.MousePosition.X;
                base.Top = this.NewPoint.Y + Control.MousePosition.Y;
            }
        }

        private void Closed_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void Closed_MouseLeave(object sender, EventArgs e)
        {
            this.Exit.Image = Resources.Closed_Normal;
            this.Exit.BackColor = System.Drawing.Color.Transparent;
        }

        private void Closed_MouseMove(object sender, EventArgs e)
        {
            this.Exit.Image = Resources.Closed_Over;
            this.Exit.BackColor = System.Drawing.Color.Transparent;
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }

        private void Minimize_MouseLeave(object sender, EventArgs e)
        {
            this.Minimize.BackgroundImage = Resources.Hide_Normal;
            this.Minimize.BackColor = System.Drawing.Color.Transparent;
        }

        private void Minimize_MouseMove(object sender, MouseEventArgs e)
        {
            this.Minimize.BackgroundImage = Resources.Hide_Over;
            this.Minimize.BackColor = System.Drawing.Color.Transparent;
        }

        private void Verif_Click(object sender, EventArgs e)
        {
            try
            {
                this.FileName.Enabled = true;
                this.FileName.Visible = true;

                int Button = int.Parse(this.GameUpdate.DownloadString(Modul.WEB + "launcher/status/button.txt"));
                string Check = this.GameUpdate.DownloadString(Modul.WEB + "launcher/status/check.txt");
                string Text = this.GameUpdate.DownloadString(Modul.WEB + "launcher/status/text.txt");

                if (Button == 1)
                {

                }
                else if (Button == 2)
                {
                    if (MessageBox.Show(Text.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                    else if (MessageBox.Show(Text.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                    this.Logger("# PBLauncher Status - " + Text.ToString());
                }
                else if (Button == 3)
                {
                    MessageBox.Show(Check.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        private void Check_MouseLeave(object sender, EventArgs e)
        {
            this.Check.BackColor = System.Drawing.Color.Transparent;
            this.Check.BackgroundImage = Resources.Check_Normal;
        }

        private void Check_MouseMove(object sender, MouseEventArgs e)
        {
            this.Check.BackColor = System.Drawing.Color.Transparent;
            this.Check.BackgroundImage = Resources.Check_Over;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            try
            {
                Value = false;
                int Button = int.Parse(this.GameUpdate.DownloadString(Modul.WEB + "launcher/status/button.txt"));
                if (Button == 1)
                {
                    if (File.Exists(Application.StartupPath))
                    {
                        if (File.Exists("PointBlank.exe"))
                        {
                            /*if (!File.Exists(Application.StartupPath + @"\BC.exe"))
                            {
                                MessageBox.Show("ไม่พบไฟล์ดังกล่าว.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (MessageBox.Show("กำลังดาวน์โหลดไฟล์ที่หายไป...", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    Web.DownloadFile(Modul.WEB + "launcher/file/BC.exe", "BC.exe");
                                }
                            }
                            else
                            {
                                Process.Start(Application.StartupPath + @"\BC.exe");
                            }*/
                            Process.Start(Application.StartupPath + @"\PointBlank.exe", " /GameID:GarenaPB /Token: " + this.token);
                            IntPtr userApi = LoadLibrary("Codex.dll");
                            this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                            base.Close();
                        }
                        else
                        {
                            Value = true;
                            MessageBox.Show("ไม่สามารถเริ่มเกมได้ เนื่องจากไม่มีไฟล์ PointBlank.exe", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        if (File.Exists("PointBlank.exe"))
                        {
                            /*if (!File.Exists(Application.StartupPath + @"\BC.exe"))
                            {
                                MessageBox.Show("ไม่พบไฟล์ดังกล่าว.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (MessageBox.Show("กำลังดาวน์โหลดไฟล์ที่หายไป...", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    Web.DownloadFile(Modul.WEB + "launcher/file/BC.exe", "BC.exe");
                                }
                            }
                            else
                            {
                                Process.Start(Application.StartupPath + @"\BC.exe");
                            }*/
                            Process.Start(Application.StartupPath + @"\PointBlank.exe", " /GameID:GarenaPB /Token: " + this.token);
                            IntPtr userApi = LoadLibrary("Codex.dll");
                            this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                            base.Close();
                        }
                        else
                        {
                            Value = true;
                            MessageBox.Show("ไม่สามารถเริ่มเกมได้ เนื่องจากไม่มีไฟล์ PointBlank.exe", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
                else if (Button == 2)
                {
                    Value = true;
                    string text = this.GameUpdate.DownloadString(Modul.WEB + "launcher/status/text.txt");

                    if (MessageBox.Show(text.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                    else if (MessageBox.Show(text.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                    this.Logger("# PBLauncher Status - " + text.ToString());
                }
                else if (Button == 3)
                {
                    if (File.Exists(Application.StartupPath))
                    {
                        if (File.Exists("PointBlank.exe"))
                        {
                            /*if (!File.Exists(Application.StartupPath + @"\BC.exe"))
                            {
                                MessageBox.Show("ไม่พบไฟล์ดังกล่าว.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (MessageBox.Show("กำลังดาวน์โหลดไฟล์ที่หายไป...", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    Web.DownloadFile(Modul.WEB + "launcher/file/BC.exe", "BC.exe");
                                }
                            }
                            else
                            {
                                Process.Start(Application.StartupPath + @"\BC.exe");
                            }*/
                            Process.Start(Application.StartupPath + @"\PointBlank.exe", " /GameID:GarenaPB /Token: " + this.token);
                            IntPtr userApi = LoadLibrary("Codex.dll");
                            this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                            base.Close();
                        }
                        else
                        {
                            Value = true;
                            MessageBox.Show("ไม่สามารถเริ่มเกมได้ เนื่องจากไม่มีไฟล์ PointBlank.exe", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        if (File.Exists("PointBlank.exe"))
                        {
                            /*if (!File.Exists(Application.StartupPath + @"\BC.exe"))
                            {
                                MessageBox.Show("ไม่พบไฟล์ดังกล่าว.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (MessageBox.Show("กำลังดาวน์โหลดไฟล์ที่หายไป...", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                                {
                                    Web.DownloadFile(Modul.WEB + "launcher/file/BC.exe", "BC.exe");
                                }
                            }
                            else
                            {
                                Process.Start(Application.StartupPath + @"\BC.exe");
                            }*/
                            Process.Start(Application.StartupPath + @"\PointBlank.exe", " /GameID:GarenaPB /Token: " + this.token);
                            IntPtr userApi = LoadLibrary("Codex.dll");
                            this.Logger("PBLauncher End - " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                            base.Close();
                        }
                        else
                        {
                            Value = true;
                            MessageBox.Show("ไม่สามารถเริ่มเกมได้ เนื่องจากไม่มีไฟล์ PointBlank.exe", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
                else if (Button == 4)
                {
                    Value = true;
                    string text = this.GameUpdate.DownloadString(Modul.WEB + "launcher/status/text.txt");

                    if (MessageBox.Show(text.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                    else if (MessageBox.Show(text.ToString(), Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                    {

                    }
                    this.Logger("# PBLauncher Status - " + text.ToString());
                }
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        private void btn_pbconfig_MouseLeave(object sender, EventArgs e)
        {
            this.btn_pbconfig.BackgroundImage = Resources.btn_pbconfig_v2;
            this.btn_pbconfig.BackColor = Color.Transparent;
        }

        private void btn_pbconfig_MouseMove(object sender, MouseEventArgs e)
        {
            this.btn_pbconfig.BackgroundImage = Resources.btn_pbconfig_v2_over;
            this.btn_pbconfig.BackColor = Color.Transparent;
        }   

        private void Start_MouseLeave(object sender, EventArgs e)
        {
            this.Start.BackgroundImage = Resources.btn_start_v2;
            this.Start.BackColor = Color.Transparent;
        }

        private void Start_MouseMove(object sender, MouseEventArgs e)
        {
            this.Start.BackgroundImage = Resources.btn_start_v2_over;
            this.Start.BackColor = Color.Transparent;
        }

        public void Bar1SetProgress(long received, long maximum, bool progress)
        {
            this.ArchiveBar.Width = (int)((received * 463) / maximum);
        }

        public void Bar2SetProgress(ulong received, ulong maximum)
        {
            this.TotalBar.Width = (int)((received * ((ulong)463)) / maximum);
        }

        private void UpdatePatch_MouseLeave(object sender, EventArgs e)
        {
            this.UpdatePatch.BackgroundImage = Resources.btn_update_v2;
            this.UpdatePatch.BackColor = System.Drawing.Color.Transparent;
        }

        private void UpdatePatch_MouseMove(object sender, MouseEventArgs e)
        {
            this.UpdatePatch.BackgroundImage = Resources.btn_update_v2_over;
            this.UpdatePatch.BackColor = System.Drawing.Color.Transparent;
        }

        private List<XMLModel> Parse(string Path)
        {
            List<XMLModel> List = new List<XMLModel>();
            XmlDocument Document = new XmlDocument();
            FileStream Stream = new FileStream(Path, FileMode.Open);
            if (Stream.Length == 0L)
            {
                this.Logger("[ERROR] CODE PAUSE");
            }
            else
            {
                try
                {
                    Document.Load(Stream);
                    for (XmlNode Node = Document.FirstChild; Node != null; Node = Node.NextSibling)
                    {
                        if ("List".Equals(Node.Name))
                        {
                            for (XmlNode Node2 = Node.FirstChild; Node2 != null; Node2 = Node2.NextSibling)
                            {
                                if ("File".Equals(Node2.Name))
                                {
                                    XmlNamedNodeMap attributes = Node2.Attributes;
                                    List.Add(new XMLModel(attributes.GetNamedItem("Name").Value));
                                }
                            }
                        }
                    }
                }
                catch (XmlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            Stream.Dispose();
            Stream.Close();
            return List;
        }

        public void XmlLoad()
        {
            try
            {
                if (File.Exists(Application.StartupPath + @"\Removes.xml"))
                {
                    List<XMLModel> List = this.Parse(Application.StartupPath + @"\Removes.xml");
                    int Total = 0;
                    this.Label.Text = "กำลังลบไฟล์ที่ไม่ได้ใช้.";
                    this.Check.Enabled = false;
                    this.Check.BackgroundImage = Resources.Check_Normal;
                    this.FileName.Visible = true;
                    foreach (XMLModel Model in List)
                    {
                        Total++;
                        if (File.Exists(Application.StartupPath + Model.Name))
                        {
                            this.FileName.Text = Model.Name;
                            File.Delete(Application.StartupPath + Model.Name);
                        }
                        this.Bar2SetProgress((ulong)Total, (ulong)List.Count);
                    }
                    File.Delete(Application.StartupPath + @"\Removes.xml");
                    this.Label.Text = "คุณสามารถเริ่มเกมได้ในขณะนี้!";
                    this.FileName.Visible = false;
                    this.Start.Enabled = true;
                    this.Check.Enabled = true;
                    this.Start.BackgroundImage = Resources.Start_Normal;
                    this.Check.BackgroundImage = Resources.Check_Normal;
                }
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        private void GameUpdate_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    int Num = int.Parse(File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version"));
                    int Version = Num + 1;
                    this.ArchiveBar.Width = 0;
                    this.Unzip(Application.StartupPath, string.Concat(new object[] { Application.StartupPath, @"\_DownloadPatchFiles\UpdatePatch_Client_", Version, ".zip" }));
                    this.Timer.Start();
                    Directory.Delete(Application.StartupPath + @"\_DownloadPatchFiles", true);
                    this.ArchiveBar.Width = 463;
                }
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        private void GameUpdate_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Bar1SetProgress(e.BytesReceived, e.TotalBytesToReceive, false);
        }

        private void Unzip_ExtractProgressChanged(object sender, ExtractProgressEventArgs e)
        {
            if (e.TotalBytesToTransfer != 0L)
            {
                this.Bar1SetProgress(e.BytesTransferred, e.TotalBytesToTransfer, false);
            }
            this.ArchiveBar.Refresh();
            this.ArchiveBar.Update();
        }

        public void Unzip(string TargetDir, string ZipToUnpack)
        {
            try
            {
                SearchFileExtension();

                this.Label.Text = "กำลังติดตั้ง...";
                Value = true;
                using (ZipFile File = ZipFile.Read(ZipToUnpack))
                {
                    File.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(this.Unzip_ExtractProgressChanged);
                    int Total = 0;
                    int Count = 0;
                    foreach (ZipEntry Entry in File)
                    {
                        if (!Entry.IsDirectory)
                        {
                            Count++;
                        }
                    }
                    foreach (ZipEntry Entry in File)
                    {
                        string FileName = Entry.FileName;
                        if (FileName.Contains("/"))
                        {
                            int Num = FileName.LastIndexOf("/");
                            FileName = FileName.Substring(Num + 1);
                        }
                        if (!Entry.IsDirectory)
                        {
                            if (FileName != "pbkuyrai.version")
                            {
                                this.Logger("# Patch file Update Exception # FileName : " + FileName);
                            }
                            base.Update();
                            this.Refresh();
                            this.Bar2SetProgress((ulong)(++Total), (ulong)Count);
                        }
                        Entry.Extract(TargetDir, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                LauncherModul.LastVersion = int.Parse(this.Web.DownloadString(Modul.WEB + "launcher/versions/last_client_version.txt"));

                if (LauncherModul.LastVersion == int.Parse(File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version")))
                {
                    this.KeyPreview = true;
                    this.WebBrowser.Visible = true;
                    int Button = int.Parse(this.Web.DownloadString(Modul.WEB + "launcher/status/button.txt"));
                    base.Visible = true;
                    this.Label_Version.Visible = false; // 
                    this.Start.Visible = true;
                    this.Check.Visible = true;
                    this.UpdatePatch.Visible = false;
                    LauncherModul.LastVersion = int.Parse(this.Web.DownloadString(Modul.WEB + "launcher/versions/last_client_version.txt"));
                    this.Label_Version.Text = "เวอร์ชั่นแพทช์: " + File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version") + "/" + LauncherModul.LastVersion.ToString();
                    this.Start.Enabled = false;
                    if (this.Computer.FileSystem.DirectoryExists(Application.StartupPath + @"\_LauncherPatchFiles"))
                    {
                        this.Computer.FileSystem.DeleteDirectory(Application.StartupPath + @"\_LauncherPatchFiles", DeleteDirectoryOption.DeleteAllContents);
                    }
                    this.Label.Text = "คุณสามารถเริ่มเกมได้ในขณะนี้!";
                    if (Button == 1)
                    {
                        this.Start.Enabled = true;
                        this.Check.Enabled = true;
                    }
                    else if (Button == 2)
                    {
                        this.Start.BackgroundImage = Resources.Start_Disable;
                        this.Check.BackgroundImage = Resources.Check_Disable;
                        this.Start.Enabled = false;
                        this.Check.Enabled = false;
                    }
                    else if (Button == 3)
                    {
                        this.Check.BackgroundImage = Resources.Check_Disable;
                        this.Check.Enabled = false;
                        this.Start.Enabled = true;
                    }
                    else if (Button == 4)
                    {
                        this.Check.BackgroundImage = Resources.Check_Disable;
                        this.Check.Enabled = false;
                        this.Start.Enabled = true;
                    }
                    this.XmlLoad();
                    this.Timer.Stop();
                }
                else
                {
                    Value = true;
                    int Version = int.Parse(File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version")) + 1;
                    this.Computer.FileSystem.CreateDirectory(Application.StartupPath + @"\_DownloadPatchFiles");
                    try
                    {
                        this.GameUpdate.DownloadFileAsync(new Uri(Modul.UPDATEPATCH + "update/client/UpdatePatch_Client_" + Version + ".zip"), string.Concat(new object[] { Application.StartupPath, @"\_DownloadPatchFiles\UpdatePatch_Client_", Version, ".zip" }));
                    }
                    catch
                    {
                        this.Label.Text = "ล้มเหลว";
                        MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                        this.Logger("[ERROR] CODE UPDATE");
                    }
                    this.Bar2SetProgress(0L, 100L);
                    this.Logger("# PBLauncher Download - " + Version);
                    this.Timer.Stop();
                }
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        private void UpdatePatch_Click(object sender, EventArgs e)
        {
            try
            {
                Value = true;
                this.ArchiveBar.Width = 0;
                this.TotalBar.Width = 0;
                this.UpdatePatch.Enabled = false;
                int.Parse(this.Web.DownloadString(Modul.WEB + "launcher/versions/last_client_version.txt"));
                int Version = int.Parse(File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version")) + 1;

                this.Label.Text = "กำลังตรวจสอบเวอร์ชั่น...";
                this.Computer.FileSystem.CreateDirectory(Application.StartupPath + @"\_DownloadPatchFiles");
                this.Label.Text = "กำลังเริ่มการดาวน์โหลด...";

                try
                {
                    this.GameUpdate.DownloadFileAsync(new Uri(Modul.UPDATEPATCH + "update/client/UpdatePatch_Client_" + Version + ".zip"), string.Concat(new object[] { Application.StartupPath, @"\_DownloadPatchFiles\UpdatePatch_Client_", Version, ".zip" }));
                }
                catch
                {
                    this.Label.Text = "ล้มเหลว";
                    MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                    this.Logger("[ERROR] CODE UPDATE");
                }
                this.Logger("## PBLauncher Download Patch Version - " + Version);
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        public static List<string> GetFilesRecursive(string Initial, string[] Extension)
        {
            List<string> Result = new List<string>();
            Stack<string> Stack = new Stack<string>();
            Stack.Push(Initial);

            while ((Stack.Count > 0))
            {
                try
                {
                    string Dir = Stack.Pop();
                    foreach (string ext in Extension)
                    {
                        string Ext = (ext.Substring(0, 1) != "*") ? "*" + ext : ext;
                        Result.AddRange(Directory.GetFiles(Dir, Ext));
                    }
                    string DirectoryName = null;
                    foreach (string Variable in Directory.GetDirectories(Dir))
                    {
                        DirectoryName = Variable;
                        Stack.Push(DirectoryName);
                    }
                }
                catch
                {
                    MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                }
            }
            return Result;
        }

        public void SearchFileExtension()
        {
            try
            {
                string[] Extensions = new[] { "*.tmp", "*.PendingOverwrite" };
                List<string> List = GetFilesRecursive(Application.StartupPath, Extensions);
                foreach (string FileName in List)
                {
                    File.Delete(FileName);
                }
            }
            catch
            {
                MessageBox.Show("กรุณาปิดแอนตี้ไวรัส หรือ ปิด Windows Defender", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            }
        }

        private void btn_pbconfig_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.Verb = "runas";
            pi.FileName = "PBConfig.exe";
            try
            {
                Process.Start(pi);
            }
            catch
            {
                MessageBox.Show("ไม่สามารถแก้ไขขนาดหน้าจอได้ เนื่องจากไม่มีไฟล์ PBConfig.exe", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://43.255.241.35");
            Process.Start(sInfo);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://43.255.241.35/index.php?page=shop");
            Process.Start(sInfo);
        }

        private void lb_exp_Click(object sender, EventArgs e)
        {

        }

        private void img_rank_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        /*private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(LoadLibrary("helperx.dll").ToString());
        }*/

        private void txt_Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Login.PerformClick();
            }
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                //if (GetFileSize("helperx.dll") == 77824)

                    if (PerformRequest("http://43.255.241.35/check_login/check_login.php", txt_User.Text, txt_Pass.Text) == "ชื่อผู้ใช้งาน หรือ รหัสผ่านไม่ถูกต้อง")
                    {
                        MessageBox.Show(PerformRequest("http://43.255.241.35/check_login/check_login.php", txt_User.Text, txt_Pass.Text));
                        txt_Pass.Text = "";
                        txt_User.Text = "";
                        txt_User.Focus();
                    }
                    else if (PerformRequest("http://43.255.241.35/check_login/check_login.php", txt_User.Text, txt_Pass.Text) == "ban")
                    {

                        string message = "ไอดี : " + txt_User.Text + " โดนแบน กรุณาติดต่อทีมงาน!!";
                        string caption = "แจ้งเตือน";
                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;
                        result = MessageBox.Show(message, caption, buttons);

                        txt_Pass.Text = "";
                        txt_User.Text = "";
                        txt_User.Focus();
                    }
                    else
                    {
                        pn_Login.Visible = false;
                        btn_Login.Visible = false;
                        txt_User.Visible = false;
                        txt_Pass.Visible = false;

                        Player resultPlayer = JsonConvert.DeserializeObject<Player>(PerformRequest("http://43.255.241.35/check_login/check_login.php", txt_User.Text, txt_Pass.Text));
                        this.login = resultPlayer.login;
                        this.name = resultPlayer.name;
                        this.rank = resultPlayer.rank;
                        this.exp_ = resultPlayer.exp_;
                        this.token = resultPlayer.token;


                        var i = this.exp_;
                        var s = $"{i:n}";
                        s = $"{i:n0}";

                        img_rank.Visible = true;
                        lb_name.Visible = true;
                        lb_exp.Visible = true;
                        pictureBox3.Visible = true;

                        try
                        {
                            Image image = Image.FromFile(@"Gui\rank\" + this.rank + ".gif");
                            img_rank.Image = image;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("ไม่พบไฟล์ Gui/rank กรุณาติดต่อทีมงาน", "แจ้งเตือน");
                        }

                        lb_name.Text = this.name;
                        lb_exp.Text = s;

                        SearchFileExtension();
                        this.KeyPreview = true;
                        int Button = int.Parse(this.Web.DownloadString(Modul.WEB + "launcher/status/button.txt"));
                        this.Label.Text = " ";
                        this.Label_File.Text = "ไฟล์";
                        this.Label_Total.Text = "ทั้งหมด";
                        base.WindowState = FormWindowState.Normal;
                        base.Visible = true;
                        this.Label_Version.Visible = false; // 
                        this.Start.Visible = true;
                        this.Check.Visible = true;
                        LauncherModul.LastVersion = int.Parse(this.Web.DownloadString(Modul.WEB + "launcher/versions/last_client_version.txt"));
                        this.Label_Version.Text = "เวอร์ชั่นแพทช์: " + File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version") + "/" + LauncherModul.LastVersion.ToString();
                        this.Start.Enabled = false;
                        if (this.Computer.FileSystem.DirectoryExists(Application.StartupPath + @"\_LauncherPatchFiles"))
                        {
                            this.Computer.FileSystem.DeleteDirectory(Application.StartupPath + @"\_LauncherPatchFiles", DeleteDirectoryOption.DeleteAllContents);
                        }
                        this.Label.Text = "คุณสามารถเริ่มเกมได้ในขณะนี้!";
                        if (Button == 1)
                        {
                            this.Start.Enabled = true;
                            this.Check.Enabled = true;
                        }
                        else if (Button == 2)
                        {
                            this.Start.BackgroundImage = Resources.Start_Disable;
                            this.Check.BackgroundImage = Resources.Check_Disable;
                            this.Start.Enabled = false;
                            this.Check.Enabled = false;
                        }
                        else if (Button == 3)
                        {
                            this.Check.BackgroundImage = Resources.Check_Disable;
                            this.Check.Enabled = false;
                            this.Start.Enabled = true;
                        }
                        else if (Button == 4)
                        {
                            this.Check.BackgroundImage = Resources.Check_Disable;
                            this.Check.Enabled = false;
                            this.Start.Enabled = true;
                        }

                        if (LauncherModul.LastVersion.ToString() == File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version"))
                        {
                            this.XmlLoad();
                        }
                        else if (LauncherModul.LastVersion < int.Parse(File.ReadAllText(Application.StartupPath + @"\pbkuyrai.version")))
                        {
                            File.WriteAllText(Application.StartupPath + @"\pbkuyrai.version", (0).ToString());
                            Value = true;
                            this.Label.Text = "กรุณาอัปเดตตัวเกมส์ของคุณ.";
                            this.ArchiveBar.Width = 0;
                            this.TotalBar.Width = 0;
                            this.Start.Visible = false;
                            this.Start.Enabled = true;
                            this.UpdatePatch.Enabled = true;
                            this.UpdatePatch.Visible = true;
                            this.Check.Visible = true;
                            this.Check.Enabled = false;
                            this.Check.BackgroundImage = Resources.Check_Disable;
                        }
                        else
                        {
                            Value = true;
                            this.Label.Text = "กรุณาอัปเดตตัวเกมส์ของคุณ.";
                            this.ArchiveBar.Width = 0;
                            this.TotalBar.Width = 0;
                            this.Start.Visible = false;
                            this.Start.Enabled = true;
                            this.UpdatePatch.Enabled = true;
                            this.UpdatePatch.Visible = true;
                            this.Check.Visible = true;
                            this.Check.Enabled = false;
                            this.Check.BackgroundImage = Resources.Check_Disable;
                        }
                    }
            }
            catch (Exception)
            {
                MessageBox.Show("ไม่พบไฟล์ Newtonsoft.Json.dll กรุณาติดต่อทีมงาน", "แจ้งเตือน");
            }
        }

        private string PerformRequest(string url, string username, string password)
        {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = Cookie; // use the global cookie variable
                string postData = "usercheck=" + username + "&passcheck=" + password;
                byte[] data = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                WebResponse response = (HttpWebResponse)request.GetResponse();
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}
