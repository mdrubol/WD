<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConvertStatements.aspx.cs" Inherits="WD.UI.ConvertStatements" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" ng-app="myApp">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <base href="/" />
    <title>Conver Statements</title>
    <link href="App/css/bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        .active {
            color: #FFF;
        }
    </style>
    <!--angular js for routing-->
<script src="<%=ResolveClientUrl("~/Scripts/angular.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/Scripts/angular-ui-router.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/Scripts/angular-resource.min.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/App/js/app.js")%>" type="text/javascript"></script>
<!--dataSet-->
<script src="<%=ResolveClientUrl("~/App/dataSet/controllers.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/App/dataSet/dataFactory.js")%>" type="text/javascript"></script>
<!--dataTable-->
<script src="<%=ResolveClientUrl("~/App/dataTable/controllers.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/App/dataTable/tableFactory.js")%>" type="text/javascript"></script>
<!--executeNonQuery-->
<script src="<%=ResolveClientUrl("~/App/executeNonQuery/controllers.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/App/executeNonQuery/executeNonQueryFactory.js")%>" type="text/javascript"></script>
<!--executeScalar-->
 <script src="<%=ResolveClientUrl("~/App/executeScalar/controllers.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/App/executeScalar/executeScalarFactory.js")%>" type="text/javascript"></script>

    <!--executeFilter-->
<script src="<%=ResolveClientUrl("~/App/executeFilter/controllers.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/App/executeFilter/executeFilterFactory.js")%>" type="text/javascript"></script>

        <!--executeEncrypt-->
<script src="<%=ResolveClientUrl("~/App/executeEncrypt/controllers.js")%>" type="text/javascript"></script>
<script src="<%=ResolveClientUrl("~/App/executeEncrypt/executeEncryptFactory.js")%>" type="text/javascript"></script>
<!-- angular JS Ends-->

    
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-inverse navbar-fixed-header" id="navBar" runat="server">
        <div class="container-fluid">
            <div class="navbar-header">
                <a class="navbar-brand active" href="/">WD</a>
            </div>
            <ul class="nav navbar-nav">
                <li class="{{dataset}}"><a href="#DataSet">DataSet</a></li>
                <li class="{{datatable}}"><a href="#DataTable">DataTable</a></li>
                <li class="{{executeNonQuery}}"><a href="#executeNonQuery">ExecuteNonQuery</a></li>
                 <li class="{{executeScalar}}"><a href="#executeScalar">ExecuteScalar</a></li>
                   <li class="{{executeFilter}}"><a href="#executeFilter">Filter</a></li>
                  <li class="{{executeEncrypt}}"><a href="#executeEncrypt">Encrypt</a></li>
            </ul>
        </div>
       </nav>
        <div class="container">
            <div class="row">
                <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-warning" Visible="false"></asp:Label>
            </div>
            <div class="clearfix"></div>
             <div class="row">
                 <div class="col-lg-5">
                            Source&nbsp;
                        <asp:DropDownList ID="ddlSource" runat="server">
                        <asp:ListItem Text="Sql" Value="sql"></asp:ListItem>
                        <asp:ListItem Text="Db2" Value="db2"></asp:ListItem>
                        <asp:ListItem Text="Oracle" Value="oracle"></asp:ListItem>
                        <asp:ListItem Text="TeraData" Value="teradata"></asp:ListItem>
                       </asp:DropDownList>
                        </div>
                  <div class="col-lg-1"></div>
                 <div class="col-lg-5">
                            Target&nbsp;
                        <asp:DropDownList ID="ddlTarget" runat="server">
                        <asp:ListItem Text="Sql" Value="sql"></asp:ListItem>
                        <asp:ListItem Text="Db2" Value="db2"></asp:ListItem>
                        <asp:ListItem Text="Oracle" Value="oracle"></asp:ListItem>
                        <asp:ListItem Text="TeraData" Value="teradata"></asp:ListItem>
                       </asp:DropDownList>
                        </div>
                 <div class="col-lg-1">
                    <asp:Button CssClass="btn btn-primary" Text="Convert" runat="server" ID="btnConvert" OnClick="btnConvert_Click" />
                </div>
            </div>
            
            <div class="row">
                <div class="col-lg-5">
                     <div class="row">
                        <div class="col-lg-12">
                        <asp:TextBox ID="txtSource" runat="server" TextMode="MultiLine" Rows="30" Columns="90"></asp:TextBox>
                        </div>
                    </div>
                </div>
                  <div class="col-lg-1"></div>
                <div class="col-lg-5">
                     <div class="row">
                        <div class="col-lg-12">
                        <asp:TextBox ID="txtTarget" runat="server" TextMode="MultiLine" Rows="30" Columns="90"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        
        </div>
    </form>

</body>
</html>

