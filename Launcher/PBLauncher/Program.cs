namespace PointBlank.Launcher
{
    using DLL;
    using DLL.Modul;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var frm = new Connection())
            {
                try
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        WebClient Web = new WebClient();
                        if (!File.Exists(Application.StartupPath + @"\pbkuyrai.version"))
                        {
                            MessageBox.Show("ไม่พบไฟล์ดังกล่าว.", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (MessageBox.Show("กำลังดาวน์โหลดไฟล์ที่หายไป...", Modul.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                Web.DownloadFile(Modul.WEB + "launcher/file/pbkuyrai.version", "pbkuyrai.version");
                                Application.Restart();
                            }
                        }
                        else
                        {
                            LauncherModul.LastVersion = int.Parse(new Launcher(frm, new AuthModul()).Web.DownloadString(Modul.WEB + "launcher/versions/last_launcher_version.txt"));

                            if (File.ReadAllText(Application.StartupPath + "\\lckuyrai.version") != LauncherModul.LastVersion.ToString())
                            {
                                Process.Start(Application.StartupPath + @"\PBUpdate.exe");
                            }
                            else
                            {
                                Application.Run(new Launcher(frm, new AuthModul()));
                            }
                        }
                    }
                }
                catch
                {
                    frm.Close();
                }
            }
        }
    }
}

