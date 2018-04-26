'Add references
Imports WD.DataAccess.Abstract
Imports WD.DataAccess.Context
Imports WD.DataAccess.Enums
Imports WD.DataAccess.Parameters

Public Class frmMain
    Dim dbp As Integer
    Private Sub btnExecNonQuery_Click(sender As Object, e As EventArgs) Handles btnExecNonQuery.Click
        Dim dbContaxt As New DbContext(dbp)
        Dim command As ICommands
        command = dbContaxt.ICommands
        Dim rowsAffected As Integer = command.ExecuteNonQuery(txtExecNonQuery.Text)
        lblExecNonQueryStatus.Text = String.Format("Status: {0} rows effected", rowsAffected)
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbp = DBProvider.Sql
    End Sub

    Private Sub btnInsert_Click(sender As Object, e As EventArgs) Handles btnInsert.Click
        Dim strQ As String = String.Format("Insert Into tempEmployee(EmployeeId, FirstName, MiddleName, LastName, DateOfJoining,Provider) Values({0},'{1}','{2}','{3}','{4}','{5}')", txtEmployeeId.Text, txtFName.Text, txtMName.Text, txtLName.Text, dpDateOfJoining.Value.Date.ToString("MM/dd/yyyy"), dbp.ToString())


        Dim dbContaxt As New DbContext(dbp)
        Dim command As ICommands
        command = dbContaxt.ICommands
        Dim rowsAffected As Integer = command.ExecuteNonQuery(strQ)
        lblExecNonQueryStatus.Text = String.Format("Status: {0} rows effected", rowsAffected)


        txtEmployeeId.Text = ""
        txtFName.Text = ""
        txtLName.Text = ""
        txtMName.Text = ""
    End Sub

    Private Sub btnExecDataTable_Click(sender As Object, e As EventArgs) Handles btnExecDataTable.Click
        ' Get the command object by initializing the DBContext.
        ' Choose which Database to connect. (In my case I am using SQL server so DBProvider.Sql)
        Dim command As ICommands = New DbContext(dbp).ICommands

        Dim dt As DataTable = command.ExecuteDataTable(txtExecDataTable.Text)

            

        lblExecDataTableStatus.Text = String.Format("Status: Rows selected={0}", dt.Rows.Count)
        dgExecDataTable.DataSource = dt
    End Sub
End Class
