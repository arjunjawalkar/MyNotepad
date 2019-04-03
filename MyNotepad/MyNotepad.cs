using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace MyNotepad
{
    public partial class MyNotepad : Form
    {
        string filename;
        bool isNewFile = false;
        string tempappname;
        string combined_appname;
        string originalText;
        public MyNotepad()
        {
            InitializeComponent();
            tempappname = this.Text;
            combined_appname = " - " + tempappname;
        }

        public int getWidth()
        {
            int w = 25;
            // get total lines of contentRichTextBox    
            int line = contentRichTextBox.Lines.Length;

            if (line <= 99)
            {
                w = 20 + (int)contentRichTextBox.Font.Size;
            }
            else if (line <= 999)
            {
                w = 30 + (int)contentRichTextBox.Font.Size;
            }
            else
            {
                w = 50 + (int)contentRichTextBox.Font.Size;
            }

            return w;
        }

        public void AddLineNumbers()
        {
            // create & set Point pt to (0,0)    
            Point pt = new Point(0, 0);
            // get First Index & First Line from contentRichTextBox    
            int First_Index = contentRichTextBox.GetCharIndexFromPosition(pt);
            int First_Line = contentRichTextBox.GetLineFromCharIndex(First_Index);
            // set X & Y coordinates of Point pt to ClientRectangle Width & Height respectively    
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            // get Last Index & Last Line from contentRichTextBox    
            int Last_Index = contentRichTextBox.GetCharIndexFromPosition(pt);
            int Last_Line = contentRichTextBox.GetLineFromCharIndex(Last_Index);
            // set Center alignment to LineNumberTextBox    
            lineNumberTextBox.SelectionAlignment = HorizontalAlignment.Center;
            // set LineNumberTextBox text to null & width to getWidth() function value    
            lineNumberTextBox.Text = "";
            lineNumberTextBox.Width = getWidth();
            //contentRichTextBox.Width = this.Width - getWidth();
            // now add each line number to LineNumberTextBox upto last line    
            for (int i = First_Line; i < Last_Line+1; i++)
            {
                lineNumberTextBox.Text += i + 1 + "\n";
            }
        }

        private void MyNotepad_Load(object sender, EventArgs e)
        {
            lineNumberTextBox.Font = contentRichTextBox.Font;
            contentRichTextBox.Select();
            //AddLineNumbers();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is developed by Arjun Jawalkar.\nReach me at arjun.jawalkar@gmail.com", "About Author", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Open";
            openFileDialog.Filter = "text files (*.txt)|*.txt";
          
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog.FileName;
                this.Text = filename + combined_appname;
                this.Update();
                using (StreamReader reader = new StreamReader(filename))
                {
                    contentRichTextBox.Visible = true;
                    contentRichTextBox.Text = await reader.ReadToEndAsync();
                    originalText = contentRichTextBox.Text;
                    
                }
            }
            AddLineNumbers();
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string inputtext = contentRichTextBox.Text;

            if (isNewFile == true && !File.Exists(filename))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    
                    foreach (string line in contentRichTextBox.Lines)
                    {
                        await writer.WriteLineAsync(line);
                    }
                    MessageBox.Show("Text written to file", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    originalText = contentRichTextBox.Text;
                }
            }
            this.Text = filename + combined_appname;
        }

        private bool isContentChanged()
        {
            if (originalText != contentRichTextBox.Text)
                return true;
            else 
                return false;
        }

        private void contentRichTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 0|| e.KeyChar <= 255)// && isTextChanged == false)
            {
                if (isContentChanged())
                {
                    if (!this.Text.StartsWith("*"))
                        this.Text = "*" + this.Text;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isNewFile = true;
            this.Text = "*" + "untitled text file" + " - " + tempappname;
            contentRichTextBox.Visible = true;
            contentRichTextBox.Select();
            AddLineNumbers();
        }

        private async void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string disk_filename;
            //MessageBox.Show("Need to implement.\n file not saved", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.InitialDirectory = @"C:\";
            savefile.Title = "Save As";
            savefile.DefaultExt = "txt";
            savefile.Filter = "Text files (*.txt)|*.txt";
            savefile.RestoreDirectory = true;
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                disk_filename = savefile.FileName;

                using (StreamWriter writer = new StreamWriter(disk_filename))
                {
                    foreach (string line in contentRichTextBox.Lines)
                    {
                        await writer.WriteLineAsync(line);
                    }
                    MessageBox.Show("Text written to file", "MESSAGE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    originalText = contentRichTextBox.Text;
                    this.Text = disk_filename + combined_appname;
                    filename = disk_filename;
                }
            }
            this.Text = filename + combined_appname;
        }

        private void contentRichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = contentRichTextBox.GetPositionFromCharIndex(contentRichTextBox.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers();
            }
        }

        private void contentRichTextBox_VScroll(object sender, EventArgs e)
        {
            lineNumberTextBox.Text = "";
            AddLineNumbers();
            lineNumberTextBox.Invalidate();
        }

        private void contentRichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (contentRichTextBox.Text == "")
            {
                AddLineNumbers();
            }
            
        }

        private void MyNotepad_Resize(object sender, EventArgs e)
        {
            AddLineNumbers();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contentRichTextBox.Paste();
        }

        private void fontsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //fontDialog.ShowDialog();
            

            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                contentRichTextBox.SelectionFont = fontDialog.Font;
            }
        }
    }
}
