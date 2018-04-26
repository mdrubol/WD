namespace WD.XLC.WIN.Controls
{
    partial class ServerInfoListControl
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
            this.grdServerInfoList = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdServerInfoList)).BeginInit();
            this.SuspendLayout();
            // 
            // grdServerInfoList
            // 
            this.grdServerInfoList.AllowUserToAddRows = false;
            this.grdServerInfoList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdServerInfoList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdServerInfoList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.ServerName,
            this.CreatedBy,
            this.CreatedOn});
            this.grdServerInfoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdServerInfoList.Location = new System.Drawing.Point(0, 0);
            this.grdServerInfoList.MultiSelect = false;
            this.grdServerInfoList.Name = "grdServerInfoList";
            this.grdServerInfoList.ReadOnly = true;
            this.grdServerInfoList.RowHeadersVisible = false;
            this.grdServerInfoList.Size = new System.Drawing.Size(819, 700);
            this.grdServerInfoList.TabIndex = 1;
            this.grdServerInfoList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdServerInfoList_CellDoubleClick);
            this.grdServerInfoList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdServerInfoList_KeyDown);
            this.grdServerInfoList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdServerInfoList_MouseClick);
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Id.DataPropertyName = "ID";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Id.Visible = false;
            // 
            // ServerName
            // 
            this.ServerName.DataPropertyName = "ServerName";
            this.ServerName.HeaderText = "Server";
            this.ServerName.Name = "ServerName";
            this.ServerName.ReadOnly = true;
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "CreatedBy";
            this.CreatedBy.HeaderText = "Created By";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            // 
            // CreatedOn
            // 
            this.CreatedOn.DataPropertyName = "CreatedOn";
            this.CreatedOn.HeaderText = "Created On";
            this.CreatedOn.Name = "CreatedOn";
            this.CreatedOn.ReadOnly = true;
            // 
            // ServerInfoListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdServerInfoList);
            this.Name = "ServerInfoListControl";
            this.Size = new System.Drawing.Size(819, 700);
            ((System.ComponentModel.ISupportInitialize)(this.grdServerInfoList)).EndInit();
            this.ResumeLayout(false);

        }

       

      

        #endregion

        private System.Windows.Forms.DataGridView grdServerInfoList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedOn;
    }
}
