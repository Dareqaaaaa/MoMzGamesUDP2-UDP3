using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PointBlank.Launcher
{
    public partial class Antihack : Form
    {
        public Antihack()
        {
            InitializeComponent();
        }

        int countdown = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            countdown += 1;
            if (countdown == 5)
            {
                Process.Start("helperx.exe");
                Process.Start("PointBlank.exe");               
                Application.Exit();
            }
        }

        private void Antihack_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
