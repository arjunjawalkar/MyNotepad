using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuTemplate
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is developer by Arjun Jawalkar.\nReach me at arjun.jawalkar@gmail.com", "About Author", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Browse text file";
            openFileDialog.Filter = "text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            { }
        }
    }
}
