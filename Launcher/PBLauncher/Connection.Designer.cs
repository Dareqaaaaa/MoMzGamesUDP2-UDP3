using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace PointBlank.Launcher
{
    partial class Connection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Connection));
            this.Label = new System.Windows.Forms.Label();
            this.Start = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // Label
            // 
            this.Label.BackColor = System.Drawing.Color.Transparent;
            this.Label.Location = new System.Drawing.Point(1, 9);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(264, 21);
            this.Label.TabIndex = 0;
            this.Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Start
            // 
            this.Start.WorkerReportsProgress = true;
            this.Start.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Start_DoWork);
            this.Start.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Start_RunWorkerCompleted);
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PointBlank.Launcher.Properties.Resources.Start_BG;
            this.ClientSize = new System.Drawing.Size(266, 42);
            this.Controls.Add(this.Label);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Connection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PBLauncher";
            this.Load += new System.EventHandler(this.Connection_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Label Label;
        public BackgroundWorker Start;
    }
}