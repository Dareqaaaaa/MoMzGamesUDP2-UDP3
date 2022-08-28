using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PointBlank.Launcher
{
    partial class Launcher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.Label = new System.Windows.Forms.Label();
            this.Label_File = new System.Windows.Forms.Label();
            this.Label_Total = new System.Windows.Forms.Label();
            this.Label_Version = new System.Windows.Forms.Label();
            this.Exit = new System.Windows.Forms.PictureBox();
            this.Check = new System.Windows.Forms.Button();
            this.TotalBar = new System.Windows.Forms.PictureBox();
            this.ArchiveBar = new System.Windows.Forms.PictureBox();
            this.FileName = new System.Windows.Forms.Label();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.Minimize = new System.Windows.Forms.PictureBox();
            this.BackGroundWorker = new System.ComponentModel.BackgroundWorker();
            this.Start = new System.Windows.Forms.Button();
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.UpdatePatch = new System.Windows.Forms.Button();
            this.btn_pbconfig = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txt_User = new System.Windows.Forms.TextBox();
            this.txt_Pass = new System.Windows.Forms.TextBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.pn_Login = new System.Windows.Forms.GroupBox();
            this.img_rank = new System.Windows.Forms.PictureBox();
            this.lb_exp = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.lb_name = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Exit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArchiveBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pn_Login.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img_rank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.BackColor = System.Drawing.Color.Transparent;
            this.Label.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.Label.ForeColor = System.Drawing.Color.Transparent;
            this.Label.Location = new System.Drawing.Point(125, 633);
            this.Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(411, 16);
            this.Label.TabIndex = 0;
            this.Label.Text = " ";
            this.Label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.Label.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            // 
            // Label_File
            // 
            this.Label_File.BackColor = System.Drawing.Color.Transparent;
            this.Label_File.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.Label_File.ForeColor = System.Drawing.Color.Transparent;
            this.Label_File.Location = new System.Drawing.Point(125, 652);
            this.Label_File.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_File.Name = "Label_File";
            this.Label_File.Size = new System.Drawing.Size(48, 16);
            this.Label_File.TabIndex = 1;
            this.Label_File.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.Label_File.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            // 
            // Label_Total
            // 
            this.Label_Total.BackColor = System.Drawing.Color.Transparent;
            this.Label_Total.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.Label_Total.ForeColor = System.Drawing.Color.Transparent;
            this.Label_Total.Location = new System.Drawing.Point(125, 684);
            this.Label_Total.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Total.Name = "Label_Total";
            this.Label_Total.Size = new System.Drawing.Size(411, 16);
            this.Label_Total.TabIndex = 2;
            this.Label_Total.Text = " ";
            this.Label_Total.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.Label_Total.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            // 
            // Label_Version
            // 
            this.Label_Version.BackColor = System.Drawing.Color.Transparent;
            this.Label_Version.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Label_Version.ForeColor = System.Drawing.Color.White;
            this.Label_Version.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label_Version.Location = new System.Drawing.Point(262, 7);
            this.Label_Version.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label_Version.Name = "Label_Version";
            this.Label_Version.Size = new System.Drawing.Size(471, 13);
            this.Label_Version.TabIndex = 3;
            this.Label_Version.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.Label_Version.Visible = false;
            this.Label_Version.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.Label_Version.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            // 
            // Exit
            // 
            this.Exit.BackColor = System.Drawing.Color.White;
            this.Exit.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.Closed_Normal;
            this.Exit.Location = new System.Drawing.Point(995, 24);
            this.Exit.Margin = new System.Windows.Forms.Padding(2);
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(23, 23);
            this.Exit.TabIndex = 4;
            this.Exit.TabStop = false;
            this.Exit.Click += new System.EventHandler(this.Closed_Click);
            this.Exit.MouseEnter += new System.EventHandler(this.Closed_MouseMove);
            this.Exit.MouseLeave += new System.EventHandler(this.Closed_MouseLeave);
            // 
            // Check
            // 
            this.Check.BackColor = System.Drawing.Color.Transparent;
            this.Check.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.btn_check_v23;
            this.Check.FlatAppearance.BorderSize = 0;
            this.Check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Check.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Check.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(32)))), ((int)(((byte)(45)))));
            this.Check.Location = new System.Drawing.Point(567, 642);
            this.Check.Margin = new System.Windows.Forms.Padding(0);
            this.Check.Name = "Check";
            this.Check.Size = new System.Drawing.Size(106, 72);
            this.Check.TabIndex = 0;
            this.Check.TabStop = false;
            this.Check.UseVisualStyleBackColor = false;
            this.Check.Visible = false;
            this.Check.Click += new System.EventHandler(this.Verif_Click);
            this.Check.MouseLeave += new System.EventHandler(this.Check_MouseLeave);
            this.Check.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Check_MouseMove);
            // 
            // TotalBar
            // 
            this.TotalBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(172)))), ((int)(((byte)(240)))));
            this.TotalBar.Location = new System.Drawing.Point(127, 701);
            this.TotalBar.Name = "TotalBar";
            this.TotalBar.Size = new System.Drawing.Size(409, 10);
            this.TotalBar.TabIndex = 10;
            this.TotalBar.TabStop = false;
            this.TotalBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.TotalBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            // 
            // ArchiveBar
            // 
            this.ArchiveBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(172)))), ((int)(((byte)(240)))));
            this.ArchiveBar.Location = new System.Drawing.Point(127, 669);
            this.ArchiveBar.Name = "ArchiveBar";
            this.ArchiveBar.Size = new System.Drawing.Size(409, 10);
            this.ArchiveBar.TabIndex = 11;
            this.ArchiveBar.TabStop = false;
            this.ArchiveBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.ArchiveBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            // 
            // FileName
            // 
            this.FileName.BackColor = System.Drawing.Color.Transparent;
            this.FileName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.FileName.ForeColor = System.Drawing.Color.Transparent;
            this.FileName.Location = new System.Drawing.Point(168, 652);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(368, 16);
            this.FileName.TabIndex = 100;
            this.FileName.Text = " ";
            this.FileName.Visible = false;
            this.FileName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.FileName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // Minimize
            // 
            this.Minimize.BackColor = System.Drawing.Color.White;
            this.Minimize.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.Hide_Normal;
            this.Minimize.Location = new System.Drawing.Point(967, 24);
            this.Minimize.Name = "Minimize";
            this.Minimize.Size = new System.Drawing.Size(23, 23);
            this.Minimize.TabIndex = 15;
            this.Minimize.TabStop = false;
            this.Minimize.Click += new System.EventHandler(this.Minimize_Click);
            this.Minimize.MouseLeave += new System.EventHandler(this.Minimize_MouseLeave);
            this.Minimize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Minimize_MouseMove);
            // 
            // Start
            // 
            this.Start.BackColor = System.Drawing.Color.Transparent;
            this.Start.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.btn_start_v2;
            this.Start.FlatAppearance.BorderSize = 0;
            this.Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Start.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(32)))), ((int)(((byte)(45)))));
            this.Start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Start.Location = new System.Drawing.Point(721, 640);
            this.Start.Margin = new System.Windows.Forms.Padding(0);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(166, 76);
            this.Start.TabIndex = 0;
            this.Start.TabStop = false;
            this.Start.UseVisualStyleBackColor = false;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            this.Start.MouseLeave += new System.EventHandler(this.Start_MouseLeave);
            this.Start.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Start_MouseMove);
            // 
            // WebBrowser
            // 
            this.WebBrowser.Location = new System.Drawing.Point(94, 130);
            this.WebBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.WebBrowser.MinimumSize = new System.Drawing.Size(13, 13);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.ScrollBarsEnabled = false;
            this.WebBrowser.Size = new System.Drawing.Size(842, 431);
            this.WebBrowser.TabIndex = 1000;
            this.WebBrowser.TabStop = false;
            this.WebBrowser.Url = new System.Uri("http://43.255.241.35/pblauncher/launcher/web/", System.UriKind.Absolute);
            this.WebBrowser.Visible = false;
            // 
            // UpdatePatch
            // 
            this.UpdatePatch.BackColor = System.Drawing.Color.Transparent;
            this.UpdatePatch.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.btn_update_v2;
            this.UpdatePatch.Enabled = false;
            this.UpdatePatch.FlatAppearance.BorderSize = 0;
            this.UpdatePatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdatePatch.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.UpdatePatch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(32)))), ((int)(((byte)(45)))));
            this.UpdatePatch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.UpdatePatch.Location = new System.Drawing.Point(721, 640);
            this.UpdatePatch.Margin = new System.Windows.Forms.Padding(0);
            this.UpdatePatch.Name = "UpdatePatch";
            this.UpdatePatch.Size = new System.Drawing.Size(166, 76);
            this.UpdatePatch.TabIndex = 1001;
            this.UpdatePatch.TabStop = false;
            this.UpdatePatch.UseVisualStyleBackColor = false;
            this.UpdatePatch.Visible = false;
            this.UpdatePatch.Click += new System.EventHandler(this.UpdatePatch_Click);
            this.UpdatePatch.MouseLeave += new System.EventHandler(this.UpdatePatch_MouseLeave);
            this.UpdatePatch.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UpdatePatch_MouseMove);
            // 
            // btn_pbconfig
            // 
            this.btn_pbconfig.BackColor = System.Drawing.Color.Transparent;
            this.btn_pbconfig.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.btn_pbconfig_v2;
            this.btn_pbconfig.FlatAppearance.BorderSize = 0;
            this.btn_pbconfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pbconfig.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btn_pbconfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(32)))), ((int)(((byte)(45)))));
            this.btn_pbconfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_pbconfig.Location = new System.Drawing.Point(539, 640);
            this.btn_pbconfig.Margin = new System.Windows.Forms.Padding(0);
            this.btn_pbconfig.Name = "btn_pbconfig";
            this.btn_pbconfig.Size = new System.Drawing.Size(166, 76);
            this.btn_pbconfig.TabIndex = 1002;
            this.btn_pbconfig.TabStop = false;
            this.btn_pbconfig.UseVisualStyleBackColor = false;
            this.btn_pbconfig.Click += new System.EventHandler(this.btn_pbconfig_Click);
            this.btn_pbconfig.MouseLeave += new System.EventHandler(this.btn_pbconfig_MouseLeave);
            this.btn_pbconfig.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btn_pbconfig_MouseMove);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.btn_web1;
            this.pictureBox1.Location = new System.Drawing.Point(171, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(166, 58);
            this.pictureBox1.TabIndex = 1003;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.btn_shop1;
            this.pictureBox2.Location = new System.Drawing.Point(343, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(166, 58);
            this.pictureBox2.TabIndex = 1004;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // txt_User
            // 
            this.txt_User.Location = new System.Drawing.Point(13, 21);
            this.txt_User.Multiline = true;
            this.txt_User.Name = "txt_User";
            this.txt_User.Size = new System.Drawing.Size(166, 27);
            this.txt_User.TabIndex = 1005;
            // 
            // txt_Pass
            // 
            this.txt_Pass.Location = new System.Drawing.Point(13, 52);
            this.txt_Pass.Multiline = true;
            this.txt_Pass.Name = "txt_Pass";
            this.txt_Pass.PasswordChar = '*';
            this.txt_Pass.Size = new System.Drawing.Size(166, 26);
            this.txt_Pass.TabIndex = 1006;
            this.txt_Pass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Pass_KeyDown);
            // 
            // btn_Login
            // 
            this.btn_Login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_Login.ForeColor = System.Drawing.Color.White;
            this.btn_Login.Location = new System.Drawing.Point(185, 21);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(169, 57);
            this.btn_Login.TabIndex = 1007;
            this.btn_Login.Text = "เข้าสู่ระบบ";
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // pn_Login
            // 
            this.pn_Login.BackColor = System.Drawing.Color.Silver;
            this.pn_Login.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.lgpn;
            this.pn_Login.Controls.Add(this.txt_User);
            this.pn_Login.Controls.Add(this.btn_Login);
            this.pn_Login.Controls.Add(this.txt_Pass);
            this.pn_Login.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pn_Login.ForeColor = System.Drawing.Color.White;
            this.pn_Login.Location = new System.Drawing.Point(539, 627);
            this.pn_Login.Name = "pn_Login";
            this.pn_Login.Size = new System.Drawing.Size(368, 93);
            this.pn_Login.TabIndex = 1008;
            this.pn_Login.TabStop = false;
            this.pn_Login.Text = "เข้าสู่ระบบ |";
            // 
            // img_rank
            // 
            this.img_rank.Image = global::PointBlank.Launcher.Properties.Resources._54;
            this.img_rank.Location = new System.Drawing.Point(552, 26);
            this.img_rank.Name = "img_rank";
            this.img_rank.Size = new System.Drawing.Size(20, 20);
            this.img_rank.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.img_rank.TabIndex = 1009;
            this.img_rank.TabStop = false;
            this.img_rank.Visible = false;
            this.img_rank.Click += new System.EventHandler(this.img_rank_Click);
            // 
            // lb_exp
            // 
            this.lb_exp.AutoSize = true;
            this.lb_exp.BackColor = System.Drawing.Color.Maroon;
            this.lb_exp.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_exp.Location = new System.Drawing.Point(810, 28);
            this.lb_exp.Name = "lb_exp";
            this.lb_exp.Size = new System.Drawing.Size(56, 16);
            this.lb_exp.TabIndex = 1011;
            this.lb_exp.Text = "103254";
            this.lb_exp.Visible = false;
            this.lb_exp.Click += new System.EventHandler(this.lb_exp_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.pn_lg;
            this.pictureBox3.Image = global::PointBlank.Launcher.Properties.Resources.sss;
            this.pictureBox3.Location = new System.Drawing.Point(517, 7);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(422, 59);
            this.pictureBox3.TabIndex = 1012;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Visible = false;
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.BackColor = System.Drawing.Color.Maroon;
            this.lb_name.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_name.Location = new System.Drawing.Point(610, 28);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(46, 16);
            this.lb_name.TabIndex = 1013;
            this.lb_name.Text = "label1";
            this.lb_name.Visible = false;
            // 
            // Launcher
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkRed;
            this.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.bg;
            this.ClientSize = new System.Drawing.Size(1029, 721);
            this.Controls.Add(this.lb_name);
            this.Controls.Add(this.lb_exp);
            this.Controls.Add(this.img_rank);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pn_Login);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_pbconfig);
            this.Controls.Add(this.WebBrowser);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.Minimize);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.TotalBar);
            this.Controls.Add(this.ArchiveBar);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.Label_Version);
            this.Controls.Add(this.Label);
            this.Controls.Add(this.Label_File);
            this.Controls.Add(this.Label_Total);
            this.Controls.Add(this.UpdatePatch);
            this.Controls.Add(this.Check);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Launcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PBLauncher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.Load += new System.EventHandler(this.Launcher_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Launcher_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.Exit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ArchiveBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pn_Login.ResumeLayout(false);
            this.pn_Login.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.img_rank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox ArchiveBar;
        private Label FileName;
        private Label Label;
        private Label Label_File;
        private Label Label_Total;
        private Label Label_Version;
        private Point NewPoint;
        private PictureBox Exit;
        private PictureBox Minimize;
        private Timer Timer;
        private PictureBox TotalBar;
        private Button Check;
        private Button Start;
        private BackgroundWorker BackGroundWorker;
        private WebBrowser WebBrowser;
        private Button UpdatePatch;
        private Button btn_pbconfig;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private TextBox txt_User;
        private TextBox txt_Pass;
        private Button btn_Login;
        private GroupBox pn_Login;
        private PictureBox img_rank;
        private Label lb_exp;
        private PictureBox pictureBox3;
        private Label lb_name;
        private Button button1;
    }
}