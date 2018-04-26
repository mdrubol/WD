namespace WD.XLC.WIN.PopUps
{
    partial class FormFolder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFolder));
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.btnAdd = new MetroFramework.Controls.MetroButton();
            this.txtFolderLocation = new MetroFramework.Controls.MetroTextBox();
            this.txtFolderExtension = new MetroFramework.Controls.MetroTextBox();
            this.btnBrowse = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(9, 184);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(411, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.Location = new System.Drawing.Point(9, 148);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(411, 23);
            this.btnAdd.TabIndex = 25;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseSelectable = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtFolderLocation
            // 
            // 
            // 
            // 
            this.txtFolderLocation.CustomButton.Image = null;
            this.txtFolderLocation.CustomButton.Location = new System.Drawing.Point(303, 1);
            this.txtFolderLocation.CustomButton.Name = "";
            this.txtFolderLocation.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtFolderLocation.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFolderLocation.CustomButton.TabIndex = 1;
            this.txtFolderLocation.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFolderLocation.CustomButton.UseSelectable = true;
            this.txtFolderLocation.CustomButton.Visible = false;
            this.txtFolderLocation.Lines = new string[0];
            this.txtFolderLocation.Location = new System.Drawing.Point(9, 76);
            this.txtFolderLocation.MaxLength = 32767;
            this.txtFolderLocation.Name = "txtFolderLocation";
            this.txtFolderLocation.PasswordChar = '\0';
            this.txtFolderLocation.PromptText = "Folder Location";
            this.txtFolderLocation.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFolderLocation.SelectedText = "";
            this.txtFolderLocation.SelectionLength = 0;
            this.txtFolderLocation.SelectionStart = 0;
            this.txtFolderLocation.ShortcutsEnabled = true;
            this.txtFolderLocation.Size = new System.Drawing.Size(325, 23);
            this.txtFolderLocation.TabIndex = 24;
            this.txtFolderLocation.UseSelectable = true;
            this.txtFolderLocation.WaterMark = "Folder Location";
            this.txtFolderLocation.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFolderLocation.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtFolderExtension
            // 
            // 
            // 
            // 
            this.txtFolderExtension.CustomButton.Image = null;
            this.txtFolderExtension.CustomButton.Location = new System.Drawing.Point(389, 1);
            this.txtFolderExtension.CustomButton.Name = "";
            this.txtFolderExtension.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtFolderExtension.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtFolderExtension.CustomButton.TabIndex = 1;
            this.txtFolderExtension.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtFolderExtension.CustomButton.UseSelectable = true;
            this.txtFolderExtension.CustomButton.Visible = false;
            this.txtFolderExtension.Lines = new string[0];
            this.txtFolderExtension.Location = new System.Drawing.Point(9, 112);
            this.txtFolderExtension.MaxLength = 32767;
            this.txtFolderExtension.Name = "txtFolderExtension";
            this.txtFolderExtension.PasswordChar = '\0';
            this.txtFolderExtension.PromptText = "File Extension";
            this.txtFolderExtension.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFolderExtension.SelectedText = "";
            this.txtFolderExtension.SelectionLength = 0;
            this.txtFolderExtension.SelectionStart = 0;
            this.txtFolderExtension.ShortcutsEnabled = true;
            this.txtFolderExtension.Size = new System.Drawing.Size(411, 23);
            this.txtFolderExtension.TabIndex = 28;
            this.txtFolderExtension.UseSelectable = true;
            this.txtFolderExtension.WaterMark = "File Extension";
            this.txtFolderExtension.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtFolderExtension.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(341, 76);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 29;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseSelectable = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // FormFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 276);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFolderExtension);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtFolderLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFolder";
            this.Text = "Folder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnCancel;
        private MetroFramework.Controls.MetroButton btnAdd;
        private MetroFramework.Controls.MetroTextBox txtFolderLocation;
        private MetroFramework.Controls.MetroTextBox txtFolderExtension;
        private MetroFramework.Controls.MetroButton btnBrowse;

    }
}