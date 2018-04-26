using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WD.XLC.Domain.Entities;

namespace WD.XLC.WIN.PopUps
{
    public partial class FormFolder : MetroFramework.Forms.MetroForm
    {
        public delegate bool AddFolder(FolderStructure fs);
        public AddFolder OnAddFolder;
        public FormFolder()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFolderLocation.Text))
            {
                try
                {
                    if (Directory.Exists(txtFolderLocation.Text) && !string.IsNullOrEmpty(txtFolderExtension.Text))
                    {
                        if (OnAddFolder(new FolderStructure()
                        {
                            Id = Guid.NewGuid().ToString(),
                            FolderName = txtFolderLocation.Text,
                            FolderExtension = txtFolderExtension.Text,
                            IsActive = 1,
                            CreatedBy = Environment.UserName,
                            CreatedOn = DateTime.UtcNow
                        }))
                        {
                            if (MessageBox.Show("Folder path added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                this.Dispose();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please entervalid folder location and valid file extension.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Please select folder location.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtFolderLocation.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
