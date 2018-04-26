<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebApiClient.aspx.cs" Inherits="WD.UI.WebApiClient" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" ng-app="myApp">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <base href="/" />
    <title>Web Client</title>
    <link href="App/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        .active {
            color: #FFF;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-inverse navbar-fixed-header">
            <div class="container-fluid">
                <div class="navbar-header">
                    <a class="navbar-brand active" href="/">WD</a>
                </div>
            </div>
        </nav>
        <div class="container">
            <div class="row">
                <div class="col-lg-2 ">
                    DbProvider
                </div>
                <div class="col-lg-3">
                    <asp:DropDownList ID="ddlDbType" runat="server">
                        <asp:ListItem Text="Sql" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Db2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Oracle" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Oracle2" Value="4"></asp:ListItem>
                        <asp:ListItem Text="TeraData" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2">
                    Connection String
                </div>
                <div class="col-lg-10">
                    <asp:TextBox ID="txtConnectionString" runat="server" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2">
                    Command Text
                </div>
                <div class="col-lg-10">
                    <asp:TextBox ID="txtCommandText" runat="server" Width="100%" Text="Select * from TempEmployee" TextMode="MultiLine" Rows="10" Columns="100"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2">
                    Command Type
                </div>
                <div class="col-lg-3">
                    <asp:DropDownList ID="ddlCommandType" runat="server">
                        <asp:ListItem Text="Text" Value="1"></asp:ListItem>
                        <asp:ListItem Text="StoredProcedure" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2 pull-right">
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-warning"  Text="Add Paramter" OnClick="btnAdd_Click" ValidationGroup="Params"  />
                </div>
            </div>
            <div class="row">
                <div class="col-lg-2">
                    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger" Visible="false"></asp:Label>
                    </div>
                </div>
            <div class="row">
                 <div class="col-lg-12">
                    <asp:GridView ID="grdParamter" runat="server" CssClass="table table-bordered" Width="100%" AutoGenerateColumns="false"  OnRowCommand="grdParamter_RowCommand" OnRowDeleting="grdParamter_RowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="Parameter Name">
                                <ItemTemplate>
                                  <asp:TextBox ID="txtParameterName" runat="server" Text='<%#Eval("ParameterName") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvParamName" runat="server" ValidationGroup="Params" ControlToValidate="txtParameterName" SetFocusOnError="true" ErrorMessage="Parameter Name Required!" Display="Dynamic"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parameter Value">
                                <ItemTemplate>
                                 <asp:TextBox ID="txtParameterValue" runat="server" Text='<%#Eval("ParameterValue") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvParamValue" runat="server" ValidationGroup="Params"  ControlToValidate="txtParameterValue" SetFocusOnError="true" ErrorMessage="Parameter Value Required!" Display="Dynamic"></asp:RequiredFieldValidator>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger"  CommandArgument='<%#Eval("ParameterName") %>' CommandName="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                      </div>
            </div>
             <div class="row">
                <div class="col-lg-2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                </div>
            </div>
            <div class="row">
                  <div class="col-lg-12">
                <asp:GridView ID="grdResult" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered"  Width="100%"  ></asp:GridView>
            </div>
                  </div>
        </div>
    </form>
</body>
</html>
