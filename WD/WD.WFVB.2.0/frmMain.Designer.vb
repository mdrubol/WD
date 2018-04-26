<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tpExecNonQuery = New System.Windows.Forms.TabPage()
        Me.tpExecDataTable = New System.Windows.Forms.TabPage()
        Me.lblExecNonQueryStatus = New System.Windows.Forms.Label()
        Me.btnExecNonQuery = New System.Windows.Forms.Button()
        Me.txtExecNonQuery = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.dpDateOfJoining = New System.Windows.Forms.DateTimePicker()
        Me.txtLName = New System.Windows.Forms.TextBox()
        Me.txtMName = New System.Windows.Forms.TextBox()
        Me.txtFName = New System.Windows.Forms.TextBox()
        Me.txtEmployeeId = New System.Windows.Forms.TextBox()
        Me.label9 = New System.Windows.Forms.Label()
        Me.label8 = New System.Windows.Forms.Label()
        Me.label7 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.dgExecDataTable = New System.Windows.Forms.DataGridView()
        Me.lblExecDataTableStatus = New System.Windows.Forms.Label()
        Me.btnExecDataTable = New System.Windows.Forms.Button()
        Me.txtExecDataTable = New System.Windows.Forms.TextBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.tpExecNonQuery.SuspendLayout()
        Me.tpExecDataTable.SuspendLayout()
        CType(Me.dgExecDataTable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpExecNonQuery)
        Me.TabControl1.Controls.Add(Me.tpExecDataTable)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(728, 416)
        Me.TabControl1.TabIndex = 0
        '
        'tpExecNonQuery
        '
        Me.tpExecNonQuery.Controls.Add(Me.btnInsert)
        Me.tpExecNonQuery.Controls.Add(Me.dpDateOfJoining)
        Me.tpExecNonQuery.Controls.Add(Me.txtLName)
        Me.tpExecNonQuery.Controls.Add(Me.txtMName)
        Me.tpExecNonQuery.Controls.Add(Me.txtFName)
        Me.tpExecNonQuery.Controls.Add(Me.txtEmployeeId)
        Me.tpExecNonQuery.Controls.Add(Me.label9)
        Me.tpExecNonQuery.Controls.Add(Me.label8)
        Me.tpExecNonQuery.Controls.Add(Me.label7)
        Me.tpExecNonQuery.Controls.Add(Me.label4)
        Me.tpExecNonQuery.Controls.Add(Me.label2)
        Me.tpExecNonQuery.Controls.Add(Me.lblExecNonQueryStatus)
        Me.tpExecNonQuery.Controls.Add(Me.btnExecNonQuery)
        Me.tpExecNonQuery.Controls.Add(Me.txtExecNonQuery)
        Me.tpExecNonQuery.Controls.Add(Me.label1)
        Me.tpExecNonQuery.Location = New System.Drawing.Point(4, 22)
        Me.tpExecNonQuery.Name = "tpExecNonQuery"
        Me.tpExecNonQuery.Padding = New System.Windows.Forms.Padding(3)
        Me.tpExecNonQuery.Size = New System.Drawing.Size(720, 390)
        Me.tpExecNonQuery.TabIndex = 0
        Me.tpExecNonQuery.Text = "Execute Non-Query"
        Me.tpExecNonQuery.UseVisualStyleBackColor = True
        '
        'tpExecDataTable
        '
        Me.tpExecDataTable.Controls.Add(Me.dgExecDataTable)
        Me.tpExecDataTable.Controls.Add(Me.lblExecDataTableStatus)
        Me.tpExecDataTable.Controls.Add(Me.btnExecDataTable)
        Me.tpExecDataTable.Controls.Add(Me.txtExecDataTable)
        Me.tpExecDataTable.Controls.Add(Me.label3)
        Me.tpExecDataTable.Location = New System.Drawing.Point(4, 22)
        Me.tpExecDataTable.Name = "tpExecDataTable"
        Me.tpExecDataTable.Padding = New System.Windows.Forms.Padding(3)
        Me.tpExecDataTable.Size = New System.Drawing.Size(720, 390)
        Me.tpExecDataTable.TabIndex = 1
        Me.tpExecDataTable.Text = "Execute DataTable"
        Me.tpExecDataTable.UseVisualStyleBackColor = True
        '
        'lblExecNonQueryStatus
        '
        Me.lblExecNonQueryStatus.AutoSize = True
        Me.lblExecNonQueryStatus.Location = New System.Drawing.Point(6, 54)
        Me.lblExecNonQueryStatus.Name = "lblExecNonQueryStatus"
        Me.lblExecNonQueryStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblExecNonQueryStatus.TabIndex = 22
        Me.lblExecNonQueryStatus.Text = "Status:"
        '
        'btnExecNonQuery
        '
        Me.btnExecNonQuery.Location = New System.Drawing.Point(572, 54)
        Me.btnExecNonQuery.Name = "btnExecNonQuery"
        Me.btnExecNonQuery.Size = New System.Drawing.Size(135, 23)
        Me.btnExecNonQuery.TabIndex = 21
        Me.btnExecNonQuery.Text = "Execute Non-Query"
        Me.btnExecNonQuery.UseVisualStyleBackColor = True
        '
        'txtExecNonQuery
        '
        Me.txtExecNonQuery.Location = New System.Drawing.Point(9, 28)
        Me.txtExecNonQuery.Name = "txtExecNonQuery"
        Me.txtExecNonQuery.Size = New System.Drawing.Size(698, 20)
        Me.txtExecNonQuery.TabIndex = 20
        Me.txtExecNonQuery.Text = "Insert into tempEmployee(EmployeeId, FirstName, MiddleName, LastName, DateOfJoini" & _
    "ng,Provider) Values(786,'Muhammad','Asim','Naeem','1/12/2016','SQL')"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(6, 12)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(79, 13)
        Me.label1.TabIndex = 19
        Me.label1.Text = "SQL Statement"
        '
        'btnInsert
        '
        Me.btnInsert.Location = New System.Drawing.Point(242, 289)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(135, 23)
        Me.btnInsert.TabIndex = 40
        Me.btnInsert.Text = "Insert"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'dpDateOfJoining
        '
        Me.dpDateOfJoining.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dpDateOfJoining.Location = New System.Drawing.Point(134, 254)
        Me.dpDateOfJoining.Name = "dpDateOfJoining"
        Me.dpDateOfJoining.Size = New System.Drawing.Size(243, 20)
        Me.dpDateOfJoining.TabIndex = 39
        Me.dpDateOfJoining.Value = New Date(2017, 2, 22, 14, 34, 41, 0)
        '
        'txtLName
        '
        Me.txtLName.Location = New System.Drawing.Point(134, 224)
        Me.txtLName.Name = "txtLName"
        Me.txtLName.Size = New System.Drawing.Size(243, 20)
        Me.txtLName.TabIndex = 38
        '
        'txtMName
        '
        Me.txtMName.Location = New System.Drawing.Point(134, 197)
        Me.txtMName.Name = "txtMName"
        Me.txtMName.Size = New System.Drawing.Size(243, 20)
        Me.txtMName.TabIndex = 37
        '
        'txtFName
        '
        Me.txtFName.Location = New System.Drawing.Point(134, 170)
        Me.txtFName.Name = "txtFName"
        Me.txtFName.Size = New System.Drawing.Size(243, 20)
        Me.txtFName.TabIndex = 36
        '
        'txtEmployeeId
        '
        Me.txtEmployeeId.Location = New System.Drawing.Point(134, 143)
        Me.txtEmployeeId.Name = "txtEmployeeId"
        Me.txtEmployeeId.Size = New System.Drawing.Size(243, 20)
        Me.txtEmployeeId.TabIndex = 35
        '
        'label9
        '
        Me.label9.AutoSize = True
        Me.label9.Location = New System.Drawing.Point(35, 254)
        Me.label9.Name = "label9"
        Me.label9.Size = New System.Drawing.Size(80, 13)
        Me.label9.TabIndex = 34
        Me.label9.Text = "Date Of Joining"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(57, 227)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(58, 13)
        Me.label8.TabIndex = 33
        Me.label8.Text = "Last Name"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(46, 200)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(69, 13)
        Me.label7.TabIndex = 32
        Me.label7.Text = "Middle Name"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(58, 173)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(57, 13)
        Me.label4.TabIndex = 31
        Me.label4.Text = "First Name"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(48, 146)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(67, 13)
        Me.label2.TabIndex = 30
        Me.label2.Text = "Employee ID"
        '
        'dgExecDataTable
        '
        Me.dgExecDataTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgExecDataTable.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgExecDataTable.Location = New System.Drawing.Point(3, 92)
        Me.dgExecDataTable.Name = "dgExecDataTable"
        Me.dgExecDataTable.Size = New System.Drawing.Size(714, 295)
        Me.dgExecDataTable.TabIndex = 22
        '
        'lblExecDataTableStatus
        '
        Me.lblExecDataTableStatus.AutoSize = True
        Me.lblExecDataTableStatus.Location = New System.Drawing.Point(10, 47)
        Me.lblExecDataTableStatus.Name = "lblExecDataTableStatus"
        Me.lblExecDataTableStatus.Size = New System.Drawing.Size(40, 13)
        Me.lblExecDataTableStatus.TabIndex = 21
        Me.lblExecDataTableStatus.Text = "Status:"
        '
        'btnExecDataTable
        '
        Me.btnExecDataTable.Location = New System.Drawing.Point(576, 47)
        Me.btnExecDataTable.Name = "btnExecDataTable"
        Me.btnExecDataTable.Size = New System.Drawing.Size(135, 23)
        Me.btnExecDataTable.TabIndex = 20
        Me.btnExecDataTable.Text = "Execute Data Table"
        Me.btnExecDataTable.UseVisualStyleBackColor = True
        '
        'txtExecDataTable
        '
        Me.txtExecDataTable.Location = New System.Drawing.Point(13, 21)
        Me.txtExecDataTable.Name = "txtExecDataTable"
        Me.txtExecDataTable.Size = New System.Drawing.Size(698, 20)
        Me.txtExecDataTable.TabIndex = 19
        Me.txtExecDataTable.Text = "Select * from tempEmployee"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(10, 5)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(79, 13)
        Me.label3.TabIndex = 18
        Me.label3.Text = "SQL Statement"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(728, 416)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "frmMain"
        Me.Text = "Windows Form VB 2.0"
        Me.TabControl1.ResumeLayout(False)
        Me.tpExecNonQuery.ResumeLayout(False)
        Me.tpExecNonQuery.PerformLayout()
        Me.tpExecDataTable.ResumeLayout(False)
        Me.tpExecDataTable.PerformLayout()
        CType(Me.dgExecDataTable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tpExecNonQuery As System.Windows.Forms.TabPage
    Friend WithEvents tpExecDataTable As System.Windows.Forms.TabPage
    Private WithEvents lblExecNonQueryStatus As System.Windows.Forms.Label
    Private WithEvents btnExecNonQuery As System.Windows.Forms.Button
    Private WithEvents txtExecNonQuery As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents btnInsert As System.Windows.Forms.Button
    Private WithEvents dpDateOfJoining As System.Windows.Forms.DateTimePicker
    Private WithEvents txtLName As System.Windows.Forms.TextBox
    Private WithEvents txtMName As System.Windows.Forms.TextBox
    Private WithEvents txtFName As System.Windows.Forms.TextBox
    Private WithEvents txtEmployeeId As System.Windows.Forms.TextBox
    Private WithEvents label9 As System.Windows.Forms.Label
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents dgExecDataTable As System.Windows.Forms.DataGridView
    Private WithEvents lblExecDataTableStatus As System.Windows.Forms.Label
    Private WithEvents btnExecDataTable As System.Windows.Forms.Button
    Private WithEvents txtExecDataTable As System.Windows.Forms.TextBox
    Private WithEvents label3 As System.Windows.Forms.Label

End Class
