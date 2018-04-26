using System.Text.RegularExpressions;
using System.Windows.Forms;
using WD.XLC.Domain.Entities;
namespace WD.XLC.WIN.Controls
{
    partial class CreateOrEditTemplateControl
    {
        private readonly AppConfig appConfig = null;
        private readonly Mapping mapper = null;
        public delegate void SaveChange(AppConfig appConfig);
        public SaveChange SaveChanges;
        public delegate void ResetChange();
        public ResetChange ResetChanges;
        private  readonly int max;
        public OnCheckTable CheckTable;
        public delegate object[] OnCheckTable(string connString,int dbProvider);

        public OnCheckDuplicate CheckDuplicate;
        public delegate bool OnCheckDuplicate(AppConfig info);
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
            System.GC.SuppressFinalize(this);
        }
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new MetroFramework.Controls.MetroButton();
            this.btnAddTemplate = new MetroFramework.Controls.MetroButton();
            this.txtTemplateName = new MetroFramework.Controls.MetroTextBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.cbxServer = new MetroFramework.Controls.MetroComboBox();
            this.cbxTable = new MetroFramework.Controls.MetroComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(3, 160);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseSelectable = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddTemplate
            // 
            this.btnAddTemplate.AutoSize = true;
            this.btnAddTemplate.Location = new System.Drawing.Point(3, 131);
            this.btnAddTemplate.Name = "btnAddTemplate";
            this.btnAddTemplate.Size = new System.Drawing.Size(150, 23);
            this.btnAddTemplate.TabIndex = 19;
            this.btnAddTemplate.Text = "Add Template";
            this.btnAddTemplate.UseSelectable = true;
            this.btnAddTemplate.Click += new System.EventHandler(this.btnAddTemplate_Click);
            // 
            // txtTemplateName
            // 
            // 
            // 
            // 
            this.txtTemplateName.CustomButton.Image = null;
            this.txtTemplateName.CustomButton.Location = new System.Drawing.Point(131, 1);
            this.txtTemplateName.CustomButton.Name = "";
            this.txtTemplateName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtTemplateName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtTemplateName.CustomButton.TabIndex = 1;
            this.txtTemplateName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtTemplateName.CustomButton.UseSelectable = true;
            this.txtTemplateName.CustomButton.Visible = false;
            this.txtTemplateName.Lines = new string[0];
            this.txtTemplateName.Location = new System.Drawing.Point(0, 9);
            this.txtTemplateName.MaxLength = 32767;
            this.txtTemplateName.Name = "txtTemplateName";
            this.txtTemplateName.PasswordChar = '\0';
            this.txtTemplateName.PromptText = "Template Name";
            this.txtTemplateName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtTemplateName.SelectedText = "";
            this.txtTemplateName.SelectionLength = 0;
            this.txtTemplateName.SelectionStart = 0;
            this.txtTemplateName.ShortcutsEnabled = true;
            this.txtTemplateName.Size = new System.Drawing.Size(150, 23);
            this.txtTemplateName.TabIndex = 11;
            this.txtTemplateName.UseSelectable = true;
            this.txtTemplateName.WaterMark = "Template Name";
            this.txtTemplateName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtTemplateName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Location = new System.Drawing.Point(3, 108);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(67, 17);
            this.chkIsActive.TabIndex = 21;
            this.chkIsActive.Text = "Is Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // cbxServer
            // 
            this.cbxServer.FormattingEnabled = true;
            this.cbxServer.ItemHeight = 23;
            this.cbxServer.Items.AddRange(new object[] {
            "Sql",
            "Oracle",
            "Db2",
            "Other"});
            this.cbxServer.Location = new System.Drawing.Point(0, 38);
            this.cbxServer.Name = "cbxServer";
            this.cbxServer.PromptText = "Choose server";
            this.cbxServer.Size = new System.Drawing.Size(150, 29);
            this.cbxServer.TabIndex = 22;
            this.cbxServer.Text = "Choose server";
            this.cbxServer.UseSelectable = true;
            this.cbxServer.SelectionChangeCommitted += new System.EventHandler(this.cbxServer_SelectionChangeCommitted);
            // 
            // cbxTable
            // 
            this.cbxTable.FormattingEnabled = true;
            this.cbxTable.ItemHeight = 23;
            this.cbxTable.Location = new System.Drawing.Point(0, 73);
            this.cbxTable.Name = "cbxTable";
            this.cbxTable.PromptText = "Choose Table";
            this.cbxTable.Size = new System.Drawing.Size(150, 29);
            this.cbxTable.Sorted = true;
            this.cbxTable.TabIndex = 23;
            this.cbxTable.Text = "Choose Table";
            this.cbxTable.UseSelectable = true;
            // 
            // CreateOrEditTemplateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbxTable);
            this.Controls.Add(this.cbxServer);
            this.Controls.Add(this.chkIsActive);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddTemplate);
            this.Controls.Add(this.txtTemplateName);
            this.Name = "CreateOrEditTemplateControl";
            this.Size = new System.Drawing.Size(153, 188);
            this.Resize += new System.EventHandler(this.CreateOrEditTemplateControl_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnCancel;
        private MetroFramework.Controls.MetroButton btnAddTemplate;
        private MetroFramework.Controls.MetroTextBox txtTemplateName;
        private CheckBox chkIsActive;
        private MetroFramework.Controls.MetroComboBox cbxServer;
        private MetroFramework.Controls.MetroComboBox cbxTable;
    }
}
