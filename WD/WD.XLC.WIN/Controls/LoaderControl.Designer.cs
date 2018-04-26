using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WD.DataAccess.Context;
using WD.XLC.Domain.Entities;
namespace WD.XLC.WIN.Controls
{
    partial class LoaderControl
    {
        public delegate void OnSaveFileCount(int aCount);
        public OnSaveFileCount SaveFileCount;
        /// <summary>   The instance. </summary>
        private MyInstance instance = null;
       
        private readonly ServerInfo serverInfo = null;
        private readonly int max;
       
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
            // Suppress finalization.
            System.GC.SuppressFinalize(this);
        }
        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoaderControl));
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.leftSplitContainer = new System.Windows.Forms.SplitContainer();
            this.grdFolderList = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckFolder = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FolderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdTemplateList = new System.Windows.Forms.DataGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TemplateName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Config = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetTable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConnString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DbProvider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rightSplitContainer = new System.Windows.Forms.SplitContainer();
            this.pnlToggle = new System.Windows.Forms.Panel();
            this.toggleStartProcess = new MetroFramework.Controls.MetroToggle();
            this.lblTimer = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblTotalFiles = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.timerLoad = new System.Windows.Forms.Timer(this.components);
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblEventLog = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblAverageTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBrowse = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmbxLog = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftSplitContainer)).BeginInit();
            this.leftSplitContainer.Panel1.SuspendLayout();
            this.leftSplitContainer.Panel2.SuspendLayout();
            this.leftSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFolderList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemplateList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).BeginInit();
            this.rightSplitContainer.Panel1.SuspendLayout();
            this.rightSplitContainer.Panel2.SuspendLayout();
            this.rightSplitContainer.SuspendLayout();
            this.pnlToggle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.leftSplitContainer);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.rightSplitContainer);
            this.mainSplitContainer.Size = new System.Drawing.Size(819, 664);
            this.mainSplitContainer.SplitterDistance = 273;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // leftSplitContainer
            // 
            this.leftSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.leftSplitContainer.Name = "leftSplitContainer";
            this.leftSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // leftSplitContainer.Panel1
            // 
            this.leftSplitContainer.Panel1.Controls.Add(this.grdFolderList);
            // 
            // leftSplitContainer.Panel2
            // 
            this.leftSplitContainer.Panel2.Controls.Add(this.grdTemplateList);
            this.leftSplitContainer.Size = new System.Drawing.Size(273, 664);
            this.leftSplitContainer.SplitterDistance = 306;
            this.leftSplitContainer.TabIndex = 0;
            // 
            // grdFolderList
            // 
            this.grdFolderList.AllowUserToAddRows = false;
            this.grdFolderList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFolderList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.CheckFolder,
            this.FolderName,
            this.FileExtension});
            this.grdFolderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFolderList.Location = new System.Drawing.Point(0, 0);
            this.grdFolderList.Name = "grdFolderList";
            this.grdFolderList.RowHeadersVisible = false;
            this.grdFolderList.Size = new System.Drawing.Size(273, 306);
            this.grdFolderList.TabIndex = 0;
            this.grdFolderList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdFolderList_MouseClick);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // CheckFolder
            // 
            this.CheckFolder.HeaderText = "";
            this.CheckFolder.Name = "CheckFolder";
            this.CheckFolder.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CheckFolder.Width = 30;
            // 
            // FolderName
            // 
            this.FolderName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FolderName.HeaderText = "Folder";
            this.FolderName.Name = "FolderName";
            this.FolderName.ReadOnly = true;
            // 
            // FileExtension
            // 
            this.FileExtension.HeaderText = "Extension";
            this.FileExtension.Name = "FileExtension";
            this.FileExtension.Width = 55;
            // 
            // grdTemplateList
            // 
            this.grdTemplateList.AllowUserToAddRows = false;
            this.grdTemplateList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdTemplateList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTemplateList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.TemplateName,
            this.AppId,
            this.Config,
            this.TargetTable,
            this.ConnString,
            this.DbProvider});
            this.grdTemplateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTemplateList.Location = new System.Drawing.Point(0, 0);
            this.grdTemplateList.Name = "grdTemplateList";
            this.grdTemplateList.RowHeadersVisible = false;
            this.grdTemplateList.Size = new System.Drawing.Size(273, 354);
            this.grdTemplateList.TabIndex = 2;
            // 
            // Check
            // 
            this.Check.DataPropertyName = "CheckBox";
            this.Check.FillWeight = 52.10839F;
            this.Check.HeaderText = "";
            this.Check.Name = "Check";
            this.Check.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TemplateName
            // 
            this.TemplateName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TemplateName.DataPropertyName = "RecId";
            this.TemplateName.FillWeight = 290.07F;
            this.TemplateName.HeaderText = "Template";
            this.TemplateName.Name = "TemplateName";
            this.TemplateName.ReadOnly = true;
            // 
            // AppId
            // 
            this.AppId.DataPropertyName = "Id";
            this.AppId.HeaderText = "";
            this.AppId.Name = "AppId";
            this.AppId.ReadOnly = true;
            this.AppId.Visible = false;
            // 
            // Config
            // 
            this.Config.DataPropertyName = "Config";
            this.Config.HeaderText = "";
            this.Config.Name = "Config";
            this.Config.ReadOnly = true;
            this.Config.Visible = false;
            // 
            // TargetTable
            // 
            this.TargetTable.DataPropertyName = "TargetTableName";
            this.TargetTable.HeaderText = "";
            this.TargetTable.Name = "TargetTable";
            this.TargetTable.ReadOnly = true;
            this.TargetTable.Visible = false;
            // 
            // ConnString
            // 
            this.ConnString.DataPropertyName = "ConnString";
            this.ConnString.HeaderText = "";
            this.ConnString.Name = "ConnString";
            this.ConnString.ReadOnly = true;
            this.ConnString.Visible = false;
            // 
            // DbProvider
            // 
            this.DbProvider.DataPropertyName = "DbProvider";
            this.DbProvider.HeaderText = "";
            this.DbProvider.Name = "DbProvider";
            this.DbProvider.ReadOnly = true;
            this.DbProvider.Visible = false;
            // 
            // rightSplitContainer
            // 
            this.rightSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.rightSplitContainer.Name = "rightSplitContainer";
            this.rightSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // rightSplitContainer.Panel1
            // 
            this.rightSplitContainer.Panel1.Controls.Add(this.pnlToggle);
            // 
            // rightSplitContainer.Panel2
            // 
            this.rightSplitContainer.Panel2.Controls.Add(this.splitContainer1);
            this.rightSplitContainer.Size = new System.Drawing.Size(542, 664);
            this.rightSplitContainer.SplitterDistance = 25;
            this.rightSplitContainer.TabIndex = 0;
            // 
            // pnlToggle
            // 
            this.pnlToggle.Controls.Add(this.toggleStartProcess);
            this.pnlToggle.Controls.Add(this.lblTimer);
            this.pnlToggle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlToggle.Location = new System.Drawing.Point(0, 0);
            this.pnlToggle.Name = "pnlToggle";
            this.pnlToggle.Size = new System.Drawing.Size(542, 25);
            this.pnlToggle.TabIndex = 0;
            // 
            // toggleStartProcess
            // 
            this.toggleStartProcess.AutoSize = true;
            this.toggleStartProcess.Dock = System.Windows.Forms.DockStyle.Left;
            this.toggleStartProcess.Location = new System.Drawing.Point(0, 0);
            this.toggleStartProcess.Name = "toggleStartProcess";
            this.toggleStartProcess.Size = new System.Drawing.Size(80, 25);
            this.toggleStartProcess.TabIndex = 3;
            this.toggleStartProcess.Text = "Off";
            this.toggleStartProcess.UseSelectable = true;
            this.toggleStartProcess.CheckedChanged += new System.EventHandler(this.toggleStartProcess_CheckedChanged);
            // 
            // lblTimer
            // 
            this.lblTimer.AutoSize = true;
            this.lblTimer.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimer.Location = new System.Drawing.Point(542, 0);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(0, 15);
            this.lblTimer.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblTotalFiles);
            this.splitContainer1.Panel1.Controls.Add(this.lblInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtLog);
            this.splitContainer1.Size = new System.Drawing.Size(542, 635);
            this.splitContainer1.SplitterDistance = 30;
            this.splitContainer1.TabIndex = 0;
            // 
            // lblTotalFiles
            // 
            this.lblTotalFiles.AutoSize = true;
            this.lblTotalFiles.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTotalFiles.Location = new System.Drawing.Point(542, 0);
            this.lblTotalFiles.Name = "lblTotalFiles";
            this.lblTotalFiles.Size = new System.Drawing.Size(0, 13);
            this.lblTotalFiles.TabIndex = 1;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(0, 13);
            this.lblInfo.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(542, 601);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // timerLoad
            // 
            this.timerLoad.Interval = 1;
            this.timerLoad.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.mainSplitContainer);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer4.Size = new System.Drawing.Size(819, 700);
            this.splitContainer4.SplitterDistance = 664;
            this.splitContainer4.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblEventLog,
            this.toolStripSeparator1,
            this.lblAverageTime,
            this.toolStripSeparator2,
            this.btnBrowse,
            this.toolStripSeparator4,
            this.cmbxLog});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(819, 32);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // lblEventLog
            // 
            this.lblEventLog.Name = "lblEventLog";
            this.lblEventLog.Size = new System.Drawing.Size(119, 29);
            this.lblEventLog.Text = "Time Elapsed : 0:0:0:0";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // lblAverageTime
            // 
            this.lblAverageTime.Name = "lblAverageTime";
            this.lblAverageTime.Size = new System.Drawing.Size(148, 29);
            this.lblAverageTime.Text = "Avg Time: 0.0 sec / 0 file(s)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnBrowse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBrowse.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowse.Image")));
            this.btnBrowse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(23, 29);
            this.btnBrowse.Text = "Show Log";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
            // 
            // cmbxLog
            // 
            this.cmbxLog.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmbxLog.Name = "cmbxLog";
            this.cmbxLog.Size = new System.Drawing.Size(121, 32);
            this.cmbxLog.SelectedIndexChanged += new System.EventHandler(this.cmbxLog_SelectedIndexChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCheckBoxColumn1.Width = 30;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Folder";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "CheckBox";
            this.dataGridViewCheckBoxColumn2.HeaderText = "";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "RecId";
            this.dataGridViewTextBoxColumn2.FillWeight = 169.5432F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Template";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn3.HeaderText = "";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Config";
            this.dataGridViewTextBoxColumn4.HeaderText = "";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "TargetTableName";
            this.dataGridViewTextBoxColumn5.HeaderText = "";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "ConnString";
            this.dataGridViewTextBoxColumn6.HeaderText = "";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "DbProvider";
            this.dataGridViewTextBoxColumn7.HeaderText = "";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // LoaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer4);
            this.Name = "LoaderControl";
            this.Size = new System.Drawing.Size(819, 700);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.leftSplitContainer.Panel1.ResumeLayout(false);
            this.leftSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftSplitContainer)).EndInit();
            this.leftSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFolderList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemplateList)).EndInit();
            this.rightSplitContainer.Panel1.ResumeLayout(false);
            this.rightSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).EndInit();
            this.rightSplitContainer.ResumeLayout(false);
            this.pnlToggle.ResumeLayout(false);
            this.pnlToggle.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

       

        #endregion

        private SplitContainer mainSplitContainer;
        private SplitContainer leftSplitContainer;
        private SplitContainer rightSplitContainer;
        private DataGridView grdFolderList;
        private Panel pnlToggle;
        private DataGridView grdTemplateList;
        private Label lblTimer;
        private SplitContainer splitContainer1;
        //private Label lblInfo;
        //private Label lblTotalFiles;
        private Timer timerLoad;
        private MetroFramework.Controls.MetroToggle toggleStartProcess;
        private SplitContainer splitContainer4;
        private Label lblInfo;
        private Label lblTotalFiles;
        private ToolStripSeparator toolStripSeparator3;
        private RichTextBox txtLog;
        private ToolStrip toolStrip1;
        private ToolStripLabel lblEventLog;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel lblAverageTime;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnBrowse;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripComboBox cmbxLog;
       
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewCheckBoxColumn Check;
        private DataGridViewTextBoxColumn TemplateName;
        private DataGridViewTextBoxColumn AppId;
        private DataGridViewTextBoxColumn Config;
        private DataGridViewTextBoxColumn TargetTable;
        private DataGridViewTextBoxColumn ConnString;
        private DataGridViewTextBoxColumn DbProvider;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewCheckBoxColumn CheckFolder;
        private DataGridViewTextBoxColumn FolderName;
        private DataGridViewTextBoxColumn FileExtension;
           
    }
}
