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
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace MTextEditor
{
    public partial class RM : Form
    {
        [DllImport("Project.dll")]
        private static extern void WriteCreateFile([In]char[] fileName, int size, [In, Out] char[] text);
        [DllImport("Project.dll")]
        private static extern void MyReadFromFile([In]char[] fileName, [In, Out] char[] text);
        public RM()
        {

            InitializeComponent();
            LastSearchText = " ";
            //FileName = null;
            //FileName.FileName = null;
        }

        public string LastSearchText;
        public FileDialog FileName;
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
              richTextBox1.Clear();
        }
        public string f;
       
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Open File";
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Clear();
                FileName = openFile;
                 f = " ";
                 //MyReadFromFile(openFile.FileName.ToCharArray(), f.ToCharArray());
                //Console.WriteLine(f);
                 //richTextBox1.Text = f;
                using (StreamReader sr = new StreamReader(openFile.FileName))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (FileName != null && FileName.FileName != null) {
                using (StreamWriter sw = new StreamWriter(FileName.FileName))
                {
                    sw.Write(richTextBox1.Text);
                    sw.Close();
                }
            }
            else
            {
                saveAsToolStripMenuItem.PerformClick();
            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void Undo_Btn_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void Redo_Btn_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void SelectAll_Btn_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripButton.PerformClick();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openToolStripButton.PerformClick();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton.PerformClick();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "Save File";
            saveFile.DefaultExt = "txt";
            saveFile.AddExtension = true;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                FileName = saveFile;
                WriteCreateFile(saveFile.FileName.ToCharArray(), richTextBox1.Text.Length, richTextBox1.Text.ToCharArray());
                /*using (StreamWriter sw = new StreamWriter(saveFile.FileName))
                {
                    sw.Write(richTextBox1.Text);
                    sw.Close();
                }*/
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 0)
                Application.Exit();
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo_Btn.PerformClick();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo_Btn.PerformClick();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cutToolStripButton.PerformClick();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripButton.PerformClick();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripButton.PerformClick();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll_Btn.PerformClick();
        }

        private void Font_btn_Click(object sender, EventArgs e)
        {
            FontDialog Font = new FontDialog();
            if(Font.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = Font.Font;
            }
        }

        private void Color_Btn_Click(object sender, EventArgs e)
        {
            ColorDialog Color = new ColorDialog();
            if (Color.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = Color.Color;
            }
        }

        private void Search_Btn_Click(object sender, EventArgs e)
        {

            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
            richTextBox1.DeselectAll();

            bool NoChoice = false;
            bool Found = false;
            if (Search_Text.Text.Length != 0 )
            {
                int index = 0;
                while (index < richTextBox1.Text.LastIndexOf(Search_Text.Text))
                {
                    Found = true;
                    if (ChoiceS_Text.Text.Length != 0)
                    {
                        if (ChoiceS_Text.Text == "1")
                        {
                            NoChoice = true;
                            richTextBox1.Find(Search_Text.Text, index, richTextBox1.TextLength, RichTextBoxFinds.MatchCase);
                        }
                        else if (ChoiceS_Text.Text == "2")
                        {
                            NoChoice = true;
                            Search_Text.Text = Search_Text.Text.ToLower();
                            richTextBox1.Find(Search_Text.Text, index, richTextBox1.TextLength, RichTextBoxFinds.WholeWord);
                        }
                        else
                        {
                            NoChoice = false;
                            break;
                        }
                        richTextBox1.SelectionBackColor = Color.Gray;
                        //index = richTextBox1.Text.IndexOf(Search_Text.Text, index) + 1;
                        index = richTextBox1.Text.IndexOf(Search_Text.Text, index, StringComparison.InvariantCultureIgnoreCase) + 1;
                    }
                    else
                    {
                        NoChoice = false;
                        break;
                    }
                }
                if (!Found)
                {
                    MessageBox.Show("Not Found");
                }
                else if (NoChoice == false)
                 MessageBox.Show("1 : Case Sensitive.\n2 : Not Case Sensitive.");
            }
        }
    }
}
