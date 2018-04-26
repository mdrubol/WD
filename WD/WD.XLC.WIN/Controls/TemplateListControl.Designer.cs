namespace WD.XLC.WIN.Controls
{
    partial class TemplateListControl
    {
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
            this.grdTemplateList = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdTemplateList)).BeginInit();
            this.SuspendLayout();
            // 
            // grdTemplateList
            // 
            this.grdTemplateList.AllowUserToAddRows = false;
            this.grdTemplateList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdTemplateList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTemplateList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.RecId,
            this.ServerInfo,
            this.TableName,
            this.CreatedBy,
            this.CreatedOn,
            this.IsActive});
            this.grdTemplateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTemplateList.Location = new System.Drawing.Point(0, 0);
            this.grdTemplateList.MultiSelect = false;
            this.grdTemplateList.Name = "grdTemplateList";
            this.grdTemplateList.ReadOnly = true;
            this.grdTemplateList.RowHeadersVisible = false;
            this.grdTemplateList.Size = new System.Drawing.Size(819, 700);
            this.grdTemplateList.TabIndex = 1;
            this.grdTemplateList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdTemplateList_CellDoubleClick);
            this.grdTemplateList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdTemplateList_KeyDown);
            this.grdTemplateList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdTemplateList_MouseClick);
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Id.Visible = false;
            // 
            // RecId
            // 
            this.RecId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RecId.DataPropertyName = "RecId";
            this.RecId.HeaderText = "Template Name";
            this.RecId.Name = "RecId";
            this.RecId.ReadOnly = true;
            // 
            // ServerInfo
            // 
            this.ServerInfo.DataPropertyName = "ServerName";
            this.ServerInfo.HeaderText = "Server Info";
            this.ServerInfo.Name = "ServerInfo";
            this.ServerInfo.ReadOnly = true;
            // 
            // TableName
            // 
            this.TableName.DataPropertyName = "TargetTableName";
            this.TableName.HeaderText = "Table Name";
            this.TableName.Name = "TableName";
            this.TableName.ReadOnly = true;
            // 
            // CreatedBy
            // 
            this.CreatedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            // 
            // CreatedOn
            // 
            this.CreatedOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CreatedOn.DataPropertyName = "CreatedOn";
            this.CreatedOn.HeaderText = "Created On";
            this.CreatedOn.Name = "CreatedOn";
            this.CreatedOn.ReadOnly = true;
            // 
            // IsActive
            // 
            this.IsActive.DataPropertyName = "IsActive";
            this.IsActive.HeaderText = "IsActive";
            this.IsActive.Name = "IsActive";
            this.IsActive.ReadOnly = true;
            // 
            // TemplateListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdTemplateList);
            this.Name = "TemplateListControl";
            this.Size = new System.Drawing.Size(819, 700);
            ((System.ComponentModel.ISupportInitialize)(this.grdTemplateList)).EndInit();
            this.ResumeLayout(false);

        }

       

      

        #endregion

        private System.Windows.Forms.DataGridView grdTemplateList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServerInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedOn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsActive;
    }
}
