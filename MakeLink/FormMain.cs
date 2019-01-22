using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MakeLink
{
    public partial class FormMain : Form
    {
        private bool isFolder = false;
        private string targetName = string.Empty;

        public FormMain()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.Text = Application.ProductName +
                " v" + Application.ProductVersion +
                " - " + Application.CompanyName;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            DialogResult result = openFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                isFolder = false;
                textSource.Text = openFile.FileName;
                targetName = Path.GetFileName(openFile.FileName);

                radioButton1.Checked = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = false;
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                isFolder = true;
                textSource.Text = folderBrowser.SelectedPath;
                targetName = Path.GetFileName(folderBrowser.SelectedPath);

                radioButton1.Checked = true;
                radioButton2.Enabled = false;
                radioButton3.Enabled = true;
            }
        }

        private void btnSaveLink_Click(object sender, EventArgs e)
        {
            saveFile.FileName = targetName;
            DialogResult result = saveFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                textOutput.Text = saveFile.FileName;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (File.Exists(textOutput.Text) || Directory.Exists(textOutput.Text))
            {
                MessageBox.Show("Abort! Output is already exist.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string argument = null;
                if (radioButton2.Checked)
                    argument = "/h";
                else if (radioButton3.Checked)
                    argument = "/j";
                else if (isFolder)
                    argument = "/d";

                bool result = MakeLink.Create(argument, textOutput.Text, textSource.Text);

                if (result)
                    MessageBox.Show($"Done! Link created :\r\n{textOutput.Text}", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Something went wrong!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void textCheck_Changed(object sender, EventArgs e)
        {
            isFolder = Directory.Exists(textSource.Text);
            btnCreate.Enabled = ((textSource.Text != string.Empty) && (textOutput.Text != string.Empty) &&
                (File.Exists(textSource.Text) || Directory.Exists(textSource.Text)));
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            textSource.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }
    }
}
