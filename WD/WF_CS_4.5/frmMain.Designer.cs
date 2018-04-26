namespace WF_CS_4._5
{
    partial class frmMain
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtExecNonQuery = new System.Windows.Forms.TextBox();
            this.btnExecNonQuery = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGetData = new System.Windows.Forms.TabPage();
            this.lblExecDataTableStatus = new System.Windows.Forms.Label();
            this.btnExecDataTable = new System.Windows.Forms.Button();
            this.txtExecDataTable = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgExecuteDataTable = new System.Windows.Forms.DataGridView();
            this.tpExecNonQuery = new System.Windows.Forms.TabPage();
            this.btnInsert = new System.Windows.Forms.Button();
            this.dpDateOfJoining = new System.Windows.Forms.DateTimePicker();
            this.txtLName = new System.Windows.Forms.TextBox();
            this.txtMName = new System.Windows.Forms.TextBox();
            this.txtFName = new System.Windows.Forms.TextBox();
            this.txtEmployeeId = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblExecNonQueryStatus = new System.Windows.Forms.Label();
            this.tpEntity = new System.Windows.Forms.TabPage();
            this.btnGetList = new System.Windows.Forms.Button();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblGetEntityStatus = new System.Windows.Forms.Label();
            this.btnGetEntity = new System.Windows.Forms.Button();
            this.txtEmpId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rbMitec = new System.Windows.Forms.RadioButton();
            this.rbDbLog = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tpGetData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgExecuteDataTable)).BeginInit();
            this.tpExecNonQuery.SuspendLayout();
            this.tpEntity.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SQL Statement";
            // 
            // txtExecNonQuery
            // 
            this.txtExecNonQuery.Location = new System.Drawing.Point(9, 28);
            this.txtExecNonQuery.Multiline = true;
            this.txtExecNonQuery.Name = "txtExecNonQuery";
            this.txtExecNonQuery.Size = new System.Drawing.Size(698, 53);
            this.txtExecNonQuery.TabIndex = 1;
            this.txtExecNonQuery.Text = "Insert into tempEmployee(EmployeeId, FirstName, MiddleName, LastName, DateOfJoini" +
    "ng,Provider) Values(786,\'Muhammad\',\'Asim\',\'Naeem\',\'1/12/2016\',\'SQL\')";
            // 
            // btnExecNonQuery
            // 
            this.btnExecNonQuery.Location = new System.Drawing.Point(572, 92);
            this.btnExecNonQuery.Name = "btnExecNonQuery";
            this.btnExecNonQuery.Size = new System.Drawing.Size(135, 23);
            this.btnExecNonQuery.TabIndex = 2;
            this.btnExecNonQuery.Text = "Execute Non-Query";
            this.btnExecNonQuery.UseVisualStyleBackColor = true;
            this.btnExecNonQuery.Click += new System.EventHandler(this.btnExecNonQuery_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpGetData);
            this.tabControl1.Controls.Add(this.tpExecNonQuery);
            this.tabControl1.Controls.Add(this.tpEntity);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(728, 416);
            this.tabControl1.TabIndex = 3;
            // 
            // tpGetData
            // 
            this.tpGetData.Controls.Add(this.rbDbLog);
            this.tpGetData.Controls.Add(this.rbMitec);
            this.tpGetData.Controls.Add(this.lblExecDataTableStatus);
            this.tpGetData.Controls.Add(this.btnExecDataTable);
            this.tpGetData.Controls.Add(this.txtExecDataTable);
            this.tpGetData.Controls.Add(this.label3);
            this.tpGetData.Controls.Add(this.dgExecuteDataTable);
            this.tpGetData.Location = new System.Drawing.Point(4, 22);
            this.tpGetData.Name = "tpGetData";
            this.tpGetData.Padding = new System.Windows.Forms.Padding(3);
            this.tpGetData.Size = new System.Drawing.Size(720, 390);
            this.tpGetData.TabIndex = 1;
            this.tpGetData.Text = "Execute DataTable";
            this.tpGetData.UseVisualStyleBackColor = true;
            // 
            // lblExecDataTableStatus
            // 
            this.lblExecDataTableStatus.AutoSize = true;
            this.lblExecDataTableStatus.Location = new System.Drawing.Point(8, 80);
            this.lblExecDataTableStatus.Name = "lblExecDataTableStatus";
            this.lblExecDataTableStatus.Size = new System.Drawing.Size(40, 13);
            this.lblExecDataTableStatus.TabIndex = 12;
            this.lblExecDataTableStatus.Text = "Status:";
            // 
            // btnExecDataTable
            // 
            this.btnExecDataTable.Location = new System.Drawing.Point(574, 45);
            this.btnExecDataTable.Name = "btnExecDataTable";
            this.btnExecDataTable.Size = new System.Drawing.Size(135, 23);
            this.btnExecDataTable.TabIndex = 11;
            this.btnExecDataTable.Text = "Execute Data Table";
            this.btnExecDataTable.UseVisualStyleBackColor = true;
            this.btnExecDataTable.Click += new System.EventHandler(this.btnExecDataTable_Click);
            // 
            // txtExecDataTable
            // 
            this.txtExecDataTable.Location = new System.Drawing.Point(11, 19);
            this.txtExecDataTable.Name = "txtExecDataTable";
            this.txtExecDataTable.Size = new System.Drawing.Size(698, 20);
            this.txtExecDataTable.TabIndex = 10;
            this.txtExecDataTable.Text = "Select * from tempEmployee";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "SQL Statement";
            // 
            // dgExecuteDataTable
            // 
            this.dgExecuteDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgExecuteDataTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgExecuteDataTable.Location = new System.Drawing.Point(3, 96);
            this.dgExecuteDataTable.Name = "dgExecuteDataTable";
            this.dgExecuteDataTable.Size = new System.Drawing.Size(714, 291);
            this.dgExecuteDataTable.TabIndex = 8;
            // 
            // tpExecNonQuery
            // 
            this.tpExecNonQuery.Controls.Add(this.btnInsert);
            this.tpExecNonQuery.Controls.Add(this.dpDateOfJoining);
            this.tpExecNonQuery.Controls.Add(this.txtLName);
            this.tpExecNonQuery.Controls.Add(this.txtMName);
            this.tpExecNonQuery.Controls.Add(this.txtFName);
            this.tpExecNonQuery.Controls.Add(this.txtEmployeeId);
            this.tpExecNonQuery.Controls.Add(this.label9);
            this.tpExecNonQuery.Controls.Add(this.label8);
            this.tpExecNonQuery.Controls.Add(this.label7);
            this.tpExecNonQuery.Controls.Add(this.label4);
            this.tpExecNonQuery.Controls.Add(this.label2);
            this.tpExecNonQuery.Controls.Add(this.lblExecNonQueryStatus);
            this.tpExecNonQuery.Controls.Add(this.btnExecNonQuery);
            this.tpExecNonQuery.Controls.Add(this.txtExecNonQuery);
            this.tpExecNonQuery.Controls.Add(this.label1);
            this.tpExecNonQuery.Location = new System.Drawing.Point(4, 22);
            this.tpExecNonQuery.Name = "tpExecNonQuery";
            this.tpExecNonQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tpExecNonQuery.Size = new System.Drawing.Size(720, 390);
            this.tpExecNonQuery.TabIndex = 0;
            this.tpExecNonQuery.Text = "Execute Non-Query";
            this.tpExecNonQuery.UseVisualStyleBackColor = true;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(216, 292);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(135, 23);
            this.btnInsert.TabIndex = 14;
            this.btnInsert.Text = "Insert";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // dpDateOfJoining
            // 
            this.dpDateOfJoining.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpDateOfJoining.Location = new System.Drawing.Point(108, 257);
            this.dpDateOfJoining.Name = "dpDateOfJoining";
            this.dpDateOfJoining.Size = new System.Drawing.Size(243, 20);
            this.dpDateOfJoining.TabIndex = 13;
            this.dpDateOfJoining.Value = new System.DateTime(2017, 2, 22, 14, 34, 41, 0);
            // 
            // txtLName
            // 
            this.txtLName.Location = new System.Drawing.Point(108, 227);
            this.txtLName.Name = "txtLName";
            this.txtLName.Size = new System.Drawing.Size(243, 20);
            this.txtLName.TabIndex = 12;
            // 
            // txtMName
            // 
            this.txtMName.Location = new System.Drawing.Point(108, 200);
            this.txtMName.Name = "txtMName";
            this.txtMName.Size = new System.Drawing.Size(243, 20);
            this.txtMName.TabIndex = 11;
            // 
            // txtFName
            // 
            this.txtFName.Location = new System.Drawing.Point(108, 173);
            this.txtFName.Name = "txtFName";
            this.txtFName.Size = new System.Drawing.Size(243, 20);
            this.txtFName.TabIndex = 10;
            // 
            // txtEmployeeId
            // 
            this.txtEmployeeId.Location = new System.Drawing.Point(108, 146);
            this.txtEmployeeId.Name = "txtEmployeeId";
            this.txtEmployeeId.Size = new System.Drawing.Size(243, 20);
            this.txtEmployeeId.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 257);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Date Of Joining";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 230);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Last Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Middle Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "First Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Employee ID";
            // 
            // lblExecNonQueryStatus
            // 
            this.lblExecNonQueryStatus.AutoSize = true;
            this.lblExecNonQueryStatus.Location = new System.Drawing.Point(6, 97);
            this.lblExecNonQueryStatus.Name = "lblExecNonQueryStatus";
            this.lblExecNonQueryStatus.Size = new System.Drawing.Size(40, 13);
            this.lblExecNonQueryStatus.TabIndex = 3;
            this.lblExecNonQueryStatus.Text = "Status:";
            // 
            // tpEntity
            // 
            this.tpEntity.Controls.Add(this.btnGetList);
            this.tpEntity.Controls.Add(this.txtFirstName);
            this.tpEntity.Controls.Add(this.label6);
            this.tpEntity.Controls.Add(this.lblGetEntityStatus);
            this.tpEntity.Controls.Add(this.btnGetEntity);
            this.tpEntity.Controls.Add(this.txtEmpId);
            this.tpEntity.Controls.Add(this.label5);
            this.tpEntity.Location = new System.Drawing.Point(4, 22);
            this.tpEntity.Name = "tpEntity";
            this.tpEntity.Size = new System.Drawing.Size(720, 390);
            this.tpEntity.TabIndex = 2;
            this.tpEntity.Text = "Get Entity";
            this.tpEntity.UseVisualStyleBackColor = true;
            // 
            // btnGetList
            // 
            this.btnGetList.Location = new System.Drawing.Point(208, 46);
            this.btnGetList.Name = "btnGetList";
            this.btnGetList.Size = new System.Drawing.Size(135, 23);
            this.btnGetList.TabIndex = 19;
            this.btnGetList.Text = "GetList";
            this.btnGetList.UseVisualStyleBackColor = true;
            this.btnGetList.Click += new System.EventHandler(this.btnGetList_Click);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(208, 20);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(135, 20);
            this.txtFirstName.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(205, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "FirstName";
            // 
            // lblGetEntityStatus
            // 
            this.lblGetEntityStatus.AutoSize = true;
            this.lblGetEntityStatus.Location = new System.Drawing.Point(3, 126);
            this.lblGetEntityStatus.Name = "lblGetEntityStatus";
            this.lblGetEntityStatus.Size = new System.Drawing.Size(40, 13);
            this.lblGetEntityStatus.TabIndex = 16;
            this.lblGetEntityStatus.Text = "Status:";
            // 
            // btnGetEntity
            // 
            this.btnGetEntity.Location = new System.Drawing.Point(8, 46);
            this.btnGetEntity.Name = "btnGetEntity";
            this.btnGetEntity.Size = new System.Drawing.Size(135, 23);
            this.btnGetEntity.TabIndex = 15;
            this.btnGetEntity.Text = "GetEntity";
            this.btnGetEntity.UseVisualStyleBackColor = true;
            this.btnGetEntity.Click += new System.EventHandler(this.btnGetEntity_Click);
            // 
            // txtEmpId
            // 
            this.txtEmpId.Location = new System.Drawing.Point(8, 20);
            this.txtEmpId.Name = "txtEmpId";
            this.txtEmpId.Size = new System.Drawing.Size(135, 20);
            this.txtEmpId.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "EmployeeID";
            // 
            // rbMitec
            // 
            this.rbMitec.AutoSize = true;
            this.rbMitec.Location = new System.Drawing.Point(11, 45);
            this.rbMitec.Name = "rbMitec";
            this.rbMitec.Size = new System.Drawing.Size(65, 17);
            this.rbMitec.TabIndex = 13;
            this.rbMitec.TabStop = true;
            this.rbMitec.Text = "MITECS";
            this.rbMitec.UseVisualStyleBackColor = true;
            this.rbMitec.CheckedChanged += new System.EventHandler(this.rbMitec_CheckedChanged);
            // 
            // rbDbLog
            // 
            this.rbDbLog.AutoSize = true;
            this.rbDbLog.Checked = true;
            this.rbDbLog.Location = new System.Drawing.Point(103, 45);
            this.rbDbLog.Name = "rbDbLog";
            this.rbDbLog.Size = new System.Drawing.Size(57, 17);
            this.rbDbLog.TabIndex = 14;
            this.rbDbLog.TabStop = true;
            this.rbDbLog.Text = "db_log";
            this.rbDbLog.UseVisualStyleBackColor = true;
            this.rbDbLog.CheckedChanged += new System.EventHandler(this.rbDbLog_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 416);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.Text = "Sample Application 4.5";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpGetData.ResumeLayout(false);
            this.tpGetData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgExecuteDataTable)).EndInit();
            this.tpExecNonQuery.ResumeLayout(false);
            this.tpExecNonQuery.PerformLayout();
            this.tpEntity.ResumeLayout(false);
            this.tpEntity.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExecNonQuery;
        private System.Windows.Forms.Button btnExecNonQuery;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpExecNonQuery;
        private System.Windows.Forms.TabPage tpGetData;
        private System.Windows.Forms.TabPage tpEntity;
        private System.Windows.Forms.Label lblExecNonQueryStatus;
        private System.Windows.Forms.DataGridView dgExecuteDataTable;
        private System.Windows.Forms.Label lblExecDataTableStatus;
        private System.Windows.Forms.Button btnExecDataTable;
        private System.Windows.Forms.TextBox txtExecDataTable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblGetEntityStatus;
        private System.Windows.Forms.Button btnGetEntity;
        private System.Windows.Forms.TextBox txtEmpId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGetList;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dpDateOfJoining;
        private System.Windows.Forms.TextBox txtLName;
        private System.Windows.Forms.TextBox txtMName;
        private System.Windows.Forms.TextBox txtFName;
        private System.Windows.Forms.TextBox txtEmployeeId;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.RadioButton rbDbLog;
        private System.Windows.Forms.RadioButton rbMitec;
    }
}

